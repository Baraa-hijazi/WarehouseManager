using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using WarehouseManager.Core.Entities;
using WarehouseManager.Extensions;
using WarehouseManager.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server=b-hejazi;Database=WarehouseManager;Trusted_Connection=True;"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.ClaimsIssuer = configuration["Jwt:Issuer"];
    options.Audience = configuration["Jwt:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"])),
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.Scan(scan => scan
    .FromCallingAssembly()
    .AddClasses()
    .AsMatchingInterface()
    .WithScopedLifetime());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.DefaultModelRendering(ModelRendering.Model);
        c.EnableDeepLinking();
    });
}

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.MapControllers();

app.Run();