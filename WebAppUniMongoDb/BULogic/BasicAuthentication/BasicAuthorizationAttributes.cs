using Microsoft.AspNetCore.Authorization;

namespace WebAppUniMongoDb.BULogic.BasicAuthentication
{
    public class BasicAuthorizationAttributes : AuthorizeAttribute
    {
        public BasicAuthorizationAttributes() { Policy = "BasicAuthentication"; }
    }
}

