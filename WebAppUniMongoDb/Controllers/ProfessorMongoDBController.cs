using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebAppUniMongoDb.Model;

namespace WebAppUniMongoDb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfessorMongoDBController : ControllerBase
    {
        private IMongoCollection<Professor> mongoCollection;
        public ProfessorMongoDBController(IOptions<ProfessorDbConfig> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            var mongoDB = mongoClient.GetDatabase(options.Value.DatabaseName);
            mongoCollection = mongoDB.GetCollection<Professor>(options.Value.CollectionName);
        }

        [HttpGet]
        public async Task<List<Professor>> Get()
        {
            try
            {
                return await mongoCollection.Find(_ => true).ToListAsync();
            }
            catch (Exception ex) {
                return new List<Professor>();
            }
            
        }

        [HttpGet("{id}")]
        public async Task<List<Professor>> Get(int id)
        {
            try
            {
                return await mongoCollection.Find(p => p.Id == id).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<Professor>();
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> Post(Professor professor)
        {
            try
            {
                await mongoCollection.InsertOneAsync(professor);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut]
        public async Task<ActionResult> Put(int professorId, int facultyId, decimal pay)
        {
            try
            {
                var filter = Builders<Professor>.Filter.Eq(p => p.Id, professorId);
                var update = Builders<Professor>.Update.Set(p => p.FacultyId, facultyId).Set(p => p.Pay, pay);
                await mongoCollection.UpdateOneAsync(filter, update);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await mongoCollection.DeleteOneAsync(p => p.Id == id);
                return Ok();
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message); 
            }
        }

    }
}
