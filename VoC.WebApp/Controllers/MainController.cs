using System.Web.Http;
using VoC.DataAccess;
using VoC.WebApp.Attrebutes;
using VoC.WebApp.Business;

namespace VoC.WebApp.Controllers
{
    [RoutePrefix("api/Main")]
    [UserAuthorize]
    public class MainController : ApiController
    {
        TranslationManager translationManager;
        UserManager userManager;

        public MainController()
        {
            translationManager = new TranslationManager();
            userManager = new UserManager();
        }
        [HttpGet]
        [Route("GetTranslation")]
        public IHttpActionResult GetTranslation(string word)
        {
            if (!string.IsNullOrWhiteSpace(word) && word.Length > 3)
            {
                string result = string.Empty;
                var translations = translationManager.GetLanguagesProbabilities(word);
                userManager.RegistredUserActivity(this.UserId);
                return Ok(translations);
            }
            else
            {
                return BadRequest();
            }
        }

        [Route("GetTop")]
        public IHttpActionResult GetTop()
        {
            var users = userManager.GetTop(5);
            return Ok(users);
        }
        private int UserId
        {
            get
            {
                return (User.Identity as Identity).UserId;
            }
        }
    }
}
