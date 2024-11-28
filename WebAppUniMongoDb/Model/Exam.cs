using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebAppUniMongoDb.Model
{
    public class Exam
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string IdBson { get; set; }
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int FacultyId { get; set; }
        public int ProfessorId { get; set; }
        public int CourseId { get; set; }
        public DateTime ExamDate { get; set; }
    }
}
