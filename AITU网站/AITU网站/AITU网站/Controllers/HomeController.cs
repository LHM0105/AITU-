using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AITU网站.Controllers
{
    public class HomeController : Controller
    {
        //我的上传（分部视图）
        public ActionResult upload()
        {
            return PartialView();
        }
        //个人中心页
        public ActionResult My()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        
        public ActionResult MainIndex()
        {
            ViewBag.text = Session["status"];
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}