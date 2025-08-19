using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Models;
using Microsoft.Extensions.Options;



public class ReservationsModel : PageModel
{
    ILogger<ReservationsModel> _logger;
    private readonly IConnectionFactory _connectionFactory;

    private readonly IOptionsMonitor<RabbitOptions> _options;
    private IConnection? _connection;
    private IChannel? _channel;

    public ReservationsModel(
        IConnectionFactory connectionFactory,
        IOptionsMonitor<RabbitOptions> options,
        ILogger<ReservationsModel> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _options = options;
    }

    [BindProperty]
    public PhoneReservation Reservation { get; set; } = new();

    public List<StorageOption> StorageOptions { get; private set; } = default!;
    public List<ColorOption> ColorOptions { get; private set; } = default!;


    public async void OnGetAsync(int? id)
    {
        LoadOptions();
        // Defaults
        if (Reservation.Color is null) Reservation.Color = "black";

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        int customerId = id ?? 101; // Default to 101 if no ID is provided
        Reservation.ClientId = customerId;

        string baseUrl = Environment.GetEnvironmentVariable("ACCOUNTS_API") ?? "http://localhost:5006";
        _logger.LogInformation("Base URL for API: {BaseUrl}", baseUrl);
        string apiUrl = $"{baseUrl}/api/v2/customer/{customerId}";
        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Fetched customer data: {Json}", json);

            var customer = JsonSerializer.Deserialize<Customer>(
                json, options
            );
            if (customer != null)
            {
                _logger.LogInformation("Parsed customer data: {0}", customer.FullName);
                Reservation.FullName = customer.FullName;
                Reservation.Email = customer.Email;
                Reservation.Phone = Regex.Replace(customer.PhoneNumber ?? string.Empty, "\\D*", "");
                Reservation.Phone = Reservation.Phone.PadRight(10, '0').Substring(0, 10);
                long phoneNumber = Int64.TryParse(Reservation.Phone, out phoneNumber) ? phoneNumber : 0;
                Reservation.Phone = string.Format("{0:(###) ###-####}", phoneNumber);
            }
        }
    }



    public async Task<IActionResult> OnPostAsync()
    {
        LoadOptions();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        // var factory = new ConnectionFactory { HostName = "localhost" };
        // using var connection = await factory.CreateConnectionAsync();
        // using var channel = await connection.CreateChannelAsync();
        _connection = await _connectionFactory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
 

        await _channel.QueueDeclareAsync(
            queue: _options.CurrentValue.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(Reservation));

        //TODO: Add a correlation ID 
        //Ref: https://www.rabbitmq.com/docs/publishers#message-properties
        var properties = new BasicProperties
        {
            AppId = "commerce-web",
            MessageId = Guid.NewGuid().ToString(),
            Type = "shiny-phone-reservation",
            DeliveryMode = DeliveryModes.Persistent,
            ContentType = "application/json",
            Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
        };

        await _channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: _options.CurrentValue.QueueName,
            mandatory: true,
            basicProperties: properties,
            body: body);
        _logger.LogInformation($" [x] Sent {properties.MessageId} to queue '{_options.CurrentValue.QueueName}'");


        TempData["Success"] = "¡Tu reserva ha sido confirmada!";
        return RedirectToPage("./ReservationConfirmation", new { reservation = JsonSerializer.Serialize(Reservation) });
    }

    private void LoadOptions()
    {
        StorageOptions = new()
        {
            new StorageOption("256 GB", "GB256"),
            new StorageOption("512 GB", "GB512"),
            new StorageOption("1 TB", "TB1")
        };

        ColorOptions = new()
        {
            new ColorOption("Negro","black","#111"),
            new ColorOption("Plata","silver","#c0c0c0"),
            new ColorOption("Azul","blue","#1e6bd6"),
            new ColorOption("Lavanda","lavender","#b57edc")
        };
    }
}

