using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using VoC.DataAccess;
using VoC.WebApp.Hubs;

namespace VoC.WebApp.Controllers
{
	[RoutePrefix("api/Main")]
	[Authorize]
	public class MainController : ApiController
	{
		private Guid _userId = Guid.Empty;
		private ApplicationUserManager _userManager;

		[HttpGet]
		[Route("GetTranslation")]
		public IHttpActionResult GetTranslation(string word)
		{
			if (!string.IsNullOrWhiteSpace(word) && word.Length > 3)
			{
				string result = string.Empty;
				using (Provider provider = new Provider())
				{
					var selectedWord = provider.CheckWord(word, UserId);

					if (selectedWord == null)
					{
						selectedWord = provider.AddWords(word);
					}
					result = String.Join(", ", selectedWord.Probabilities.Select(m => m.Language + " : " + m.Probability.ToString()));
				}

				var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<UserTop>();
				context.Clients.All.UpdateList();

				return Ok(result);
			}
			else
			{
				return BadRequest();
			}
		}

		public IHttpActionResult GetTop()
		{
			string result = "";

			using (Provider provider = new Provider())
			{
				var users = provider.GetTopFive();
				result = Newtonsoft.Json.JsonConvert.SerializeObject(users);

			}
			return Ok(result);
		}
		private Guid UserId
		{
			get
			{
				if (_userId != Guid.Empty)
				{
					return _userId;
				}

				if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
				{
					Guid userId;
					if (Guid.TryParse(User.Identity.GetUserId(), out userId))
					{
						_userId = userId;
						return this._userId;
					}
					else
					{
						throw new ArgumentException("Incorrect UserId");
					}
				}
				else
				{
					return Guid.Empty;
				}
			}
		}

		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}
	}
}
