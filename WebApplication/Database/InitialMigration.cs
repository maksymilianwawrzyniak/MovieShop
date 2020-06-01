﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApplication.Dtos;
using WebApplication.Models;
using WebApplication.Utils;

namespace WebApplication.Database
{
    /// <summary>
    ///     Initial database migration script. Recreates database using JSON files every time the application is started.
    /// </summary>
    public class InitialMigration
    {
        private readonly Connection _connection;

        public InitialMigration(Connection connection)
        {
            _connection = connection;
        }

        public async void RecreateDatabase()
        {
            await DropDatabase();

            await AddModels<Movie>("movies.json");
            await AddModels<Actor>("actors.json");
            await AddModels<Director>("directors.json");
            await AddModels<User>("users.json");

            await AddDirectorMovieRelationship("directors-movies.json");
        }

        private async Task DropDatabase()
        {
            await _connection.RunQuery("MATCH (x) DETACH DELETE x");
        }

        private async Task AddModels<T>(string filePath) where T : BaseModel
        {
            var content = await File.ReadAllTextAsync(filePath);
            var models = JsonConvert.DeserializeObject<IEnumerable<T>>(content);
            foreach (var model in models)
            {
                await _connection.Create(model);
            }
        }

        private async Task AddDirectorMovieRelationship(string filePath)
        {
            var content = await File.ReadAllTextAsync(filePath);
            var dtos = JsonConvert.DeserializeObject<IEnumerable<DirectorMovieRelationshipDto>>(content);
            foreach (var dto in dtos)
            {
                dto.Director = await _connection.Find<Director>((Constants.Surname, dto.Director.Surname));
                dto.Movie = await _connection.Find<Movie>((Constants.Title, dto.Movie.Title));
                if (dto.Movie != null && dto.Director != null)
                    await _connection.CreateDirectedRelationship(dto.Director, dto.Movie, Constants.DirectedLabel);
            }
        }
    }
}