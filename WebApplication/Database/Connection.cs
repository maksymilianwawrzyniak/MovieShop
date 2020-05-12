using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;
using WebApplication.Models;

namespace WebApplication.Database
{
    public class Connection : IDisposable
    {
        private readonly AppSettingsModel _appSettings;
        private readonly IDriver _driver;

        public Connection(AppSettingsModel appSettings, ILogger logger)
        {
            _appSettings = appSettings;
            var authToken = AuthTokens.Basic(appSettings.UserName, appSettings.Password);
            _driver = GraphDatabase.Driver(appSettings.DatabaseUri, authToken, x => x.WithLogger(logger));
        }

        public async Task<List<string>> CheckDatabaseConnection()
        {
            var session = _driver.AsyncSession(x => x.WithDatabase(_appSettings.DatabaseName));
            try
            {
                var ironManNode = await session.RunAsync(
                    "CREATE (n:Movie {title: 'Iron Man', genre: 'Action', thumbnail: '101010101010101010101010101010'}) RETURN (n)"
                    );
                await ironManNode.ConsumeAsync();
                // var ironMan2Node = await session.RunAsync("CREATE (n:Movie {title: 'Iron Man 2', genre: 'Action'}) RETURN (n)");
                // await ironMan2Node.ConsumeAsync();
                var cursor = await session.RunAsync("MATCH (movie:Movie) RETURN (movie.title)");
                return await cursor.ToListAsync(record => record["(movie.title)"].As<string>());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public void Dispose()
        {
            _driver?.CloseAsync();
        }
    }
}