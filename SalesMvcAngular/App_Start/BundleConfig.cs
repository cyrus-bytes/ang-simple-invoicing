using System.Web;
using System.Web.Optimization;

namespace SalesMvcAngular
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                         "~/Scripts/ang/moment.js",
                        "~/Scripts/angular.min.js",
                          "~/Scripts/bootstrap.js",
                        "~/Scripts/ang/angucomplete-alt.min.js",
                        "~/Scripts/angular-sanitize.min.js",
                        "~/Scripts/ang/datetimepicker.js",
                           "~/Scripts/ang/datetimepicker.templates.js",
                        "~/Scripts/ang/alertify.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryTableJs").Include(
                       "~/Scripts/DataTables/jquery.dataTables.min.js",
                       "~/Scripts/DataTables/dataTables.buttons.min.js",
                       "~/Scripts/DataTables/buttons.flash.min.js",
                       "~/Scripts/pdf/pdfmake.min.js",
                        "~/Scripts/pdf/vfs_fonts.js",
                         "~/Scripts/pdf/jszip.min.js",
                       "~/Scripts/DataTables/buttons.html5.min.js",
                       "~/Scripts/DataTables/buttons.print.min.js"
                       ));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/angucomplete-alt.css",
                       "~/Content/datetimepicker.css",
                      "~/Content/alertify.min.css",
                      "~/Content/DataTables/css/jquery.dataTables.min.css",
                      "~/Content/DataTables/css/buttons.dataTables.min.css",

                      "~/Content/site.css"));
        }
    }
}
