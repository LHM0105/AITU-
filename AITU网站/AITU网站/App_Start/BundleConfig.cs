using System.Web;
using System.Web.Optimization;

namespace AITU网站
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //jquery基本功能
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            //jquery的非介入式ajax
            bundles.Add(new ScriptBundle("~/bundles/jquery/unobtrusive-ajax").Include(
                         "~/Scripts/jquery.unobtrusive*"));

            //jquery客户端验证
            bundles.Add(new ScriptBundle("~/bundles/jquery/validate").Include(
                        "~/Scripts/jquery.validate*"));

            //jqueryUI脚本
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                         "~/Scripts/jquery-ui-{version}.js",
                         "~/Scripts/jquery-ui-datepicker-cn.js"
                         ));

            //开发和调试程序用的 Modernizr 的版本
            //实际发布前，可使用 http://modernizr.com 上的生成工具简化要检测的项。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //Bootstrap和respond脚本
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            //模板默认使用的CSS文件
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css"));

            //jqueryUI的CSS文件
            bundles.Add(new StyleBundle("~/Content/themes/base/jquery-ui").Include(
                      "~/Content/themes/base/all.css"));

            //true表示自动进行捆绑【将js全部捆绑到一个文件中】和缩小【自动选择.min.*的文件】。
            //    这种方式由于缩小了文件大小和加载文件的次数，因此加载速度稍快，但调试困难。
            //false表示不进行捆绑和缩小。
            BundleTable.EnableOptimizations = true;
        }
    }
}
