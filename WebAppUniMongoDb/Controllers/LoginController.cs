using Microsoft.AspNetCore.Mvc;

namespace WebAppUniMongoDb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        [HttpPost]
        public ActionResult LoginWorker(Credentials credentials)
        {
            if (credentials.Username.ToLower() == "alice" && credentials.Password.ToLower() == "faoro")
                return Ok();
            else
                return Unauthorized();
        }
    }

    public class Credentials
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
