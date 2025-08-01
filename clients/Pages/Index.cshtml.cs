using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using System.Text.Json;

namespace clients.Pages;

public class IndexModel : PageModel
{
    public double UsageGB { get; set; }
    public int UsageMinutes { get; set; }
    public int UsageSMS { get; set; }

    public string FullName { get; set; } = "{Nombre}";
    public string Phone { get; set; } = "{Teléfono}";
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
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
                FullName = customer.FullName;
                Phone = customer.PhoneNumber;
            }
        }


        response = await httpClient.GetAsync($"{apiUrl}/usage");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Fetched customer usage data: {Json}", json);

            var usage = JsonSerializer.Deserialize<AccountUsage>(
                json, options
            );
            if (usage != null)
            {
                _logger.LogInformation("Parsed customer usage data: {0} GB, {1} min, {2} SMS", usage.DataUsedGB, usage.MinutesUsed, usage.SMSUsed);
                UsageGB = usage.DataUsedGB;
                UsageMinutes = usage.MinutesUsed;
                UsageSMS = usage.SMSUsed;
            }
        }
        else
        {
            _logger.LogWarning("Failed to fetch customer usage data: {StatusCode}", response.StatusCode);
        }

        return Page();
    }
}
