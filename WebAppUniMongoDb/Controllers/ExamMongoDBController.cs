using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebAppUniMongoDb.Model;

namespace WebAppUniMongoDb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExamMongoDBController : ControllerBase
    {
        private IMongoCollection<Exam> mongoCollection;
        public ExamMongoDBController(IOptions<ExamDbConfig> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            var mongoDB = mongoClient.GetDatabase(options.Value.DatabaseName);
            mongoCollection = mongoDB.GetCollection<Exam>(options.Value.CollectionName);
        }

        [HttpGet]
        public async Task<List<Exam>> Get()
        {
            try
            {
                return await mongoCollection.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<Exam>();
            }
        }

        [HttpGet("{id}")]
        public async Task<List<Exam>> Get(int id)
        {
            try
            {
                return await mongoCollection.Find(e => e.Id == id).ToListAsync();
            }
            catch(Exception ex)
            {
                return new List<Exam>();    
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(Exam exam)
        {
            try
            {
                mongoCollection.InsertOneAsync(exam);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, DateTime date)
        {
            try
            {
                var filter = Builders<Exam>.Filter.Eq(e => e.Id, id);
                var update = Builders<Exam>.Update.Set(e => e.ExamDate, date);
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
                await mongoCollection.DeleteOneAsync(e => e.Id == id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
