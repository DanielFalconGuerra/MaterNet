using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MasterNet.Persistence;

var service = new ServiceCollection();
service.AddLogging(l =>
{
    l.ClearProviders();
});
service.AddDbContext<MasterNetDbContext>();
var provider = service.BuildServiceProvider();
try
{
    using var scope = provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MasterNetDbContext>();
    await context.Database.MigrateAsync();
    Console.WriteLine("Migración y seeding completados exitosamente.");
}
catch (Exception ex)
{
    Console.WriteLine("Error en el seeding/migration: " + ex.Message);
}
