using BookBuddyAPI.Data;
using BookBuddyAPI.Mappings;
using BookBuddyAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext

builder.Services.AddDbContext<BookBuddyGeneralDbContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("BookBuddyGeneralConnectionString"))
);

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Add repositories

builder.Services.AddScoped<IUserRepository, SQLUserRepository>();
builder.Services.AddScoped<IBookRepository, SQLBookRepository>();
builder.Services.AddScoped<IBuddyRepository, SQLBuddyRepository>();
builder.Services.AddScoped<INotificationsRepository, SQLNotificationsRepository>();

// Handle CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow Development Calls", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// never send back properties with null values

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

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

app.UseCors("Allow Development Calls");

app.Run();
