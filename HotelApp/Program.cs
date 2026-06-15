using HotelApp.Data;
using HotelApp.Repositories;
using HotelApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<BookingRepository>();
builder.Services.AddScoped<BookingService>();

builder.Services.AddScoped<GuestRepository>();
builder.Services.AddScoped<GuestService>();

builder.Services.AddScoped<RoomRepository>();
builder.Services.AddScoped<RoomService>();

builder.Services.AddScoped<ServiceRepository>();
builder.Services.AddScoped<ServiceService>();

var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

if (string.IsNullOrEmpty(databaseUrl))
{
    throw new Exception("DATABASE_URL not found");
}

var uri = new Uri(databaseUrl);

var userInfo = uri.UserInfo.Split(':');

var connectionString =
    $"Host={uri.Host};" +
    $"Port={uri.Port};" +
    $"Database={uri.AbsolutePath.TrimStart('/')};" +
    $"Username={userInfo[0]};" +
    $"Password={userInfo[1]};" +
    $"SSL Mode=Require;Trust Server Certificate=true;";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer("Server=.;Database=HotelAppDB;Trusted_Connection=True;TrustServerCertificate=True;"));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Test DB connection
/*
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}
*/
app.Run();