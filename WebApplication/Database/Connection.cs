﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neo4jMapper;
using ServiceStack;
using WebApplication.Models;
using WebApplication.Utils;

namespace WebApplication.Database
{
    /// <summary>
    ///     Class used as API between WebApplication and Neo4J database.
    /// </summary>
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
            var initialMigration = new InitialMigration(this);
            initialMigration.RecreateDatabase();
        }

        /// <summary>
        ///     Run any database query defined by the passed query parameter.
        /// </summary>
        public async Task RunQuery(string query)
        {
            using var session = DatabaseSession.StartSession(_driver, _appSettings.DatabaseName);
            Console.WriteLine(query);
            var cursor = await session.RunAsync(query);
            await cursor.ConsumeAsync();
        }

        /// <summary>
        ///     Create a new T model node.
        /// </summary>
        /// <param name="model">Model to add to the database.</param>
        /// <typeparam name="T">Type of the model that will be added.</typeparam>
        public async Task<T> Create<T>(T model) where T : BaseModel
        {
            var modelName = typeof(T).Name;
            using var session = DatabaseSession.StartSession(_driver, _appSettings.DatabaseName);

            model.Id = Guid.NewGuid().ToString();
            var key = $"new{modelName}";
            var parameters = new Neo4jParameters().WithEntity(key, model);
            var query = $@"
                CREATE (x:{modelName} ${key}) 
                RETURN x";

            Console.WriteLine(query);

            var cursor = await session.RunAsync(query, parameters);
            var result = await cursor.MapSingleAsync<T>();
            return result;
        }

        /// <summary>
        ///     Find all nodes with label corresponding to the T type.
        /// </summary>
        /// <typeparam name="T">Type of the Model that will be searched in the database.</typeparam>
        public async Task<IEnumerable<T>> GetAllOfLabel<T>() where T : BaseModel
        {
            var modelName = typeof(T).Name;
            using var session = DatabaseSession.StartSession(_driver, _appSettings.DatabaseName);

            var cursor = await session.RunAsync($"MATCH (x:{modelName})\nRETURN x");

            var results = (await cursor.ToListAsync()).Map<T>();
            return results;
        }

        /// <summary>
        ///     Find nodes with label corresponding to the T type and parameters passed.
        /// </summary>
        /// <param name="parameter">Tuple (parameter name, parameter value) which will be used in the search.</param>
        /// <typeparam name="T">Type of the Model that will be searched in the database.</typeparam>
        public async Task<IEnumerable<T>> FindAllForLabel<T>((string, string) parameter) where T : BaseModel
        {
            var modelName = typeof(T).Name;
            var (key, value) = parameter;
            using var session = DatabaseSession.StartSession(_driver, _appSettings.DatabaseName);

            var query = new StringBuilder($"MATCH (x:{modelName}");
            query.AppendLine($" {{{key}: '{value}'}})");
            query.Append("RETURN x");

            var queryContent = query.ToString();
            Console.WriteLine(queryContent);
            try
            {
                var cursor = await session.RunAsync(queryContent);
                var result = (await cursor.ToListAsync()).Map<T>();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
                return null;
            }
        }

        /// <summary>
        ///     Find single node with label corresponding to the T type and parameters passed.
        /// </summary>
        /// <param name="parameter">Tuple (parameter name, parameter value) which will be used in the search.</param>
        /// <typeparam name="T">Type of the Model that will be searched in the database.</typeparam>
        public async Task<T> Find<T>((string, string) parameter) where T : BaseModel
        {
            var modelName = typeof(T).Name;
            var (key, value) = parameter;
            using var session = DatabaseSession.StartSession(_driver, _appSettings.DatabaseName);

            var query = new StringBuilder($"MATCH (x:{modelName}");
            query.AppendLine($" {{{key}: '{value}'}})");
            query.Append("RETURN x");

            var queryContent = query.ToString();
            Console.WriteLine(queryContent);
            var cursor = await session.RunAsync(queryContent);
            var result = (await cursor.ToListAsync()).Map<T>().FirstOrDefault();
            return result;
        }

        /// <summary>
        ///     Find all nodes for TV type that are connected to model T with specified relationship. 
        /// </summary>
        /// <param name="startModel">Model from which search is started and to which relationships are directed.</param>
        /// <param name="relationshipLabel">Label of the relationships.</param>
        /// <typeparam name="T">Type of model from which relationships are directed.</typeparam>
        /// <typeparam name="TV">Type of models returned.</typeparam>
        public async Task<IEnumerable<TV>> FindAllByRelationship<T, TV>(T startModel, string relationshipLabel)
            where T : BaseModel where TV : BaseModel
        {
            var fromModelName = typeof(T).Name;
            var searchedModelName = typeof(TV).Name;
            using var session = DatabaseSession.StartSession(_driver, _appSettings.DatabaseName);

            var query = new StringBuilder($"MATCH (x:{fromModelName}");
            query.Append($" {{{Constants.Id}: '{startModel.Id}'}})");
            query.Append($"<-[r:{relationshipLabel}]-");
            query.AppendLine($"(y:{searchedModelName})");
            query.AppendLine("RETURN y");

            var queryContent = query.ToString();
            Console.WriteLine(queryContent);

            var cursor = await session.RunAsync(queryContent);
            var result = (await cursor.ToListAsync()).Map<TV>();
            return result;
        }

        /// <summary>
        ///     Update model of given ID with data from the passed model.
        /// </summary>
        /// <param name="id">ID of the model that data will be replaced.</param>
        /// <param name="model">New data that will be used to replace old data.</param>
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
            var queryContent = query.ToString();
            Console.WriteLine(queryContent);
            var cursor = await session.RunAsync(queryContent);
            var result = (await cursor.SingleAsync()).Map<T>();
            return result;
        }

        /// <summary>
        ///     Remove node from the database.
        /// </summary>
        /// <param name="model"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task Delete<T>(T model) where T : BaseModel
        {
            var modelName = typeof(T).Name;
            using var session = DatabaseSession.StartSession(_driver, _appSettings.DatabaseName);

            var query = new StringBuilder($"MATCH (x:{modelName} {{Id: '{model.Id}'}})\n");
            query.Append("DELETE x");
            var queryContent = query.ToString();
            Console.WriteLine(queryContent);
            var cursor = await session.RunAsync(queryContent);
            await cursor.ConsumeAsync();
        }

        /// <summary>
        ///     Create relationship from model T to model TV with passed label.
        /// </summary>
        public async Task<string> CreateDirectedRelationship<T, TV>(T fromModel, TV toModel, string relationshipLabel)
            where T : BaseModel where TV : BaseModel
        {
            var fromModelName = fromModel.GetType().Name;
            var toModelName = toModel.GetType().Name;

            using var session = DatabaseSession.StartSession(_driver, _appSettings.DatabaseName);

            var query = new StringBuilder($"MATCH (x:{fromModelName}");
            query.AppendLine($" {{{nameof(fromModel.Id)}: '{fromModel.Id}'}}),");

            query.Append($"(y:{toModelName}");
            query.AppendLine($" {{{nameof(toModel.Id)}: '{toModel.Id}'}})");

            query.AppendLine(relationshipLabel.IsNullOrEmpty()
                ? "CREATE (x)-[r]->(y)"
                : $"CREATE (x)-[r:{relationshipLabel}]->(y)");
            query.AppendLine("RETURN type(r)");

            var cursor = await session.RunAsync(query.ToString());
            return (await cursor.SingleAsync()).Map<string>();
        }

        /// <summary>
        ///     Returns relationships created between T and TV models.
        /// </summary>
        /// <param name="fromModel">Model from which relationships starts.</param>
        /// <param name="toModel">Model to which relationship directs to.</param>
        /// <param name="relationshipLabel">Label of relationship to find.</param>
        public async Task<IEnumerable<string>> GetRelationshipsBetween<T, TV>(T fromModel, TV toModel,
            string relationshipLabel = null) where T : BaseModel where TV : BaseModel
        {
            var fromModelName = fromModel.GetType().Name;
            var toModelName = toModel.GetType().Name;

            using var session = DatabaseSession.StartSession(_driver, _appSettings.DatabaseName);

            var query = new StringBuilder($"MATCH (x:{fromModelName}");
            query.Append($" {{{nameof(fromModel.Id)}: '{fromModel.Id}'}})");
            query.Append(relationshipLabel.IsNullOrEmpty()
                ? "-[r]->"
                : $"-[r:{relationshipLabel}]->");
            query.Append($"(y:{toModelName}");
            query.Append($" {{{nameof(toModel.Id)}: '{toModel.Id}'}})");
            query.AppendLine("RETURN type(r)");

            var cursor = await session.RunAsync(query.ToString());
            return (await cursor.ToListAsync()).Map<string>();
        }

        public async Task<int> CountRelationships<T, TV>(T toModel, string relationshipLabel)
            where T : BaseModel where TV : BaseModel
        {
            var fromModelName = toModel.GetType().Name;
            var toModelName = typeof(TV).Name;

            using var session = DatabaseSession.StartSession(_driver, _appSettings.DatabaseName);

            var query = new StringBuilder($"MATCH (x:{fromModelName}");
            query.Append($" {{{nameof(toModel.Id)}: '{toModel.Id}'}})");
            query.Append(relationshipLabel.IsNullOrEmpty()
                ? "<-[r]-"
                : $"<-[r:{relationshipLabel}]-");
            query.AppendLine($"(y:{toModelName})");
            query.AppendLine("RETURN COUNT(r)");

            var cursor = await session.RunAsync(query.ToString());
            return (await cursor.SingleAsync()).Map<int>();
        }

        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}