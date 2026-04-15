using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MasterNet.Persistence;
using MasterNet.Domain;

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

    var newCurso = new Curso
    {
        Id = Guid.NewGuid(),
        Titulo = "Programacion con .Net",
        Descripcion = "Aprende .Net",
        FechaPublicacion = DateTime.Now
    };
    
    context.Cursos!.Add(newCurso);
    await context.SaveChangesAsync();

    var cursos = await context.Cursos!.ToListAsync();

    foreach (var item in cursos)
    {
        Console.WriteLine($"{item.Id} - {item.Titulo}");   
    }
}
catch (Exception ex)
{
    Console.WriteLine("Error en el seeding/migration: " + ex.Message);
}
