using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using VoC.DataAccess;
using VoC.WebApp.Business;

namespace VoC.WebApp.Attrebutes
{
    public class UserAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext != null)
            {
                IEnumerable<string> authHeader;
                if (!actionContext.ControllerContext.Request.Headers.TryGetValues("auth-token", out authHeader))
                {
                    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                    return;
                }
                string token = authHeader.First();
                if (token != null)
                {
                    UserManager manager = new UserManager();
                    if (manager.IsAuthorize(token))
                    {
                        actionContext.RequestContext.Principal = new Principal(token, manager.GetCurrentUserId(token));
                    }
                    else
                    {
                        actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                    }
                }
                else
                {
                    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}