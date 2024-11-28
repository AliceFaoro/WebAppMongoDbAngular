using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebAppUniMongoDb.Model
{
    public class Course
    {

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string IdBson { get; set; }
        [Key]
        [Required]
        public int Id { get; set; }
        public string CourseName { get; set; }
        public int FacultyId { get; set; }
        public int ProfessorId { get; set; }
    }
}
