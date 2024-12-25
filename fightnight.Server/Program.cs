using fightnight.Server.repo;
using fightnight.Server.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using fightnight.Server.Interfaces;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.ResponseCompression;
using fightnight.Server.Hubs;
using fightnight.Server.Data;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Interfaces.IServices;
using fightnight.Server.Repos;
using fightnight.Server.Models.Tables;
using fightnight.Server.Providers.OAuthProviders;
using MetInProximityBack.Factories;
using MetInProximityBack.Interfaces;
using MetInProximityBack.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";
    options.User.RequireUniqueEmail = true;

    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 3;
})
.AddEntityFrameworkStores<AppDBContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.Cookie.Name = "FNAuth";

    options.LoginPath = "/account/login";
    options.LogoutPath = "/account/logout";
    //options.AccessDeniedPath = "/account/AccessDenied";
    options.SlidingExpiration = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme
);

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromMinutes(5);
});
/*
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))
    };
})
*/
/*
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.Configure<AWSOptions>(builder.Configuration.GetSection("AWSConfig"));
*/
var emailSettings = builder.Configuration;

builder.Services
    .AddFluentEmail(emailSettings["EmailSettings:DefaultSenderEmail"], emailSettings["EmailSettings:DefaultSenderName"])
    .AddSmtpSender(new SmtpClient(emailSettings["EmailSettings:SmtpServer"])
    {
        Port = 587, 
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(emailSettings["EmailSettings:SmtpUsername"], emailSettings["EmailSettings:SmtpPassword"]) // Use app-specific password
    });

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "applciation/octet-stream" });
});

builder.Services.AddScoped<IEventRepo, EventRepo>();
builder.Services.AddScoped<IMemberRepo, MemberRepo>();
builder.Services.AddScoped<IMessageRepo, MessageRepo>();
builder.Services.AddScoped<IInviteRepo, InviteRepo>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<IOAuthService, OAuthService>();

builder.Services.AddTransient<IOAuthProvider, GoogleOAuthProvider>();
builder.Services.AddTransient<IOAuthProvider, MicrosoftOAuthProvider>();
builder.Services.AddSingleton<OAuthProviderFactory>();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

//add useRouting

//useCors MUST BE before useResponseCaching
app.UseCors(x => x
     .AllowAnyMethod()
     .AllowAnyHeader()
     .AllowCredentials()
     //.WithOrigins("https://localhost:5173"));
     .SetIsOriginAllowed(origin => true));


app.UseAuthentication();
app.UseAuthorization();

//app.UseSession();

//responseCaching
app.MapHub<ChatHub>("/chathub");
app.MapControllers();


app.MapFallbackToFile("/index.html");

app.Run();
