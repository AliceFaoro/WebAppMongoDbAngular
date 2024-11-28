using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebAppUniMongoDb.Model;

namespace WebAppUniMongoDb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseMongoDBController : ControllerBase
    {

        private IMongoCollection<Course> mongoCollection;
        public CourseMongoDBController(IOptions<CourseDbConfig> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            var mongoDB = mongoClient.GetDatabase(options.Value.DatabaseName);
            mongoCollection = mongoDB.GetCollection<Course>(options.Value.CollectionName);
        }

        [HttpGet]
        public async Task<List<Course>> Get()
        {
            try
            {
                return await mongoCollection.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<Course>();
            }
        }

        [HttpGet("{id}")]
        public async Task<List<Course>> Get(int id)
        {
            try
            {
                return await mongoCollection.Find(c => c.Id == id).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<Course>();
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> Post(Course course)
        {
            try
            {
                await mongoCollection.InsertOneAsync(course);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, int professorId)
        {
            try
            {
                var filter = Builders<Course>.Filter.Eq(c => c.Id, id);
                var update = Builders<Course>.Update.Set(c => c.ProfessorId, professorId);
                await mongoCollection.UpdateOneAsync(filter, update);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await mongoCollection.DeleteOneAsync(c => c.Id == id);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
