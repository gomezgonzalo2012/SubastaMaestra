using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SubastaMaestra.API.HostedServices;
using SubastaMaestra.Data.Implements;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Data.Seeders;
using SubastaMaestra.Data;
using SubastaMaestra.Data;
using SubastaMaestra.Models.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<INotificationRepository, NotificacionRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
// servicio que maneja estado de rproductos y subastas
builder.Services.AddScoped<AuctionHandlerService>();
// evita bucles de serializacion
//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
//});



//
// autenticacion por jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ValidateIssuer = false,
            ValidateAudience= false,
            ValidateLifetime = true
        };
    });


builder.Services.AddHostedService<AuctionBackgroundService>();
// agregamos cors
builder.Services.AddCors(opciones =>
{
    opciones.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
        builder.SetIsOriginAllowed(origin=>true); // 
        
    });
    //opciones.AddPolicy("AllowSpecificOrigins", builder =>
    //{
    //    builder.WithOrigins("http://localhost:5157", "http://192.168.1.7:5083")
    //    .AllowAnyHeader()
    //    .AllowAnyMethod();
    //});
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


//// Configure the HTTP request pipeline.
//app.UseSwagger();

//if (app.Environment.IsDevelopment()) 
//{
//    app.UseSwaggerUI();
//}
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SubastaMaestra");
    });
}

app.UseHttpsRedirection();// DESCOMENTAR ANTES DE PUBLICAR
app.UseCors("AllowAll");
app.UseStaticFiles();

app.UseAuthentication(); // agregamos middleware de auth

app.UseAuthorization();

app.MapControllers();
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<SubastaContext>();
//    // Si llegas aquí sin error, el contexto está bien configurado.
//}


app.Run();

