using BookBuddyAPI.Data;
using BookBuddyAPI.Hubs.Notifications;
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

builder.Services.AddDbContext<BookBuddyGeneralDbContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("BookBuddyGeneralConnectionString"))
);

builder.Services.AddDbContext<BookBuddyAuthDbContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("BookBuddyAuthConnectionString"))
);

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Add repositories

builder.Services.AddScoped<IUserRepository, SQLUserRepository>();
builder.Services.AddScoped<IBookRepository, SQLBookRepository>();
builder.Services.AddScoped<IBuddyRepository, SQLBuddyRepository>();
builder.Services.AddScoped<INotificationsRepository, SQLNotificationsRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IMessageRepository, SQLMessageRepository>();
builder.Services.AddScoped<IConversationRepository, SQLConversationRepository>();
builder.Services.AddScoped<IConversationMemberRepository, SQLConversationMemberRepository>();

// Add services
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IBuddyRequestService, BuddyRequestService>();


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


// add authentication

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
    {
        // Configure for OIDC/OAuth2 provider
        options.Authority = builder.Configuration["OAuth:Authority"]; // e.g., "https://accounts.google.com"
        options.Audience = builder.Configuration["OAuth:ClientId"]; // Your OAuth client ID
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            //ValidIssuer = "https://accounts.google.com",
            //ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //ValidAudience = "921071488707-kusrp5jrol9g7uekdgqbseqk6c5o8p07.apps.googleusercontent.com",
            //ValidAudience = builder.Configuration["Jwt:Audience"],
            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            // Prevent automatic claim mapping
            ClockSkew = TimeSpan.FromMinutes(5),
            RequireSignedTokens = false,
            NameClaimType = "name",
            RoleClaimType = "role"
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                Debug.WriteLine("OnMessageReceived Called");

                // This allows JWT to be passed via query string for WebSocket connections
                var accessToken = context.Request.Query["access_token"];

                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                var path = context.HttpContext.Request.Path;
                Debug.WriteLine($"OnMessageReceived - Path: {path}, Token present: {!string.IsNullOrEmpty(accessToken)}");

                if (authHeader != null)
                {
                    accessToken = authHeader.Substring("Bearer ".Length).Trim();
                }

                if (!string.IsNullOrEmpty(accessToken))
                {
                    Debug.WriteLine($"our negotiate query : {authHeader} and access token: {accessToken}");
                }


                // If the request is for SignalR hub

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/notifications"))
                {
                    Debug.WriteLine($"attaching this token: {accessToken}");
                    context.Token = accessToken;
                    Debug.WriteLine("Token set for SignalR connection");
                }


                return Task.CompletedTask;
            },
            OnTokenValidated = async context =>
            {
                Debug.WriteLine($"On Token Validated Called");

                // Debug: Log all claims to see what's available
                var claims = context.Principal?.Claims?.ToList();

                if (claims != null)
                {
                    foreach (var claim in claims)
                    {
                        Debug.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
                    }
                }

                // Utilize user email to find the existing user and create a GUID claim for SignalR connectivity:
                string userEmail = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if(userEmail != null)
                {
                    await AddUserGuidClaim(context, userEmail);
                }


                //return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Debug.WriteLine($"JWT Authentication failed: {context.Exception?.Message}");
                Debug.WriteLine($"JWT Authentication failed details: {context.Exception}");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Debug.WriteLine($"JWT Challenge: {context.Error}, {context.ErrorDescription}");
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
        policy.WithOrigins("http://localhost:4200")
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



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Allow Development Calls");



// get user GUID from request headers and add a claim to use in targeted SignalR messages and notifications:
app.Use(async (context, next) =>
{
    //if (context.Request.Headers.TryGetValue("X-User-Guid", out var userGuid))
    //{
    //    var claims = new List<Claim> {
    //        new Claim("user_guid", userGuid)
    //    };

    //    Debug.WriteLine($"User guid: {userGuid} claims: {claims}");

    //    var identity = new ClaimsIdentity(claims, "Header");
    //    context.User.AddIdentity(identity);
    //}
    Debug.WriteLine($"SignalR request intercepted:");
    Debug.WriteLine($"Path: {context.Request.Path}");
    Debug.WriteLine($"Method: {context.Request.Method}");
    Debug.WriteLine($"Query: {context.Request.QueryString}");
    Debug.WriteLine($"Headers: {string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}={h.Value}"))}");
    if (context.Request.Path.StartsWithSegments("/hubs/notifications"))
    {
        Console.WriteLine($"SignalR request intercepted:");
        Console.WriteLine($"Path: {context.Request.Path}");
        Console.WriteLine($"Method: {context.Request.Method}");
        Console.WriteLine($"Query: {context.Request.QueryString}");
        Console.WriteLine($"Headers: {string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}={h.Value}"))}");

        var token = context.Request.Query["access_token"].FirstOrDefault();
        Console.WriteLine($"Access token in query: {!string.IsNullOrEmpty(token)}");

        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        Console.WriteLine($"Authorization header: {authHeader}");

    };
    await next();

});
//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.MapHub<NotificationHub>("hubs/notifications");

app.Run();
