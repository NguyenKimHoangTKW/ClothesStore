using System.Web;
using System.Web.Optimization;

namespace ClotheSstore
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/Usercss/css").Include(
                "~/Content/assets/wp-content/themes/flatsome/assets/css/flatsome9dd7.css", 
                "~/Content/assets/wp-content/plugins/woo-advanced-product-size-chart/public/css/size-chart-for-woocommerce-publicca305.css",
                "~/Content/assets/wp-content/themes/flatsome/assets/css/flatsome-shop9dd7.css",
                "~/Content/assets/wp-includes/css/classic-themes.min3781.css",
                "~/Content/assets/wp-content/plugins/woo-advanced-product-size-chart/public/css/size-chart-for-woocommerce-publica305.css",
                "~/Content/assets/wp-content/plugins/woo-variation-swatches/assets/css/frontend.min2a42.css",
                "~/Content/assets/wpwp-content/themes/flatsome-child/style6aec.css"));
            bundles.Add(new ScriptBundle("~/Content/Usercss/jquery").Include(
                "~/Content/assets/wp-includes/js/jquery/jquery.min5aed.js",
                "~/Content/assets/wp-includes/js/dist/vendor/wp-polyfill-inert.min0226.js",
                "~/Content/assets/wp-includes/js/dist/vendor/regenerator-runtime.min8fa4.js",
                "~/Content/assets/wp-includes/js/dist/vendor/wp-polyfill.min2c7c.js",
                "~/Content/assets/wp-includes/js/dist/hooks.min6c65.js"));

            bundles.Add(new ScriptBundle("~/Style/ckeditor/jquery").Include(
                        "~/Areas/Style/ckeditor/ckeditor.js"));

        }
    }
}
