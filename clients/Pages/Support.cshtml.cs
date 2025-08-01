using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using System.Text.Json;

namespace MyApp.Namespace
{
    public class SupportModel : PageModel
    {
        private readonly ILogger<SupportModel> _logger;

        public IEnumerable<Ticket> Tickets { get; set; } = new List<Ticket>();

        public SupportModel(ILogger<SupportModel> logger)
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
            string baseUrl = Environment.GetEnvironmentVariable("SUPPORT_API") ?? "http://localhost:5006";
            _logger.LogInformation("Base URL for API: {BaseUrl}", baseUrl);
            string apiUrl = $"{baseUrl}/api/v2/support/tickets/customer/{customerId}";
            using var httpClient = new HttpClient();
            try //TODO: Implement retry pattern
            {
                var response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Fetched customer data: {Json}", json);

                    var tickets = JsonSerializer.Deserialize<IEnumerable<Ticket>>(
                        json, options
                    );
                    if (tickets != null)
                    {
                        _logger.LogInformation("Customer tickets parsed: {0}", tickets.Count());
                        Tickets = tickets;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tickets for customer ID: {CustomerId}", customerId);
            }
            return Page();
        }
    }
}
