using Microsoft.Extensions.Configuration;
using System.IO;

namespace Auth_Test
{
    public static class Configuration
    {
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()                
                .AddJsonFile("appsettings.test.json")
                .Build();

            return config;
        }
    }
}
