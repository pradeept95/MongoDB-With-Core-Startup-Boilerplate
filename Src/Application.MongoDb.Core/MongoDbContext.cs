﻿using AspNetCore.MongoDb.Repository.Configuration.MongoDb;
using AspNetCore.MongoDb.Repository.Context;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Application.MongoDb.Core.Context
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database = null;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        //public IMongoCollection<Edition> Editions
        //{
        //    get
        //    {
        //        return _database.GetCollection<Edition>("Editions");
        //    }
        //}

        //public IMongoCollection<EditionFeatureSetting> EditionFeatureSettings
        //{
        //    get
        //    {
        //        return _database.GetCollection<EditionFeatureSetting>("EditionFeatureSettings");
        //    }
        //}
    }
}
