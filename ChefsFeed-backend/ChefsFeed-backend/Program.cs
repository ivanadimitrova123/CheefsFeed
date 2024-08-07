using Microsoft.EntityFrameworkCore;
using ChefsFeed_backend.Data;
using ChefsFeed_backend.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.OpenApi.Models;
using ChefsFeed_backend.Services.Implementation;
using ChefsFeed_backend.Services.Interfaces;
using ChefsFeed_backend.Repositories.Implementation;
using ChefsFeed_backend.Repositories.Interfaces;
//using ChefsFeed_backend.Business.Interfaces;
//using ChefsFeed_backend.Business.Services;
//using ChefsFeed_backend.Data.Interfaces;
//using ChefsFeed_backend.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//da se pojavi auth kopceto za da vneseme taken
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
//database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPictureService, PictureService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPictureRepository, PictureRepository>();


//password
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Add authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "https://localhost:44365/",
        ValidAudience = "https://localhost:44365/",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("VkVSfGYr8VSkxDRF8ftKCwZuqN1lLLxBZN7s20jS"))
    };
});
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPol
//    icy("AdminPolicy", policy => policy.RequireRole("Admin"));
//});


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
