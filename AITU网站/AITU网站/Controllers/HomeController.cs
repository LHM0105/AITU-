using AITU网站.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Drawing;
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
            //根据路径获取图片并将其转化为二进制流
            FileStream fs = new FileStream(path, FileMode.Open);//可以是其他重载方法 
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);
           /* BinaryReader br = new BinaryReader(fs);
            byte[] byData = br.ReadBytes((int)fs.Length);  //将流读入到字节数组中*/

            //获取图片的像素宽和像素高
            System.Drawing.Image tempimage = System.Drawing.Image.FromStream(fs, true);
            var width = tempimage.Width;
            var height = tempimage.Height;

            //获取当前时间戳
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            
            //将图片存入数据库
            using (var context = new MyDb())
            {
                var image = new Models.Image();
                image.ImgId = Convert.ToInt64(ts.TotalMilliseconds).ToString();
                image.ImgType = imgType;
                image.ImgName = imgName;
                image.ImgContent = byData;
                image.UserId = (string)Session["userId"];
                image.ImgWidth = width;
                image.ImgHight = height;
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
            var id = "1497072994559";
            using (var context = new MyDb())
            {
               // var list = new List<Image>();
                //byte[] imagebytes = null;
              //  Image image = new Image();
                var query = from t in context.Image
                            where t.ImgId == id
                            select t;
                if (query.Count() > 0)
                {
                    foreach (var item in query)
                    {
                      //  imagebytes = (byte[])item.ImgContent.GetValue(18);
                        MemoryStream ms = new MemoryStream(item.ImgContent);
                        Bitmap bmpt = new Bitmap(ms);
                       /* System.IO.MemoryStream ms = new System.IO.MemoryStream(item.ImgContent);
                        System.Drawing.Image img = System.Drawing.Image.FromStream(ms);*/
                      //  ViewBag.img = img;
                       // Session["img"] = img;
                         ViewBag.img = bmpt;
                         Session["img"] = bmpt;
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