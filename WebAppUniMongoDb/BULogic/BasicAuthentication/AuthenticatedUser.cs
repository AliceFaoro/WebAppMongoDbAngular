using System.Security.Principal;

namespace WebAppUniMongoDb.BULogic.BasicAuthentication
{
    public class AuthenticatedUser : IIdentity
    {
        public AuthenticatedUser(string authType, bool isAuth, string name)
        {
            AuthenticationType = authType;
            IsAuthenticated = isAuth;
            Name = name;
        }
        public string? AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }

        public string? Name { get; set; }
    }
}
