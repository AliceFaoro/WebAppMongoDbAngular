using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace WebAppUniMongoDb.BULogic.BasicAuthentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /* Utilizza il costruttore della classe base per configurare l'handler.
          IOptionsMonitor<AuthenticationSchemeOptions>: Per configurare lo schema di autenticazione.
          ILoggerFactory: Per registrare log utili.
          UrlEncoder: Per lavorare con URL in modo sicuro.
          ISystemClock: Utile per ottenere il tempo corrente (spesso per scadenze di token o sessioni). */
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder url, ISystemClock clock) : base(options, logger, url, clock)
        { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //Aggiunge un'intestazione WWW-Authenticate alla risposta, informando il client che è necessario utilizzare il metodo "Basic Authentication".
            Response.Headers.Append("WWW-Authenticate", "Basic");

            //Controlla se la richiesta HTTP contiene l'intestazione Authorization. Se manca, restituisce un errore di autenticazione.
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Autorizzazione mancante"));
            }

            //Usa un'espressione regolare per verificare che il valore dell'header inizi con Basic seguito da una stringa codificata. 
            var authHeader = Request.Headers["Authorization"].ToString();
            var authHeaderRegEx = new Regex("Basic (.*)");

            //Se il formato non è corretto, restituisce un errore.
            if (!authHeaderRegEx.IsMatch(authHeader))
            {
                return Task.FromResult(AuthenticateResult.Fail("Autorizzazione non valida"));
            }

            //Rimuove i primi 6 caratteri (Basic ). Decodifica il resto della stringa, che è in Base64, per ottenere una coppia username: password
            string auth64 = Encoding.UTF8.GetString(
                Convert.FromBase64String(authHeader.Substring(6)));

            //Divide la stringa decodificata in due parti, separate da :.
            var authArr = auth64.Split(':');
            var authUser = authArr[0];
            //Se non c'è una password, lancia un'eccezione.
            var authPwd = authArr.Length > 1 ?
                authArr[1] : throw new Exception("Password ASSENTE !!!");

            //Verifica che né lo username né la password siano vuoti.
            if (string.IsNullOrEmpty(authUser.Trim()) || string.IsNullOrEmpty(authPwd.Trim()))
            {
                return Task.FromResult(AuthenticateResult.Fail("Username e/o Password, NON presenti"));
            }
            else
            {
                // QUI CI SARA' IL CODICE O IL METODO O LA CLASSE O LA CHIAMATA A LIBRERIA
                // PER LA VALIDAZIONE DI USER E PASSWORD
                if (authUser.ToLower() == "alice" && authPwd.ToLower() == "faoro") 
                {
                    /* Se la validazione ha successo:
                    Crea un'istanza di AuthenticatedUser con il metodo di autenticazione e lo username.
                    Costruisce un oggetto ClaimsPrincipal che rappresenta l'utente autenticato.
                    Restituisce un risultato di successo (AuthenticateResult.Success) contenente il ClaimsPrincipal. */
                    var authenticatedUse = new AuthenticatedUser(
                    "BasicAuthentication", true, authUser);
                    var claimMain = new ClaimsPrincipal(new ClaimsIdentity(authenticatedUse));
                    return Task.FromResult(AuthenticateResult.Success(
                        new AuthenticationTicket(claimMain, Scheme.Name)));
                }
                else
                    return Task.FromResult(AuthenticateResult.Fail("Username o Password errati"));
            }

            
            

        }
    }
}
