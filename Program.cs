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

        // Add services to the container.
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Serializa enums como string no JSON
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SafeSpace API",
                Description = "API para gest�o de usu�rios e suporte psicossocial.",
                Contact = new OpenApiContact() { Name = "Samir Hage Neto", Email = "samihneto@gmail.com" }
            });
        });

        // Configura��o do DbContext com Oracle
        builder.Services.AddDbContext<SafeSpaceContext>(options =>
            options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Registro do reposit�rio gen�rico
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
