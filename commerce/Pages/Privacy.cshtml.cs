using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR.Client;


namespace commerce.Pages;

public class PrivacyModel : PageModel
{
    private static HubConnection _hubConnection;

    private readonly ILogger<PrivacyModel> _logger;

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
        // This page is used by Akamai GTM for health check purposes only. 
        // Here we use connection to the hub to cause the error in the site
        // by setting the environment variable as empty string 
        var hub = Environment.GetEnvironmentVariable("HUB");
        _hubConnection = new HubConnectionBuilder()
                .WithUrl(hub!)
                .WithAutomaticReconnect()
                .Build();

        _hubConnection.StartAsync().Wait();
    }

    public void OnGet()
    {

    }
}

