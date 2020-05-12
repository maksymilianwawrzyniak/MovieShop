using Microsoft.Extensions.Configuration;

namespace WebApplication.Models
{
    public class AppSettingsModel
    {
        public AppSettingsModel(IConfiguration configuration)
        {
            var databaseConfigSection = configuration.GetSection("DatabaseConfig");
            DatabaseUri = databaseConfigSection["DatabaseUri"];
            DatabaseName = databaseConfigSection["DatabaseName"];
            UserName = databaseConfigSection["UserName"];
            Password = databaseConfigSection["Password"];
        }
        
        public string DatabaseUri { get; }
        
        public string DatabaseName { get; }
        
        public string UserName { get; }
        
        public string Password { get; }
    }
}