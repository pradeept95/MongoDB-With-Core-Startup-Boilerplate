using Application.MongoDb.Core.Entity;
using MongoDB.Bson;

namespace Application.Core.Models
{
    public class Settings : IEntity<ObjectId>
    {
        public ObjectId Id { get; set; }
        public bool UseFromDBConfig { get; set; }
        public string DefaultDirectoryPath { get; set; }
        public string DirectoryPath { get; set; }
        public string ArchiveDirectoryPath { get; set; }

        //tag action setting
        public string DropPositionHostUrl { get; set; }
    }  
}
