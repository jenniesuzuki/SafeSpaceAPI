using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SafeSpaceAPI.Infrastructure.Context;
using SafeSpaceAPI.Infrastructure.Persistence.Repositories;
using System.Text.Json.Serialization;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configurar logging detalhado para EF Core
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        // Filtro de logging para EF Core (Database.Command exibe SQL gerado)
        builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Information);

        // Add services to the container.
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SafeSpace API",
                Description = "API para gestão de usuários e suporte psicossocial.",
                Contact = new OpenApiContact() { Name = "Samir Hage Neto", Email = "samihneto@gmail.com" }
            });
        });

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Information)
                .AddConsole();
        });

        // Configuração do DbContext com Oracle
        builder.Services.AddDbContext<SafeSpaceContext>(options => {
            options
                .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging()   // Mostra valores dos parâmetros SQL no log
                .LogTo(Console.WriteLine, LogLevel.Information);  // Loga as mensagens no console
        });

        // Registro do repositório genérico
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        var app = builder.Build();

        // Aplicar migrations automaticamente
        //using (var scope = app.Services.CreateScope())
        //{
        //    var context = scope.ServiceProvider.GetRequiredService<SafeSpaceContext>();
        //    context.Database.Migrate();
        //}

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run("http://0.0.0.0:8080");
    }
}
