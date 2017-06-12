using AITU网站.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace AITU网站.Controllers
{
    

    public class HomeController : Controller
    {

        public ActionResult FindPsw(string email)
        {
            Regex re = new Regex(@"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?");//实例化一个Regex对象
            if (re.IsMatch(email) == true)//验证数据是否匹配
            {
                var psw = "";
                //根据邮箱查询数据库
                using (var context = new MyDb2())
                {
                    var query = from t in context.User
                                where t.Email == email
                                select t.Password;
                    if (query.Count() > 0)
                    {
                        foreach (var p in query)
                        {
                            psw = p;
                        }
                        //定义邮件内容
                        string content = "您的密码为： " + psw;
                        //发送该用户密码至其输入的邮箱
                        var checkInfo = SendEmail(email, content);
                        return Content(checkInfo);
                    }
                    else
                    {
                        return Content("邮箱不存在！");
                    }
                }
            }
            else
            {
                return Content("邮箱格式错误!");//不匹配则弹出”邮箱错误“
            }
        }

        public string SendEmail(string email, string message)
        {
            var sendInfo = "";
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式     
                smtpClient.Host = "smtp.qq.com";//指定SMTP服务器            
                smtpClient.Credentials = new System.Net.NetworkCredential("1364722851@qq.com", "tqcwjttvhgzsfjah");//用户名和授权码          
                                                                                                                   // 发送邮件设置                     
                MailMessage mailMessage = new MailMessage("1364722851@qq.com", email); // 发送人和收件人             
                mailMessage.Subject = "密码找回";
                //主题                    
                mailMessage.Body = message;
                mailMessage.BodyEncoding = Encoding.UTF8;//正文编码        
                mailMessage.IsBodyHtml = true;//设置为HTML格式             
                mailMessage.Priority = MailPriority.High;//优先级           
                smtpClient.Send(mailMessage);
                sendInfo = "密码已发送至您的邮箱，请注意查看！";
            }
            catch
            {
                sendInfo = "邮件发送失败！";
            }
            return sendInfo;
        }

        Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
        public HomeController()
        {
            using (var context = new MyDb2())
            {
                var query = from t in context.Image
                            select t;
                if (query.Count() > 0)
                {
                    foreach (var item in query)
                    {
                        dictionary.Add(item.ImgId, item.ImgContent);
                    }
                }
            }
        }
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

        //上传图片
        public ActionResult Uploads(string path, string imgName, int imgType)
        {
            //根据路径获取图片并将其转化为二进制流
            FileStream fs = new FileStream(path, FileMode.Open);//可以是其他重载方法 
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);
            //BinaryReader br = new BinaryReader(fs);
            //byte[] byData = br.ReadBytes((int)fs.Length);  //将流读入到字节数组中

            //获取图片的像素宽和像素高
            System.Drawing.Image tempimage = System.Drawing.Image.FromStream(fs, true);
            var width = tempimage.Width;
            var height = tempimage.Height;

            //获取当前时间戳
            TimeSpan ts1 = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);

            //将图片存入数据库
            using (var context = new MyDb2())
            {
                var image = new Models.Image();
                image.ImgId = Convert.ToInt64(ts1.TotalMilliseconds).ToString();
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
        //获取我上传的图片
        public ActionResult GetMyUpload()
        {
            var list = new List<Models.Image>();
            using (var context = new MyDb2())
            {
                string userId = (string)Session["userId"];
                var query = from t in context.Image
                            where t.UserId == userId
                            select t;
                if (query.Count() > 0)
                {
                    foreach (var item in query)
                    {
                        Models.Image image = new Models.Image()
                        {
                            ImgId = item.ImgId,
                            ImgName = item.ImgName,
                            ImgType = item.ImgType,
                            ImgWidth = item.ImgWidth,
                            ImgHight = item.ImgHight,
                            CollectNum = item.CollectNum,
                            DownNum = item.DownNum
                        };

                        list.Add(image);
                    }
                    return Json(list);
                }
                else
                {
                    return Content("数据库查询失败！");
                }
            }
        }

        //获取我收藏的图片
        public ActionResult GetCollectImgInfo()
        {

            List<string> collectImgId = new List<string>();
            //Array[] arr = new Array[];
            var collectlist = new List<Models.Collection>();
            var list = new List<Models.Image>();
            using (var context = new MyDb2())
            {
                string userId = (string)Session["userId"];
                var query = from t in context.Collection
                            where t.UserId == userId
                            select t.ImgID;
                var q = from t in context.Image
                            select t;
                if (query.Count() > 0)
                {
                    foreach (var item in query)
                    {
                        collectImgId.Add(item);
                    }
                    foreach (var item in q)
                    {
                        foreach(var i in collectImgId)
                        {
                            if (item.ImgId == i)
                            {
                                Models.Image image = new Models.Image()
                                {
                                    ImgId = item.ImgId,
                                    ImgName = item.ImgName,
                                    ImgType = item.ImgType,
                                    ImgWidth = item.ImgWidth,
                                    ImgHight = item.ImgHight,
                                    CollectNum = item.CollectNum,
                                    DownNum = item.DownNum
                                };
                                list.Add(image);
                            }
                        }
                        
                    }
                    return Json(list);
                }
                else
                {
                    return Content("未收藏图片！");
                }
            }
        }
        //收藏功能
        public ActionResult Collection(string imgId, string userId)
        {
            var res = "";

            //获取当前时间戳
            TimeSpan ts2 = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);

            using (var context = new MyDb2())
            {
                //通过userId查询该用户所有已收藏图片的imgId
                var query = from t in context.Collection
                             where t.UserId == userId
                             select t.ImgID;
                if (query.Count() > 0)
                {
                    //判断是否有本次要收藏的图片的imgId
                    foreach(var v in query)
                    {
                        if(v == imgId)
                        {
                            res = "该图片已收藏！";
                        }
                    }
                    //如果res为空，证明改图片未被该用户收藏过
                    if(res == "")
                    {
                        //操作Collection表，插入新收藏的图片
                        var collection = new Models.Collection();
                        collection.CollectId = Convert.ToInt64(ts2.TotalMilliseconds).ToString();
                        collection.ImgID = imgId;
                        collection.UserId = userId;
                        context.Collection.Add(collection);

                        //操作Image表，将被收藏图片的CollectNum属性值加1
                        var query1 = from t in context.Image
                                     where t.ImgId == imgId
                                     select t;
                        if (query1.Count() > 0)
                        {
                            query1.First().CollectNum += 1;
                        }

                        try
                        {
                            context.SaveChanges();
                            return Content("收藏成功！");
                        }
                        catch (DbEntityValidationException dbEx)
                        {
                            return Content("系统错误！");
                        }
                    }
                    else
                    {
                        return Content(res);
                    }
                }else
                {
                    return Content("系统错误！");
                }

                   
            }
        }

        //删除功能
        public ActionResult deleteImg(string imgId)
        {
            using (var context = new MyDb2())
            {
                //操作image表，删除图片记录
                var image = new Models.Image();
                var query = from t in context.Image
                             where t.ImgId == imgId
                             select t;
                if (query.Count() == 0)
                {
                    return Content("q = 0");
                }
                context.Image.Remove(query.First());

                try
                {
                    context.SaveChanges();
                    return Content("删除成功！");
                }
                catch (DbEntityValidationException dbEx)
                {
                    return Content("删除失败！");
                }
            }
        }

        //个人中心页（显示我的收藏）
        public ActionResult My()
        {
            var userId = (string)Session["userId"];
            ViewBag.userId = userId;
            //查询用户上传图片数量
            using (var context = new MyDb2())
            {
                var query = from t in context.Image
                            where t.UserId == userId
                            select t;
                ViewBag.uploadNum = query.ToList().Count;

            }

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
            using (var context = new MyDb2())
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
        //返回图片
        public FileResult GetImg(string id)
        {
            byte[] selected = null;
            foreach (var v in dictionary)
            {
                if (v.Key == id)
                {
                    selected = v.Value;
                    break;
                }
            }
            return File(selected, "image/jpg");
        }
        //MainIndex视图对应的控制器
        public ActionResult MainIndex()
        {
            ViewBag.userId = Session["userId"];
            ViewBag.text = Session["status"];
            return View();
        }
        //主页面获取所有图片
        public ActionResult GetImgInfo()
        {
            var list = new List<Models.Image>();
            using (var context = new MyDb2())
            {
                var query = from t in context.Image
                            select t;
                if (query.Count() > 0)
                {
                    foreach (var item in query)
                    {
                        Models.Image image = new Models.Image()
                        {
                            ImgId = item.ImgId,
                            ImgName = item.ImgName,
                            ImgType = item.ImgType,
                            ImgWidth = item.ImgWidth,
                            ImgHight = item.ImgHight,
                            CollectNum = item.CollectNum,
                            DownNum = item.DownNum
                        };

                        list.Add(image);
                    }
                    return Json(list);
                }
                else
                {
                    return Content("数据库查询失败！");
                }
            }
        }
        //获取背景元素视图对应的控制器
        public ActionResult beijingImg()
        {
            ViewBag.text = Session["status"];
            return View();
        }
        //主页面获取 背景元素 图片
        public ActionResult GetImgInfo1()
        {
            var list = new List<Models.Image>();
            using (var context = new MyDb2())
            {
                var query = from t in context.Image
                            where t.ImgType == 1
                            select t;


                if (query.Count() > 0)
                {
                    foreach (var item in query)
                    {
                        Models.Image image = new Models.Image()
                        {
                            ImgId = item.ImgId,
                            ImgName = item.ImgName,
                            ImgType = item.ImgType,
                            ImgWidth = item.ImgWidth,
                            ImgHight = item.ImgHight,
                            CollectNum = item.CollectNum,
                            DownNum = item.DownNum
                        };

                        list.Add(image);
                    }
                    return Json(list);
                }
                else
                {
                    return Content("数据库查询失败！");
                }
            }
        }

        //获取图标元素视图对应的控制器
        public ActionResult tubiaoImg()
        {
            ViewBag.text = Session["status"];
            return View();
        }
        //主页面获取 图标元素 图片
        public ActionResult GetImgInfo2()
        {
            var list = new List<Models.Image>();
            using (var context = new MyDb2())
            {
                var query = from t in context.Image
                            where t.ImgType == 2
                            select t;


                if (query.Count() > 0)
                {
                    foreach (var item in query)
                    {
                        Models.Image image = new Models.Image()
                        {
                            ImgId = item.ImgId,
                            ImgName = item.ImgName,
                            ImgType = item.ImgType,
                            ImgWidth = item.ImgWidth,
                            ImgHight = item.ImgHight,
                            CollectNum = item.CollectNum,
                            DownNum = item.DownNum
                        };

                        list.Add(image);
                    }
                    return Json(list);
                }
                else
                {
                    return Content("数据库查询失败！");
                }
            }
        }

       
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}