using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebAppUniMongoDb.Model
{
    public class Faculty
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string IdBson { get; set; }
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string NameFaculty { get; set; }
    }
}
