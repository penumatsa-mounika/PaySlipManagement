namespace PaySlipManagement.API.Data
{
    public static class StaticConfigurationHelper
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetDefaultConnectionString()
        {
            if (_configuration == null)
                throw new InvalidOperationException("Configuration not initialized");

            return _configuration.GetConnectionString("DefaultConnection");
        }
    }

}
