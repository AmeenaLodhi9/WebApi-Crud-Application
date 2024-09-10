using Microsoft.Extensions.Configuration;

namespace webApiCrudApplication.Common_Util
{
    public class SqlQueries
    {
        private static IConfiguration _configuration;

        static SqlQueries()
        {
                _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Ensure the correct base path
                .AddXmlFile("SqlQueries.xml", optional: true, reloadOnChange: true)
                .Build();
        }

        public static string AddInformation
        {
            get
            {
                // Return the value of AddInformation or a default query if the key is missing
                return _configuration["AddInformation"] ?? "Default SQL Query";
            }
        }
        public static string ReadAllInformation
        {
            get
            {
                // Return the value of AddInformation or a default query if the key is missing
                return _configuration["ReadAllInformation"] ?? "Default SQL Query";
            }
        }
        public static string UpdateAllInformationById
        {
            get
            {
                // Return the value of AddInformation or a default query if the key is missing
                return _configuration["UpdateAllInformationById"] ?? "Default SQL Query";
            }
        }
        public static string DeleteInformationById
        {
            get
            {
                // Return the value of AddInformation or a default query if the key is missing
                return _configuration["DeleteInformationById"] ?? "Default SQL Query";
            }
        }

    }
}
