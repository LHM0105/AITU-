using AITU网站.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AITU网站.Controllers
{
    public class HomeController : Controller
    {
        //我的收藏
        public ActionResult myCollection()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            else
            {
                return View();
            }
        }
        //我的上传视图
        public ActionResult myUpload()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            else
            {
                return View();
            }
        }

        //上传图片
        public ActionResult Uploads(string pa)
        {
            FileStream fs = new FileStream(pa, FileMode.Open);//可以是其他重载方法 
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);

            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            
            using (var context = new MyDb())
            {
                
                var image = new Image();
                image.ImgId = Convert.ToInt64(ts.TotalMilliseconds).ToString();
                image.ImgName = "abc";
                image.ImgContent = byData;
                image.UserId = (string)Session["userId"];
                image.ImgWidth = 123;
                image.ImgHight = 456;
                context.Image.Add(image);
                if (context.SaveChanges() > 0)
                {
                    return Content("success");
                }
                else
                {
                    return Content("failed");
                }
            }
        }

        //我的下载
        public ActionResult myDownload()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            else
            {
                return View();
            }
        }
        //个人中心页（显示我的收藏）
        public ActionResult My()
        {
            ViewBag.userId = Session["userId"];
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            else
            {
                return View();
            }
        }
        //修改密码

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