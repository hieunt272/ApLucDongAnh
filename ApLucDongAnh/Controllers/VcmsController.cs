﻿using ApLucDongAnh.DAL;
using ApLucDongAnh.Models;
using ApLucDongAnh.ViewModel;
using Helpers;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ApLucDongAnh.Controllers
{
    [Authorize]
    public class VcmsController : Controller
    {
        public readonly UnitOfWork _unitOfWork = new UnitOfWork();

        #region Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(AdminLoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.Get(a => a.Username == model.Username && a.Active).SingleOrDefault();

                if (admin != null && HtmlHelpers.VerifyHash(model.Password, "SHA256", admin.Password))
                {
                    var ticket = new FormsAuthenticationTicket(1, model.Username.ToLower(), DateTime.Now, DateTime.Now.AddDays(30), true,
                        admin.ToString(), FormsAuthentication.FormsCookiePath);

                    var encTicket = FormsAuthentication.Encrypt(ticket);
                    // Create the cookie.
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Vcms");
                }
                ModelState.AddModelError("", @"Tên đăng nhập hoặc mật khẩu không chính xác.");
            }
            return View(model);
        }
        public RedirectToRouteResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Vcms");
        }
        #endregion

        #region Admin
        public ActionResult Index()
        {
            var model = new InfoAdminViewModel
            {
                Admins = _unitOfWork.AdminRepository.GetQuery().Count(),
                Articles = _unitOfWork.ArticleRepository.GetQuery(a => a.ArticleCategory.TypePost != TypePost.Service).Count(),
                Contacts = _unitOfWork.ContactRepository.GetQuery().Count(),
                Banners = _unitOfWork.BannerRepository.GetQuery().Count(),
                Products = _unitOfWork.ProductRepository.GetQuery().Count(),
                Feedbacks = _unitOfWork.FeedbackRepository.GetQuery().Count(),
                Services = _unitOfWork.ArticleRepository.GetQuery(a => a.ArticleCategory.TypePost == TypePost.Service).Count(),
            };
            return View(model);
        }
        [ChildActionOnly]
        public PartialViewResult ListAdmin()
        {
            var admins = _unitOfWork.AdminRepository.Get();
            return PartialView("ListAdmin", admins);
        }
        public ActionResult CreateAdmin(string result = "")
        {
            ViewBag.Result = result;
            return View();
        }
        [HttpPost]
        public ActionResult CreateAdmin(Admin model)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(model.Username)).SingleOrDefault();
                if (admin != null)
                {
                    ModelState.AddModelError("", @"Tên đăng nhập này có rồi");
                }
                else
                {
                    var hashPass = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                    _unitOfWork.AdminRepository.Insert(new Admin { Username = model.Username, Password = hashPass, Active = model.Active });
                    _unitOfWork.Save();
                    return RedirectToAction("CreateAdmin", new { result = "success" });
                }
            }
            return View();
        }
        public ActionResult EditAdmin(int adminId = 0)
        {
            var admin = _unitOfWork.AdminRepository.GetById(adminId);
            if (admin == null)
            {
                return RedirectToAction("CreateAdmin");
            }

            var model = new EditAdminViewModel
            {
                Id = admin.Id,
                Username = admin.Username,
                Active = admin.Active,
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult EditAdmin(EditAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.GetById(model.Id);
                if (admin == null)
                {
                    return RedirectToAction("CreateAdmin");
                }
                if (admin.Username != model.Username)
                {
                    var exists = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(model.Username)).SingleOrDefault();
                    if (exists != null)
                    {
                        ModelState.AddModelError("", @"Tên đăng nhập này có rồi");
                        return View(model);
                    }
                    admin.Username = model.Username;
                }
                admin.Active = model.Active;
                if (model.Password != null)
                {
                    admin.Password = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                }
                _unitOfWork.Save();
                return RedirectToAction("CreateAdmin", new { result = "update" });
            }
            return View(model);
        }
        public bool DeleteAdmin(string username)
        {
            var admin = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(username)).SingleOrDefault();
            if (admin == null)
            {
                return false;
            }
            if (username == "admin")
            {
                return false;
            }
            _unitOfWork.AdminRepository.Delete(admin);
            _unitOfWork.Save();
            return true;
        }
        public ActionResult ChangePassword(int result = 0)
        {
            ViewBag.Result = result;
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(User.Identity.Name,
                StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                if (admin == null)
                {
                    return HttpNotFound();
                }
                if (HtmlHelpers.VerifyHash(model.OldPassword, "SHA256", admin.Password))
                {
                    admin.Password = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                    _unitOfWork.Save();
                    return RedirectToAction("ChangePassword", new { result = 1 });
                }
                else
                {
                    ModelState.AddModelError("", @"Mật khẩu hiện tại không đúng!");
                    return View();
                }
            }
            return View(model);
        }
        #endregion

        #region ConfigSite
        public ActionResult ConfigSite(string result = "")
        {
            ViewBag.Result = result;
            var config = _unitOfWork.ConfigSiteRepository.Get().FirstOrDefault();
            return View(config);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ConfigSite(ConfigSite model, FormCollection fc)
        {
            var config = _unitOfWork.ConfigSiteRepository.Get().FirstOrDefault();
            if (config == null)
            {
                _unitOfWork.ConfigSiteRepository.Insert(model);
            }
            else
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/configs/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "Image")
                    {
                        config.Image = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "AboutImage")
                    {
                        config.AboutImage = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "Favicon")
                    {
                        config.Favicon = imgFile;
                    }
                }

                config.Facebook = model.Facebook;
                config.GoogleMap = model.GoogleMap;
                config.Youtube = model.Youtube;
                config.Instagram = model.Instagram;
                config.Title = model.Title;
                config.Description = model.Description;
                config.GoogleAnalytics = model.GoogleAnalytics;
                config.Hotline = model.Hotline;
                config.Email = model.Email;
                config.LiveChat = model.LiveChat;
                config.Place = model.Place;
                config.AboutText = model.AboutText;
                config.AboutUrl = model.AboutUrl;
                config.InfoFooter = model.InfoFooter;
                config.InfoContact = model.InfoContact;
                config.ServiceText = model.ServiceText;

                if (model.Zalo != null)
                {
                    config.Zalo = model.Zalo.Replace(" ", string.Empty);
                }

                _unitOfWork.Save();
                HttpContext.Application["ConfigSite"] = config;
                return RedirectToAction("ConfigSite", "Vcms", new { result = "success" });
            }
            return View("ConfigSite", model);
        }
        #endregion

        #region About
        public ActionResult About(string result = "")
        {
            ViewBag.Result = result;
            var about = _unitOfWork.AboutRepository.Get().FirstOrDefault();
            return View(about);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult About(About model, FormCollection fc)
        {
            var about = _unitOfWork.AboutRepository.Get().FirstOrDefault();
            if (about == null)
            {
                _unitOfWork.AboutRepository.Insert(about);
            }
            else
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/abouts/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "CoverImage")
                    {
                        about.CoverImage = imgFile;
                    }
                }

                about.ProductionImage = fc["Pictures"] == "" ? null : fc["Pictures"];
                about.Description = model.Description;
                about.Body = model.Body;
                about.SuMenh = model.SuMenh;
                about.TamNhin = model.TamNhin;
                about.GiaTriCotLoi = model.GiaTriCotLoi;
                about.Production = model.Production;

                _unitOfWork.Save();
                return RedirectToAction("About", "Vcms", new { result = "success" });
            }
            return View("About", model);
        }
        #endregion

        #region Recruit
        public ActionResult Recruit(string result = "")
        {
            ViewBag.Result = result;
            var recruit = _unitOfWork.RecruitRepository.Get().FirstOrDefault();
            return View(recruit);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Recruit(Recruit model, FormCollection fc)
        {
            var recruit = _unitOfWork.RecruitRepository.Get().FirstOrDefault();
            if (recruit == null)
            {
                _unitOfWork.RecruitRepository.Insert(recruit);
            }
            else
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/recruits/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "CoverImage")
                    {
                        recruit.CoverImage = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "Image")
                    {
                        recruit.Image = imgFile;
                    }
                }

                recruit.Description = model.Description;
                recruit.Ability = model.Ability;
                recruit.Position = model.Position;

                _unitOfWork.Save();
                return RedirectToAction("Recruit", "Vcms", new { result = "success" });
            }
            return View("Recruit", model);
        }
        #endregion

        #region RecruitPosition
        [ChildActionOnly]
        public ActionResult ListRecruitPosition()
        {
            var recruitPositions = _unitOfWork.RecruitPositionRepository.Get(orderBy: q => q.OrderBy(a => a.Sort));
            return PartialView(recruitPositions);
        }
        public ActionResult RecruitPosition(string result = "")
        {
            ViewBag.ArticleCat = result;

            var recruitPosition = new RecruitPosition { Sort = 1 };
            return View(recruitPosition);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult RecruitPosition(RecruitPosition recruitPosition)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.RecruitPositionRepository.Insert(recruitPosition);
                _unitOfWork.Save();

                return RedirectToAction("RecruitPosition", new { result = "success" });
            }

            return View(recruitPosition);
        }
        public ActionResult UpdateRecruitPosition(int recruitId = 0)
        {
            var recruitPosition = _unitOfWork.RecruitPositionRepository.GetById(recruitId);
            if (recruitPosition == null)
            {
                return RedirectToAction("RecruitPosition");
            }

            return View(recruitPosition);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateRecruitPosition(RecruitPosition recruitPosition)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.RecruitPositionRepository.Update(recruitPosition);
                _unitOfWork.Save();
                return RedirectToAction("RecruitPosition", new { result = "update" });
            }

            return View(recruitPosition);
        }
        [HttpPost]
        public bool DeleteRecruitPosition(int recruitId = 0)
        {

            var recruitPosition = _unitOfWork.RecruitPositionRepository.GetById(recruitId);
            if (recruitPosition == null)
            {
                return false;
            }
            _unitOfWork.RecruitPositionRepository.Delete(recruitPosition);
            _unitOfWork.Save();
            return true;
        }
        public bool QuickUpdate(int sort = 1, bool active = false, int recruitId = 0)
        {
            var recruitPosition = _unitOfWork.RecruitPositionRepository.GetById(recruitId);
            if (recruitPosition == null)
            {
                return false;
            }
            recruitPosition.Sort = sort;
            recruitPosition.Active = active;

            _unitOfWork.Save();
            return true;
        }
        #endregion

        public ActionResult RedirectConfig(int result = 0)
        {
            var redirect = System.IO.File.ReadAllText(Server.MapPath(Path.Combine("/images/Redirect.txt")));
            ViewBag.Config = redirect;
            ViewBag.Result = result;
            return View();
        }
        [HttpPost]
        public ActionResult RedirectConfig(FormCollection fc)
        {
            var config = fc["Config"];
            try
            {
                System.IO.File.WriteAllText(Server.MapPath(Path.Combine("/images/Redirect.txt")), config);
                return RedirectToAction("RedirectConfig", new { result = 1 });
            }
            catch (Exception e)
            {
                ViewBag.Config = config;
                return View();
            }

        }

        public ActionResult UpdateRobot(int result = 0)
        {
            //if (Role != RoleAdmin.Admin)
            //{
            //    return RedirectToAction("Index", new { error = 403 });
            //}

            var robot = System.IO.File.ReadAllText(Server.MapPath("/robots.txt"));
            ViewBag.Robot = robot;
            ViewBag.Result = result;
            return View();
        }
        [HttpPost]
        public ActionResult UpdateRobot(string robots)
        {
            //if (Role != RoleAdmin.Admin)
            //{
            //    return RedirectToAction("Index", new { error = 403 });
            //}

            System.IO.File.WriteAllText(Server.MapPath("/robots.txt"), robots);
            return RedirectToAction("UpdateRobot", new { result = 1 });
        }


        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}