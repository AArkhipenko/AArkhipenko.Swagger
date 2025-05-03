using AArkhipenko.Swagger.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using CoreConsts = AArkhipenko.Core.Consts;

namespace AArkhipenko.Swagger
{
    /// <summary>
    /// Методы расширения для использования Swagger
    /// </summary>
    public static class SwaggerExtension
    {
        /// <summary>
        /// Добавления настроек для работы с Swagger
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="versions">Список версий АПИ</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IEnumerable<string> versions)
            => services.AddCustomSwagger(versions.Select(x => new OpenApiInfo { Version = x }), null);

        /// <summary>
        /// Добавления настроек для работы с Swagger
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="versions">Список версий АПИ</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IEnumerable<OpenApiInfo> versions)
            => services.AddCustomSwagger(versions, null);

        /// <summary>
        /// Добавления настроек для работы с Swagger
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="versions">Список версий АПИ</param>
        /// <param name="secutityModels">Список моделей авторизации</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IEnumerable<string> versions, IEnumerable<SecurityModel> secutityModels)
            => services.AddCustomSwagger(versions.Select(x => new OpenApiInfo { Version = x }), secutityModels);

        /// <summary>
        /// Добавления настроек для работы с Swagger
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="versions">Список версий АПИ</param>
        /// <param name="secutityModels">Список моделей авторизации</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IEnumerable<OpenApiInfo> versions, IEnumerable<SecurityModel> secutityModels)
        {
            var serviceName = GetServiceName();
            CheckVersions(versions);

            // Работа с минимальными АПИ (создаются через Map)
            services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen(options =>
                {
                    foreach (var version in versions)
                    {
                        var info = new OpenApiInfo(version);
                        if (string.IsNullOrEmpty(version.Title))
                        {
                            info.Title = GetDefaultTitle(version.Version);
                        }

                        options.SwaggerDoc(version.Version, info);
                    }
                    options.EnableAnnotations();
                });

            if (secutityModels is not null && secutityModels.Any())
            {
                services.AddSwaggerGen(options =>
                {
                    var requirements = new OpenApiSecurityRequirement();
                    foreach (var model in secutityModels)
                    {
                        options.AddSecurityDefinition(model.Key, model.SecurityScheme);
                        requirements.Add(model.SecurityScheme, new string[] { });
                    }
                    options.AddSecurityRequirement(requirements);
                });

            }

            try
            {
                services.AddSwaggerGen(options =>
                {
                    var basePath = AppContext.BaseDirectory;

                    var xmlPath = Path.Combine(basePath, $"{serviceName}.xml");
                    options.IncludeXmlComments(xmlPath);
                });
            }
            finally { }

            return services;
        }

        /// <summary>
        /// Включение использования Swagger
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/></param>
        /// <param name="versions">Список версий АПИ</param>
        /// <returns><see cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app, IEnumerable<string> versions)
            => app.UseCustomSwagger(versions.Select(x => new OpenApiInfo { Version = x }));

        /// <summary>
        /// Включение использования Swagger
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/></param>
        /// <param name="versions">Список версий АПИ</param>
        /// <returns><see cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app, IEnumerable<OpenApiInfo> versions)
        {
            CheckVersions(versions);

            app
                .UseSwagger()
                .UseSwaggerUI(setupAction =>
                {
                    setupAction.RoutePrefix = Consts.SwaggerPrefix;
                    foreach (var version in versions)
                    {
                        var title = string.IsNullOrEmpty(version.Title) ? GetDefaultTitle(version.Version) : version.Title;
                        setupAction.SwaggerEndpoint($"../swagger/{version.Version}/swagger.json", title);
                    }
                });

            return app;
        }

        /// <summary>
        /// Получение наименования сервиса
        /// </summary>
        private static string GetServiceName()
        {
            var serviceName = Environment.GetEnvironmentVariable(CoreConsts.ServiceName);
            if (string.IsNullOrEmpty(serviceName))
            {
                throw new ApplicationException($"Не задана переменная коружения {CoreConsts.ServiceName}");
            }

            return serviceName;
        }

        /// <summary>
        /// Проверка списка версий
        /// </summary>
        /// <param name="versions">список версий</param>
        private static void CheckVersions(IEnumerable<OpenApiInfo> versions)
        {
            if (versions == null || versions.Any(x => x is null))
            {
                throw new ApplicationException("Не задан список версий");
            }

            if (versions.Any(x => string.IsNullOrEmpty(x.Version)))
            {
                throw new ApplicationException("Версия должна быть задана");
            }
        }

        /// <summary>
        /// Получение стандартного заголовка для Swagger
        /// </summary>
        /// <param name="version">версия</param>
        /// <returns>Стандартный заголовок для Swagger</returns>
        private static string GetDefaultTitle(string version)
        {
            var serviceName = GetServiceName();
            return $"{serviceName} API {version}";
        }
    }
}
