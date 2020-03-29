using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();

                webBuilder.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                                                                                .Enrich.FromLogContext()
                                                                                .WriteTo.Console(new RenderedCompactJsonFormatter()));
            });
}
