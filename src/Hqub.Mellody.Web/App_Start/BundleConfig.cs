using System.Web;
using System.Web.Optimization;

namespace Hqub.Mellody.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/app")
                .Include("~/Scripts/app/radio.js")
                .Include("~/Scripts/app/dto.js")
                .Include("~/Scripts/app/utils.js")
                .Include("~/Scripts/app/player.js")
                .Include("~/Scripts/app/services.js"));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/metro-ui/css/metro-bootstrap.css",
                    "~/Content/metro-ui/css/metro-bootstrap-responsive.css",
                    "~/Content/metro-ui/css/iconFont.min.css")
                .Include("~/Content/site.css"));

            bundles.Add(
                new ScriptBundle("~/bundles/metro-ui")
                    .Include("~/Scripts/metro-ui/jquery.ui.widget.js")
                    .Include("~/Scripts/metro-ui/metro.min.js")
                    .Include("~/Scripts/knockout-3.3.0.js"));

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
