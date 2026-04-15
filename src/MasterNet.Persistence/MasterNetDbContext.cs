namespace MasterNet.Persistence;

using MasterNet.Domain;
using Microsoft.EntityFrameworkCore;
using Bogus;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Infrastructure;

public class MasterNetDbContext : DbContext
{
    public DbSet<Curso>? Cursos { get; set; }
    public DbSet<Instructor>? Instructores { get; set; }
    public DbSet<Precio>? Precios { get; set; }
    public DbSet<Calificacion>? Calificaciones { get; set; }

    public MasterNetDbContext(){}
    public MasterNetDbContext(DbContextOptions<MasterNetDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=LocalDatabase.db")
            .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning))
            .EnableDetailedErrors()
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .UseAsyncSeeding(async(context, status, cancellationToken) =>
            {
                var masterNetDbContext = (MasterNetDbContext)context;
                var logger = context.GetService<ILogger<MasterNetDbContext>>();
                
                try
                {
                    await SeedDatabase.SeedPreciosAsync(masterNetDbContext, logger, cancellationToken);
                    await SeedDatabase.SeedInstructoresAsync(masterNetDbContext, logger, cancellationToken);
                    await SeedDatabase.SeedCursosAsync(masterNetDbContext, logger, cancellationToken);
                    await SeedDatabase.SeedCalificacionesAsync(masterNetDbContext, logger, cancellationToken);
                }catch (Exception ex)
                {
                    logger?.LogError(ex, "Error en el seeding");
                }
            })
            .UseSeeding((context, status) =>
        {
            var masterNetDbContext = (MasterNetDbContext)context;
            var logger = context.GetService<ILogger<MasterNetDbContext>>();
            try
            {
                SeedDatabase.SeedPreciosAsync(masterNetDbContext, logger, CancellationToken.None).GetAwaiter().GetResult();
                SeedDatabase.SeedInstructoresAsync(masterNetDbContext, logger, CancellationToken.None).GetAwaiter().GetResult();
                SeedDatabase.SeedCursosAsync(masterNetDbContext, logger, CancellationToken.None).GetAwaiter().GetResult();
                SeedDatabase.SeedCalificacionesAsync(masterNetDbContext, logger, CancellationToken.None).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error en el seeding síncrono");
            }
        });
    }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Curso>().ToTable("cursos");
        modelBuilder.Entity<Instructor>().ToTable("instructores");
        modelBuilder.Entity<CursoInstructor>().ToTable("cursos_instructores");
        modelBuilder.Entity<Precio>().ToTable("precios");
        modelBuilder.Entity<CursoPrecio>().ToTable("cursos_precios");
        modelBuilder.Entity<Calificacion>().ToTable("calificaciones");
        modelBuilder.Entity<Photo>().ToTable("imagenes");

        modelBuilder.Entity<Precio>()
            .Property(b => b.PrecioActual)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Precio>()
            .Property(b => b.PrecioPromocion)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Precio>()
            .Property(b => b.Nombre)
            .HasColumnType("VARCHAR")
            .HasMaxLength(250);

        // Relaciones entre los modelos
        modelBuilder.Entity<Curso>()
            .HasMany(m => m.Photos)
            .WithOne(m => m.Curso)
            .HasForeignKey(m => m.CursoId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Curso>()
            .HasMany(m => m.Calificaciones)
            .WithOne(m => m.Curso)
            .HasForeignKey(m => m.CursoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Curso>()
            .HasMany(m => m.Precios)
            .WithMany(m => m.Cursos)
            .UsingEntity<CursoPrecio>(
                j => j
                    .HasOne(p => p.Precio)
                    .WithMany(p => p.CursoPrecios)
                    .HasForeignKey(cp => cp.PrecioId),
                j => j
                    .HasOne(c => c.Curso)
                    .WithMany(c => c.CursoPrecios)
                    .HasForeignKey(cp => cp.CursoId),
                j => 
                {
                    j.HasKey(t => new { t.CursoId, t.PrecioId });
                }
            );

        modelBuilder.Entity<Curso>()
            .HasMany(m => m.Instructores)
            .WithMany(m => m.Cursos)
            .UsingEntity<CursoInstructor>(
                j => j
                    .HasOne(i => i.Instructor)
                    .WithMany(i => i.CursoInstructores)
                    .HasForeignKey(ci => ci.InstructorId),
                j => j
                    .HasOne(c => c.Curso)
                    .WithMany(c => c.CursoInstructores)
                    .HasForeignKey(ci => ci.CursoId),
                j =>
                {
                    j.HasKey(t => new { t.CursoId, t.InstructorId });
                }
            );

        /* modelBuilder.Entity<Curso>()
            .HasData(
                DataMaster().Item1
            );
        modelBuilder.Entity<Instructor>()
            .HasData(
                DataMaster().Item2
            );
        modelBuilder.Entity<Precio>()
            .HasData(
                DataMaster().Item3
            );*/
    }

    public Tuple<Curso[], Instructor[], Precio[]> DataMaster()
    {
        var cursos = new List<Curso>();
        var faker = new Faker();
        for (int i = 0; i < 10; i++)
        {
            var cursoId = Guid.NewGuid();
            cursos.Add(
                new Curso
                {
                    Id = cursoId,
                    Descripcion = faker.Commerce.ProductDescription(),
                    Titulo = faker.Commerce.ProductName(),
                    FechaPublicacion = faker.Date.Past()
                }
            );
        }

        var precioId = Guid.NewGuid();
        var precio = new Precio
        {
            Id = precioId,
            Nombre = "Precio regular",
            PrecioActual = 10.00m,
            PrecioPromocion = 8.00m
        };
        var precios = new List<Precio> { 
            precio 
        };

        var fakerInstructor = new Faker<Instructor>()
            .RuleFor(i => i.Id, _ => Guid.NewGuid())
            .RuleFor(i => i.Nombre, f => f.Name.FirstName())
            .RuleFor(i => i.Apellidos, f => f.Name.LastName())
            .RuleFor(i => i.Grado, f => f.Name.JobTitle());
        var instructores = fakerInstructor.Generate(10);

        return Tuple.Create(cursos.ToArray(), instructores.ToArray(), precios.ToArray());
    }
}
