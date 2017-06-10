using AITU网站.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
        public ActionResult Uploads(string path, string imgName, int imgType)
        {
            FileStream fs = new FileStream(path, FileMode.Open);//可以是其他重载方法 
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);

            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            
            using (var context = new MyDb())
            {
                var image = new Image();
                image.ImgId = Convert.ToInt64(ts.TotalMilliseconds).ToString();
                image.ImgType = imgType;
                image.ImgName = imgName;
                image.ImgContent = byData;
                image.UserId = (string)Session["userId"];
                image.ImgWidth = 123;
                image.ImgHight = 456;
                context.Image.Add(image);

                try
                {
                    context.SaveChanges();
                    return Content("上传成功！");
                }
                catch (DbEntityValidationException dbEx)
                {
                    return Content("系统错误，上传失败！");
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
            var useId = Session["userId"];
            ViewBag.userId = useId;
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
        public ActionResult XGMM(string nowPsw, string newPsw)
        {
            using (var context = new MyDb())
            {
                string ud = (string)Session["userId"];
                var query = from t in context.User
                            where t.UserId == ud
                            select t;
                if (query.Count() > 0)
                {
                    if (query.First().Password == nowPsw)
                    {
                        query.First().Password = newPsw;

                    }
                    else
                    {
                        return Content("原始密码输入错误！");
                    }
                }

                try
                {
                    context.SaveChanges();
                    return Content("修改密码成功！");
                }
                catch
                {
                    return Content("抱歉，系统出错！");
                }
            }
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
            var id = "1497017030753";
            using (var context = new MyDb())
            {
                var list = new List<Image>();
                Image image = new Image();
                var query = from t in context.Image
                            where t.ImgId == id
                            select t;
                if (query.Count() > 0)
                {
                    foreach (var item in query)
                    {
                        System.IO.MemoryStream ms = new System.IO.MemoryStream(item.ImgContent);
                        System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                        ViewBag.img = img;
                        Session["img"] = img;
                    }
                }
                        /*  var query = from t in context.Image
                                      select t;
                          if (query.Count() > 0)
                          {
                              foreach (var item in query)
                              {
                                  image.ImgId = item.ImgId;
                                  image.ImgName = item.ImgName;
                                  image.ImgType = item.ImgType;
                                  image.ImgWidth = item.ImgWidth;
                                  image.ImgHight = item.ImgHight;
                                  image.ImgContent = item.ImgContent;
                                  image.CollectNum = item.CollectNum;
                                  image.DownNum = item.DownNum;
                                  list.Add(image);
                              }
                              //   return Json(list);
                              ViewBag.list = list;
                              Session["list"] = list;
                    }*/
                else
                {
                    return null;
                }
            }
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