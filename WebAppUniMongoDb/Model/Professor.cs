using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebAppUniMongoDb.Model
{
    public class Professor
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string BsonId { get; set; }
        [Key]
        [Required]
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int FacultyId { get; set; }
        public decimal Pay { get; set; }
    }
}
