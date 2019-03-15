using Application.Core.Configuration.MongoDb;
using Application.Core.Configuration.TokenAuth;
using Application.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Application
{
    public static class OptionConfigurer
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Settings>(options =>
            {  
                options.UseFromDBConfig = Convert.ToBoolean(configuration.GetSection("PDF:UseFromDBConfig").Value);
                options.DefaultDirectoryPath = configuration.GetSection("PDF:DefaultDirectoryPath").Value;
                options.DirectoryPath = configuration.GetSection("PDF:DirectoryPath").Value;
                options.ArchiveDirectoryPath = configuration.GetSection("PDF:ArchiveDirectoryPath").Value;

                options.DropPositionHostUrl = configuration.GetSection("Drop:DropPositionHostUrl").Value;
            });

            services.Configure<TokenAuthConfiguration>(options =>
            {
                options.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"]));
                options.Issuer = configuration["Authentication:JwtBearer:Issuer"];
                options.Audience = configuration["Authentication:JwtBearer:Audience"];
                options.SigningCredentials = new SigningCredentials(options.SecurityKey, SecurityAlgorithms.HmacSha256);
                options.Expiration = TimeSpan.FromDays(1);
            });

            services.Configure<MongoDbSettings>(options =>
            {
                options.ConnectionString
                    = configuration["MongoSettings:ConnectionString"];
                options.Database
                    = configuration["MongoSettings:Database"]; 
            });

        }
    }
}
