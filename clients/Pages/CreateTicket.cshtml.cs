using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using Models;

namespace MyApp.Namespace
{

    public class CreateTicketModel : PageModel
    {
        private readonly ILogger<CreateTicketModel> _logger;
        public Ticket Ticket { get; set; }

        public CreateTicketModel(ILogger<CreateTicketModel> logger)
        {
            _logger = logger;
        }


        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            int customerId = id ?? 101; // Default to 101 if no ID is provided
            Ticket.CustomerId = customerId.ToString();
            string baseUrl = Environment.GetEnvironmentVariable("SUPPORT_API") ?? "http://localhost:5006";
            _logger.LogInformation("Base URL for API: {BaseUrl}", baseUrl);
            string apiUrl = $"{baseUrl}/api/v2/support/ticket/";
            using var httpClient = new HttpClient();
            try //TODO: Implement retry pattern
            {
                var json = JsonSerializer.Serialize(Ticket);
                var ticket = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(apiUrl, ticket);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("Support");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving ticket for customer ID: {CustomerId}", customerId);
            }
            return Page();
        }
    }
}
