using System;
using Application.MongoDb.Core.Entity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KNN.NULLPrinter.Core.Models
{
    public class AppUsers : IEntity<ObjectId>
    { 
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
      

        public bool IsActive { get; set; }
        public bool IsConfirmed { get; set; }

        [BsonDateTimeOptions]
        // attribute to gain control on datetime serialization
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ObjectId CreatedBy_AppUsers_Id { get; set; }
        public ObjectId UpdatedBy_AppUsers_Id { get; set; }
        public ObjectId Id { get; set ; }
    } 
}
