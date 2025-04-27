using Microsoft.OpenApi.Models;

namespace AArkhipenko.Swagger.Models
{
    /// <summary>
    /// Модель авторизации
    /// </summary>
    public class SecurityModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityModel"/> class.
        /// </summary>
        /// <param name="key"><inheritdoc cref="Key" path="/summary"/></param>
        /// <param name="securityScheme"><inheritdoc cref="SecurityScheme" path="/summary"/></param>
        public SecurityModel(
            string key,
            OpenApiSecurityScheme securityScheme)
        {
            Key = key;
            SecurityScheme = securityScheme;
        }

        /// <summary>
        /// Ключ модели авторизации
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Схема авторизации
        /// </summary>
        public OpenApiSecurityScheme SecurityScheme { get; }
    }
}
