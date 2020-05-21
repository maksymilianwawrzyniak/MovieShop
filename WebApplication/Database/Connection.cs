using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neo4jMapper;
using WebApplication.Models;

namespace WebApplication.Database
{
    public class Connection : IDisposable
    {
        private readonly AppSettingsModel _appSettings;
        private readonly IDriver _driver;

        public Connection(AppSettingsModel appSettings)
        {
            _appSettings = appSettings;
            _driver = GraphDatabase.Driver(
                appSettings.DatabaseUri,
                AuthTokens.Basic(appSettings.UserName, appSettings.Password)
            );
        }

        public async Task<T> Create<T>(T model) where T : BaseModel
        {
            var modelName = typeof(T).Name;
            using var session = DatabaseSession.StartSession(_driver, _appSettings.DatabaseName);

            model.Id = Guid.NewGuid().ToString();
            var key = $"new{modelName}";
            var parameters = new Neo4jParameters().WithEntity(key, model);
            var cursor = await session.RunAsync($@"
                CREATE (x:{modelName} ${key}) 
                RETURN x",
                parameters);

            var result = await cursor.MapSingleAsync<T>();
            return result;
        }

        public async Task<IEnumerable<T>> GetAllOfLabel<T>() where T : BaseModel
        {
            var modelName = typeof(T).Name;
            using var session = DatabaseSession.StartSession(_driver, _appSettings.DatabaseName);

            var cursor = await session.RunAsync($"MATCH (x:{modelName})\nRETURN x");

            var results = (await cursor.ToListAsync()).Map<T>();
            return results;
        }

        public async Task<T> Find<T>(Tuple<string, string> parameter) where T : BaseModel
        {
            var modelName = typeof(T).Name;
            var (key, value) = parameter;
            using var session = DatabaseSession.StartSession(_driver, _appSettings.DatabaseName);

            var query = new StringBuilder($"MATCH (x:{modelName}");
            query.AppendLine($" {{{key}: '{value}'}})");
            query.Append("RETURN x");

            var cursor = await session.RunAsync(query.ToString());
            var result = (await cursor.SingleAsync()).Map<T>();
            return result;
        }

        public async Task<T> Update<T>(string id, T model) where T : BaseModel
        {
            var modelName = typeof(T).Name;
            using var session = DatabaseSession.StartSession(_driver, _appSettings.DatabaseName);

            var query = new StringBuilder($"MATCH (x:{modelName} {{Id: '{id}'}})\n");
            query.AppendLine("SET x = {");
            query.AppendLine($"\tId: '{model.Id}',");
            foreach (var propertyName in model.Properties)
            {
                var propertyValue = model.GetPropertyValueByName(propertyName);
                if (propertyValue != null)
                    query.AppendLine($"\t{propertyName}: '{propertyValue}',");
            }

            query.Remove(query.Length - 3, 1);
            query.AppendLine("}");
            query.Append("RETURN x");

            var cursor = await session.RunAsync(query.ToString());
            var result = (await cursor.SingleAsync()).Map<T>();
            return result;
        }

        public async Task Delete<T>(T model) where T : BaseModel
        {
            var modelName = typeof(T).Name;
            using var session = DatabaseSession.StartSession(_driver, _appSettings.DatabaseName);

            var query = new StringBuilder($"MATCH (x:{modelName} {{Id: '{model.Id}'}})\n");
            query.Append("DELETE x");
            var cursor = await session.RunAsync(query.ToString());
            await cursor.ConsumeAsync();
        }

        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}