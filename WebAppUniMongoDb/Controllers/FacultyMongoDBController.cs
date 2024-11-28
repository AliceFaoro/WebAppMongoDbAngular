using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WebAppUniMongoDb.BULogic.BasicAuthentication;
using WebAppUniMongoDb.Model;

namespace WebAppUniMongoDb.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class FacultyMongoDBController : ControllerBase
    {
        private IMongoCollection<Faculty> mongoCollection;
        public FacultyMongoDBController(IOptions<FacultyDbConfig> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            var mongoDB = mongoClient.GetDatabase(options.Value.DatabaseName);
            mongoCollection = mongoDB.GetCollection<Faculty>(options.Value.CollectionName);
        }
        [Authorize]
        [HttpGet]
        public async Task<List<Faculty>> Get()
        {
            try
            {
                return await mongoCollection.Find(_ => true).ToListAsync();
            }
            catch(Exception ex)
            {
                return new List<Faculty>();
            }
        }

        [HttpGet("{id}")]
        public async Task<List<Faculty>> Get(int id)
        {
            try
            {
                return await mongoCollection.Find(f => f.Id == id).ToListAsync();
            }
            catch(Exception ex)
            {
                return new List<Faculty>();
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> Post(Faculty faculty)
        {
            try
            {
                await mongoCollection.InsertOneAsync(faculty);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id,string name)
        {
            try
            {
                var filter = Builders<Faculty>.Filter.Eq(f => f.Id, id);
                var update = Builders<Faculty>.Update.Set(f => f.NameFaculty, name);
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
                await mongoCollection.DeleteOneAsync(f => f.Id == id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

    }
}
