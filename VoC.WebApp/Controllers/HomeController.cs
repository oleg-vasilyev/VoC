using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace VoC.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            return View();
        }
    }
}
