using AspNetCore.MongoDb.Repository.Configuration.MongoDb;
using AspNetCore.MongoDb.Repository.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.MongoDb.Repository
{
    public static class RepositoryRegistrar
    {
        public static void AddMongoDbRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(options =>
            {
                options.ConnectionString
                    = configuration["MongoSettings:ConnectionString"];
                options.Database
                    = configuration["MongoSettings:Database"];
            });

            services.AddSingleton<IMongoDbContext, MongoDbContext>();
            services.AddSingleton(typeof(IMongoRepository<,>), typeof(MongoRepository<,>));
        }

        public static void AddMongoDbRepository(this IServiceCollection services, string connectionString, string database)
        {
            services.Configure<MongoDbSettings>(options =>
            {
                options.ConnectionString
                    = connectionString;
                options.Database
                    = database;
            });

            services.AddSingleton<IMongoDbContext, MongoDbContext>();
            services.AddSingleton(typeof(IMongoRepository<,>), typeof(MongoRepository<,>));
        }
    }
}
