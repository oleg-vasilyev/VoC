using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace VoC.WebApp.Business
{
    public class Identity : IIdentity
    {
        private string token;
        private int userId;

        public Identity(string token, int userId)
        {
            this.token = token;
            this.userId = userId;
        }

        public string AuthenticationType
        {
            get
            {
                return "";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return token != null && userId > 0;
            }
        }

        public string Name
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public string Token
        {
            get
            {
                return token;
            }
        }
        public int UserId
        {
            get
            {
                return userId;
            }
        }
    }
}