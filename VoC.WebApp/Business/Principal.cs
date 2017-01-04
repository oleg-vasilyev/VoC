using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace VoC.WebApp.Business
{
    public class Principal : IPrincipal
    {
        public Principal(string token, int userId)
        {
            this.Identity = new Identity(token, userId);
        }
        public IIdentity Identity
        {
            get;
            private set;
        }

        public bool IsInRole(string role)
        {
            throw new NotSupportedException();
        }
    }
}