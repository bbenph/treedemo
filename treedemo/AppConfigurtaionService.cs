using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace treedemo
{
    public class AppConfigurtaionService
    {
        public static IConfiguration Configuration { get; set; }
        static AppConfigurtaionService()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"{AppContext.BaseDirectory}appsettings.json", false, true);

            Configuration = builder.Build();
        }
    }
}
