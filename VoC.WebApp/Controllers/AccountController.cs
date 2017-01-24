using System.Web.Http;
using VoC.DataAccess;
using VoC.WebApp.Attrebutes;
using VoC.WebApp.Business;
using VoC.WebApp.Models;

namespace VoC.WebApp.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private UserManager manager;
        public AccountController()
        {
            manager = new UserManager();
        }

        [UserAuthorize]
        [HttpGet]
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            var token = (User.Identity as Identity).Token;
            manager.SignOut(token);
            return Ok();
        }

        [HttpPost]
        [Route("SignIn")]
        public IHttpActionResult SignIn(SignInModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token = "";
            if (!manager.Login(model.Email, model.Password, out token))
            {
                return BadRequest();
            }
            return Ok(token);
        }

        [HttpPost]
        [Route("Register")]
        public IHttpActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!manager.AddNewUser(model.Email, model.Password))
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
