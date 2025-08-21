using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddRazorPages();

services.Configure<RabbitOptions>(builder.Configuration.GetSection("Rabbit"));
services.AddSingleton<IConnectionFactory>(sp =>
{
    var o = sp.GetRequiredService<IOptions<RabbitOptions>>().Value;
    return new ConnectionFactory
    {
        HostName = o.Host,
        Port = o.Port,
        VirtualHost = o.VirtualHost,
        UserName = o.User,
        Password = o.Password,
        AutomaticRecoveryEnabled = false
    };
});

services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"."));

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();
