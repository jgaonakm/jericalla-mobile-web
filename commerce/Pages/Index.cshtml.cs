using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR.Client;


namespace commerce.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private static HubConnection _hubConnection;
    private static string site, whereiam;
    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
        // Used for identifying the site where the app is running and calculate the request # in the charts
        site = Environment.GetEnvironmentVariable("SITE")!;
        _logger.LogInformation($"Commerce site identified as {site}");
        whereiam = Environment.GetEnvironmentVariable("WHEREIAM");
        _logger.LogInformation($"Commerce app running on {whereiam}");

        // SignalR hub location where we notify a request was received
        var hub = Environment.GetEnvironmentVariable("HUB");
        if (string.IsNullOrEmpty(hub))
        {
            _logger.LogError("HUB environment variable is not set. Cannot connect to SignalR Hub.");
        }
        else
        {
            _logger.LogInformation($"Commerce app connecting to hub at {hub}");
            _hubConnection = new HubConnectionBuilder()
                    .WithUrl(hub!)
                    .WithAutomaticReconnect()
                    .Build();

            _hubConnection.StartAsync().Wait();
        }
    }
    public async void OnGet()
    {
        // Send the site name to the page for easy visualization. 
        // Notify the SignalR Hub we received a request
        if (_hubConnection != null)
        {
            _logger.LogInformation($"Broadcasting request count for site: {site}");
            await _hubConnection.InvokeAsync("BroadcastRequestCount", site);
        }
    }
}
