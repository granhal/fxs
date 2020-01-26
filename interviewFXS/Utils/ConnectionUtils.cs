using System;
using Microsoft.Extensions.Configuration;

namespace interviewFXS.Utils
{
    public class ConnectionUtils
    {
        public ConnectionUtils()
        {
        }
        public static string ConnectionString(IConfiguration configuration)
        {
            var enviroment = Environment.GetEnvironmentVariable("ENV");
            if (enviroment == "dev" || enviroment == "pre" || enviroment == "prod" || enviroment == "qa")
            {
                string res = string.Format("{0}{1}:{2}@{3}",
                Environment.GetEnvironmentVariable("MONGODB_URI_PREFIX"),
                Environment.GetEnvironmentVariable("MONGODB_USER"),
                Environment.GetEnvironmentVariable("MONGODB_PASS"),
                Environment.GetEnvironmentVariable("MONGODB_URI"));
                return res;
            }
            Console.WriteLine(DateTime.UtcNow + " UTC " + "[SYSTEM] Connection string mongodb is: " + configuration["ConnectionString:MongoUrl"] + " success");
            return configuration["ConnectionString:MongoUrl"];
        }
        public static string MongoDatabase(IConfiguration configuration)
        {
            var enviroment = Environment.GetEnvironmentVariable("ENV");
            if (enviroment == "dev" || enviroment == "pre" || enviroment == "prod" || enviroment == "qa")
            {
                return Environment.GetEnvironmentVariable("MONGODB_DB");
            }
            Console.WriteLine(DateTime.UtcNow + " UTC " + "[SYSTEM] mongodb using name is: " + configuration["ConnectionString:MongoDB"]);
            return configuration["ConnectionString:MongoDB"];
        }
        public static string MongoCollection(IConfiguration configuration, string collectionName)
        {
            var enviroment = Environment.GetEnvironmentVariable("ENV");
            if (enviroment == "dev" || enviroment == "pre" || enviroment == "prod" || enviroment == "qa")
            {
                return Environment.GetEnvironmentVariable(collectionName);
            }
            Console.WriteLine(DateTime.UtcNow + " UTC " + "[SYSTEM] mongodb collection is: " + configuration["ConnectionString:MongoCollection:" + collectionName]);
            return configuration["ConnectionString:MongoCollection:" + collectionName];
        }
    }
}
