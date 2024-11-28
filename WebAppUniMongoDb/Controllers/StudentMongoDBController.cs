using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebAppUniMongoDb.Model;

namespace WebAppUniMongoDb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentMongoDBController : ControllerBase
    {
        private IMongoCollection<Student> mongoCollection;
        public StudentMongoDBController(IOptions<StudentDbConfig> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            var mongoDB = mongoClient.GetDatabase(options.Value.DatabaseName);
            mongoCollection = mongoDB.GetCollection<Student>(options.Value.CollectionName);
        }

        [HttpGet]
        public async Task<List<Student>> Get()
        {
            try
            {
                return await mongoCollection.Find(_ => true).ToListAsync();
            }
            catch(Exception ex)
            {
                return new List<Student>();
            }
            
        }

        [HttpGet("{id}")]
        public async Task<List<Student>> Get(int id)
        {
            try
            {
                return await mongoCollection.Find(s => s.Id == id).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<Student>();
            }
             
        }

        [HttpPost]
        public async Task<ActionResult> Post(Student student)
        {
            try
            {
                await mongoCollection.InsertOneAsync(student);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, decimal tuition, decimal avggrades)
        {
            try
            {
                var filter = Builders<Student>.Filter.Eq(s => s.Id, id);
                var update = Builders<Student>.Update
                    .Set(s => s.Tuition, tuition)
                    .Set(s => s.AvgGrades, avggrades);
                await mongoCollection.UpdateOneAsync(filter, update);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await mongoCollection.DeleteOneAsync(s => s.Id == id);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
