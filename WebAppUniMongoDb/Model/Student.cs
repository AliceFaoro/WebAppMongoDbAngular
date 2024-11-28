using MongoDB.Bson.Serialization.Attributes;

namespace WebAppUniMongoDb.Model
{
    public class Student
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string IdBson { get; set; }
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int FacultyId { get; set; }
        public decimal Tuition { get; set; }
        public decimal AvgGrades { get; set; }
    }
}
