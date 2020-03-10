using Microsoft.Extensions.Configuration;

namespace Budget.Web.App_Start
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Get an instance of T using the convention configuration section name equals the typeof(T).FullName 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static T GetInstance<T>(this IConfiguration config) => config.GetSection(typeof(T).FullName).Get<T>();
    }
}
