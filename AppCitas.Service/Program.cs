using AppCitas.Service.Data;
using Microsoft.EntityFrameworkCore;

namespace AppCitas;

public class Program
{
    public async static Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<DataContext>();
            await context.Database.MigrateAsync();
            await Seed.SeedUsers(context);
        }
        catch(Exception e)
        {
            var Logger = services.GetRequiredService<ILogger<Program>>();
            Logger.LogError(e, "An error ocurred during migration/Seeding");
        }

        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
