# AArkhipenko.Swagger

Nuget-проект с настройками для работы с Swagger

## Настройки

Требуется указание:

1. **SERVICE_NAME** в переменных окружения (если значение совпадает с названием исполняемого проекта, тогда включается описанеи АПИ при помощи XML-документа)

## Методы расширения

Все методы расширения находятся [здесь](./AArkhipenko.Swagger/SwaggerExtension.cs)

### Работа со Swagger

Для настройки работы Swagger доступны методы:

1. По списку версий (строки)
1. По списку версий (OpenApiInfo)
1. По списку версий (строки) + настройки авторизации
1. По списку версий (OpenApiInfo) + настройки авторизации

Для использования Swagger доступны методы:

1. С указанием списка версий (строки)
1. С указанием списка версий (OpenApiInfo)

Подключение:
```C#
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var versions = new[]
{
    $"v10",
    $"v11"
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
...
var app = builder.Build();
// Использование middleware Swagger
app.UseCustomSwagger(versions);
...
app.MapControllers();

app.Run();

```

Для описания АПИ можно использовать:

1. Аттрибуты аннотации (требуется подключение nuget-пакета Swashbuckle.AspNetCore.Annotations)
1. XML-документ (требуется включение *XML-файл документации* в найстройках проекта без указания пути)
