using AArkhipenko.Core;
using AArkhipenko.Swagger;
using AArkhipenko.Swagger.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

using TestConsts = AArkhipenko.Swagger.Test.Consts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var versions = new[]
{
    $"v{TestConsts.ApiVersion10}",
    $"v{TestConsts.ApiVersion11}"
};

var security = new[]
{
    new SecurityModel("BearerAuth", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header \"Authorization: Bearer {token}\"",
            Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "BearerAuth"
                    }
        })
};

// Добавление работы с Swagger
builder.Services.AddCustomSwagger(versions, security);

// Добавление версионирования
builder.Services.AddVersioning();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Использование middleware Swagger
app.UseCustomSwagger(versions);

app.MapControllers();

app.Run();
