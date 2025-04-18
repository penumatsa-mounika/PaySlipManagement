using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PaySlipManagement.DAL.Helper
{
    // File: StaticConfigurationHelper.cs
    public static class StaticConfigurationHelper
    {
        public static IConfiguration Configuration { get; private set; }

        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static string GetConnectionString(string name = "DefaultConnection")
        {
            if (Configuration == null)
                throw new InvalidOperationException("Configuration not initialized.");

            return Configuration.GetConnectionString(name);
        }
    }

}
