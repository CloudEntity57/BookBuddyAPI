using BookBuddyAPI.Data;
using BookBuddyAPI.Hubs.AppHub;
using BookBuddyAPI.Mappings;
using BookBuddyAPI.Repositories;
using BookBuddyAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using BookbuddyAPI.Services;
using BookBuddyAPI.Models.Domain;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
// Configure DbContext

var connectionString = builder.Configuration.GetConnectionString("BookBuddyGeneralConnectionString");
Console.WriteLine($"Connection string exists: {!string.IsNullOrEmpty(connectionString)} - {connectionString}");

builder.Services.AddDbContext<BookBuddyGeneralDbContext>(options =>
options.UseSqlServer(
    connectionString)
);

builder.Services.AddDbContext<BookBuddyAuthDbContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("BookBuddyAuthConnectionString"))
);

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Bind the secrets section to settings class
builder.Services.Configure<OAuthSettings>(
    builder.Configuration.GetSection("Authentication:Google"));


// Add repositories

builder.Services.AddScoped<IUserRepository, SQLUserRepository>();
builder.Services.AddScoped<IBookRepository, SQLBookRepository>();
builder.Services.AddScoped<IBuddyRepository, SQLBuddyRepository>();
builder.Services.AddScoped<INotificationsRepository, SQLNotificationsRepository>();
// builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IMessageRepository, SQLMessageRepository>();
builder.Services.AddScoped<IConversationRepository, SQLConversationRepository>();
builder.Services.AddScoped<IConversationMemberRepository, SQLConversationMemberRepository>();
builder.Services.AddScoped<IExternalLoginRepository, SQLExternalLoginRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

// Add services
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IBuddyRequestService, BuddyRequestService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Register custom OAuth service
builder.Services.AddScoped<IExternalAuthService, GoogleAuthService>();

//For the current single-server EC2 deployment, register an in-memory implementation of IDistributedCache:
builder.Services.AddDistributedMemoryCache();

builder.Services.AddHttpClient<IGoogleOAuthClient, GoogleOAuthClient>();

static async Task AddUserGuidClaim(TokenValidatedContext context, string userEmail)
{
    try
    {
        var serviceProvider = context.HttpContext.RequestServices;
        var dbContext = serviceProvider.GetService<BookBuddyGeneralDbContext>();
        if(dbContext != null)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == userEmail);
            if(user != null)
            {
                Debug.WriteLine($"User found for Guid claim: {user}");
                var identity = context.Principal?.Identity as ClaimsIdentity;
                identity.AddClaim(new Claim("user_guid", user.Id.ToString()));
                Debug.WriteLine($"Added user_guid claim: {user.Id}");
            }
        }
    }catch (Exception ex)
    {

    }
}

// 

// add authentication

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
                    )
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Query["access_token"];

                    Console.WriteLine($"Path: {context.HttpContext.Request.Path}");
                    Console.WriteLine($"Access token in query: {!string.IsNullOrEmpty(token)}");

                    if (!string.IsNullOrEmpty(token) &&
                        context.HttpContext.Request.Path.StartsWithSegments("/hubs/app"))
                    {
                        context.Token = token;
                    }

                    return Task.CompletedTask;
                },

                OnTokenValidated = context =>
                {
                    Console.WriteLine("JWT validated successfully.");

                    foreach (var claim in context.Principal!.Claims)
                    {
                        Console.WriteLine($"{claim.Type}: {claim.Value}");
                    }

                    return Task.CompletedTask;
                },

                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"Authentication failed: {context.Exception}");
                    return Task.CompletedTask;
                }
            };
    });


// add SignalR to services:

builder.Services.AddSignalR();

// Handle CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow Development Calls", policy =>
    {
        policy.WithOrigins([
            "http://localhost:4200", 
            "http://localhost:4000", 
            "http://bookbuddy-bucket-464788833046-us-east-2-an.s3-website.us-east-2.amazonaws.com",
            "https://joinbookbuddy.com"
        ])
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});


builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();




// never send back properties with null values

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

var app = builder.Build();



//app.Use(async (context, next) =>
//{
//    var query = context.Request.QueryString.ToString();
//    Console.WriteLine($"Query string: {query}");
//    await next();
//});

// Migrate latest DB context on startup:

using var scope = app.Services.CreateScope();


var db = scope.ServiceProvider
    .GetRequiredService<BookBuddyGeneralDbContext>();
var dbConnectionString = db.Database.GetConnectionString();

// Console.WriteLine($"DB connection: {dbConnectionString}");

db.Database.Migrate();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if(!app.Environment.IsDevelopment())
{
    app.UseForwardedHeaders();
    app.UseHttpsRedirection();
}
app.UseCors("Allow Development Calls");



// get user GUID from request headers and add a claim to use in targeted SignalR messages and notifications:
app.Use(async (context, next) =>
{
    if (context.Request.Headers.TryGetValue("X-User-Guid", out var userGuid))
    {
       var claims = new List<Claim> {
           new Claim("user_guid", userGuid)
       };

       Debug.WriteLine($"User guid: {userGuid} claims: {claims}");

       var identity = new ClaimsIdentity(claims, "Header");
       context.User.AddIdentity(identity);
    }
    // Debug.WriteLine($"SignalR request intercepted:");
    // Debug.WriteLine($"Path: {context.Request.Path}");
    // Debug.WriteLine($"Method: {context.Request.Method}");
    // Debug.WriteLine($"Query: {context.Request.QueryString}");
    // Debug.WriteLine($"Headers: {string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}={h.Value}"))}");
    if (context.Request.Path.StartsWithSegments("/hubs/app"))
    {
        Console.WriteLine($"SignalR request intercepted:");
        Console.WriteLine($"Path: {context.Request.Path}");
        Console.WriteLine($"Method: {context.Request.Method}");
        Console.WriteLine($"Query: {context.Request.QueryString}");
        Console.WriteLine($"Headers: {string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}={h.Value}"))}");

        var token = context.Request.Query["access_token"].FirstOrDefault();
        Console.WriteLine($"Access token in query: {!string.IsNullOrEmpty(token)} - {token}");

        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        Console.WriteLine($"Authorization header: {authHeader}");

    };
    await next();

});
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.MapHub<AppHub>("hubs/app");

app.Run();
