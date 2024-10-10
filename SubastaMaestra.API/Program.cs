using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SubastaMaestra.API.HostedServices;
using SubastaMaestra.Data.Implements;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Data.Seeders;
using SubastaMaestra.Data.SubastaMaestra.Data;
using SubastaMaestra.Models.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Adding AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<SubastaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"))
    );
builder.Services.AddTransient<DbSeeder>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBidRepository, BidRepository>();


builder.Services.AddHostedService<AuctionBackgroundService>();
// agregamos cors
builder.Services.AddCors(opciones =>
{
    opciones.AddPolicy("newPolicy", app =>
    {
        app.AllowAnyOrigin();
        app.AllowAnyMethod();
        app.AllowAnyHeader();
        app.SetIsOriginAllowed(origin=>true); // 
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();

    try
    {
        // Ejecuta el seeder.
        await seeder.SeedAsync();
        Console.WriteLine("Base de datos inicializada correctamente.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al inicializar la base de datos: {ex.Message}");
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("newPolicy");

app.UseAuthorization();

app.MapControllers();
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<SubastaContext>();
//    // Si llegas aquí sin error, el contexto está bien configurado.
//}


app.Run();

