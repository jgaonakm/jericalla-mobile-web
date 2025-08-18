using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Models;



public class ReservationsModel : PageModel
{
    ILogger<ReservationsModel> _logger;

    public ReservationsModel(ILogger<ReservationsModel> logger)
    {
        _logger = logger;
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


        if (Reservation.PaymentOption == PaymentOption.Installments && (Reservation.InstallmentMonths is null or <= 0))
        {
            ModelState.AddModelError("Reservation.InstallmentMonths", "Select the number of months.");
        }

        if (Reservation.HasTradeIn)
        {
            if (string.IsNullOrWhiteSpace(Reservation.TradeInBrand))
                ModelState.AddModelError("Reservation.TradeInBrand", "Enter your current phone brand.");
            if (string.IsNullOrWhiteSpace(Reservation.TradeInModel))
                ModelState.AddModelError("Reservation.TradeInModel", "Enter your current phone model.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "work-queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        const string message = "Hello World!";
        var body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
        _logger.LogInformation($" [x] Sent {message}");

        // TODO: Persist reservation, send confirmation, etc.
        TempData["Success"] = "Reservation received! We’ll contact you shortly.";
        return RedirectToPage("./ReservationConfirmation", new { reservation = JsonSerializer.Serialize(Reservation) });

        return Page();
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

