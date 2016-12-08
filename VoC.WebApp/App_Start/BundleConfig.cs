using System.Web;
using System.Web.Optimization;

namespace VoC.WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/app-scripts").Include(
                            "~/wwwroot/node_modules/rxjs/bundles/Rx.js",
                            "~/wwwroot/node_modules/jquery/dist/jquery.min.js",
                            "~/wwwroot/node_modules/bootstrap/dist/js/bootstrap.min.js",
                            "~/wwwroot/node_modules/bootstrap-material-design/dist/js/material.min.js",
                            "~/wwwroot/node_modules/bootstrap-material-design/dist/js/ripples.min.js",
                            "~/wwwroot/voc.js"));
        }
    }
}
