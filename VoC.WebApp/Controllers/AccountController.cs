using System.Web.Http;
using VoC.WebApp.Models;
using VoC.DataAccess;
using VoC.WebApp.Attrebutes;
using VoC.WebApp.Business;

namespace VoC.WebApp.Controllers
{
    [UserAuthorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private UserManager manager;
        public AccountController()
        {
            manager = new UserManager();
        }

        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            var token = (User.Identity as Identity).Token;
            manager.SignOut(token);
            return Ok();
        }

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

        [AllowAnonymous]
        [Route("Register")]
        public IHttpActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (manager.AddNewUser(model.Email, model.Password))
            {
                return BadRequest();
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }
    }
}
