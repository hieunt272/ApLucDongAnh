using ApLucDongAnh.DAL;
using ApLucDongAnh.Models;
using ApLucDongAnh.ViewModel;
using Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace ApLucDongAnh.Controllers
{
    public class HomeController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private static string Email => WebConfigurationManager.AppSettings["email"];
        private static string Password => WebConfigurationManager.AppSettings["password"];
        public ConfigSite ConfigSite => (ConfigSite)HttpContext.Application["ConfigSite"];

        public SelectList CategorySelectList => new SelectList(_unitOfWork.ProductCategoryRepository.Get(orderBy: a => a.OrderBy(o => o.CategoryName)), "Id", "CategoryName");

        private IEnumerable<ArticleCategory> ArticleCategories =>
            _unitOfWork.ArticleCategoryRepository.Get(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort));
        private IEnumerable<ProductCategory> ProductCategories =>
            _unitOfWork.ProductCategoryRepository.Get(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort));
        private IEnumerable<ProjectCategory> ProjectCategories =>
            _unitOfWork.ProjectCategoryRepository.Get(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort));

        #region Home
        public ActionResult Index()
        {
            var model = new HomeViewModel
            {
                Banner = _unitOfWork.BannerRepository.GetQuery(b => b.Active && b.GroupId == 1).FirstOrDefault(),
                Banners = _unitOfWork.BannerRepository.GetQuery(b => b.Active),
                Projects = _unitOfWork.ProjectRepository.GetQuery(a => a.Active && a.Home, o => o.OrderByDescending(a => a.CreateDate), 4),
                ProductCategories = ProductCategories.Where(a => a.ShowHome),
                Services = _unitOfWork.ArticleRepository.GetQuery(a => 
                    a.Active && a.Home && a.ArticleCategory.TypePost == TypePost.Service, o => o.OrderByDescending(a => a.CreateDate), 10),
                Articles = _unitOfWork.ArticleRepository.GetQuery(a => 
                    a.Active && a.Home && a.ArticleCategory.TypePost != TypePost.Service, o => o.OrderByDescending(a => a.CreateDate), 3),
                Feedbacks = _unitOfWork.FeedbackRepository.GetQuery(a => a.Active && a.Home, o => o.OrderBy(a => a.Sort), 10),

                ArticleCategories = ArticleCategories.Where(a => a.ShowMenu && a.TypePost == TypePost.Article),
                MenuProductCategories = ProductCategories.Where(a => a.ShowMenu && a.ParentId == null),
                ProjectCategories = ProjectCategories.Where(a => a.ShowMenu && a.ParentId == null),
            };
            return View(model);
        }
        [ChildActionOnly]
        public PartialViewResult Header()
        {
            var model = new HeaderViewModel
            {
                ArticleCategories = ArticleCategories.Where(a => a.ShowMenu && a.TypePost == TypePost.Article),
                ProductCategories = ProductCategories.Where(a => a.ShowMenu && a.ParentId == null),
                ProjectCategories = ProjectCategories.Where(a => a.ShowMenu && a.ParentId == null),
            };
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult Footer()
        {
            var model = new FooterViewModel
            {
                Policies = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && a.ArticleCategory.TypePost == TypePost.Policy, o => o.OrderByDescending(a => a.CreateDate), 4),
                ArticleCategories = ArticleCategories.Where(a => a.ShowFooter && a.ParentId == null && a.TypePost == TypePost.Article)
            };
            return PartialView(model);
        }
        #endregion

        [Route("lien-he")]
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult ContactForm(Contact model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = false, msg = "Hãy điền đúng định dạng." });
            }
            _unitOfWork.ContactRepository.Insert(model);
            _unitOfWork.Save();
            var subject = "Email liên hệ từ website: " + Request.Url?.Host;
            var body = $"<p>Tên người liên hệ: {model.FullName},</p>" +
                       $"<p>Email: {model.Email},</p>" +
                       $"<p>Số điện thoại: {model.Mobile},</p>" +
                       $"<p>Tên Công Ty: {model.Company},</p>" +
                       $"<p>Nội dung: {model.Body}</p>" +
                       $"<p>Đây là hệ thống gửi email tự động, vui lòng không phản hồi lại email này.</p>";
            Task.Run(() => HtmlHelpers.SendEmail("gmail", subject, body, ConfigSite.Email, Email, Email, Password, "Áp Lực Đông Anh"));

            return Json(new { status = true, msg = "Gửi liên hệ thành công.\nChúng tôi sẽ liên lạc với bạn sớm nhất có thể." });
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult SubcribeForm(HomeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = false, msg = "Hãy điền đúng định dạng." });
            }
            _unitOfWork.SubcribeRepository.Insert(model.Subcribe);
            _unitOfWork.Save();
            var subject = "Email liên hệ từ website: " + Request.Url?.Host;
            var body = $"<p>Tên người liên hệ: {model.Subcribe.FullName},</p>" +
                       $"<p>Số điện thoại: {model.Subcribe.Mobile},</p>" +
                       $"<p>Email: {model.Subcribe.Email},</p>" +
                       $"<p>Đây là hệ thống gửi email tự động, vui lòng không phản hồi lại email này.</p>";
            Task.Run(() => HtmlHelpers.SendEmail("gmail", subject, body, ConfigSite.Email, Email, Email, Password, "Áp Lực Đông Anh"));

            return Json(new { status = true, msg = "Gửi liên hệ thành công.\nChúng tôi sẽ liên lạc với bạn sớm nhất có thể." });
        }

        [Route("gioi-thieu")]
        public ActionResult Introduce()
        {
            var about = _unitOfWork.AboutRepository.Get().FirstOrDefault();

            var model = new IntroduceViewModel
            {
                About = about,
                Banners = _unitOfWork.BannerRepository.GetQuery(b => b.Active),
            };
            return View(model);
        }
        [Route("tuyen-dung")]
        public ActionResult Recruit()
        {
            var recruit = _unitOfWork.RecruitRepository.Get().FirstOrDefault();

            var model = new RecruitViewModel
            {
                Recruit = recruit,
                RecruitPositions = _unitOfWork.RecruitPositionRepository.Get(a => a.Active, o => o.OrderBy(a => a.Sort))
            };
            return View(model);
        }

        #region Article 
        [Route("blogs/{url}.html", Order = 1)]
        public ActionResult ArticleDetail(string url)
        {
            var article = _unitOfWork.ArticleRepository.GetQuery(a => a.Url == url && a.Active).FirstOrDefault();
            if (article == null)
            {
                return RedirectToAction("Index");
            }

            var model = new ArticleDetailsViewModel
            {
                Article = article,
                Previous = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && a.Id < article.Id && 
                    (a.ArticleCategory.Id == article.ArticleCategoryId || a.ArticleCategory.ParentId == article.ArticleCategoryId), o => o.OrderByDescending(a => a.CreateDate)).FirstOrDefault(),
                Next = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && a.Id > article.Id && 
                    (a.ArticleCategory.Id == article.ArticleCategoryId || a.ArticleCategory.ParentId == article.ArticleCategoryId), o => o.OrderBy(a => a.CreateDate)).FirstOrDefault(),
            };
            return View(model);
        }
        [Route("blogs/{url:regex(^(?!.*(vcms|uploader|article|banner|contact|product|projectvcms)).*$)}", Order = 2)]
        public ActionResult ArticleCategory(int? page, string url)
        {
            var category = _unitOfWork.ArticleCategoryRepository.GetQuery(a => a.CategoryActive && a.Url == url).FirstOrDefault();
            if (category == null)
            {
                return RedirectToAction("Index");
            }

            var articles = _unitOfWork.ArticleRepository.GetQuery(
                a => a.Active && (a.ArticleCategoryId == category.Id || a.ArticleCategory.ParentId == category.Id),
                q => q.OrderByDescending(a => a.CreateDate));
            var pageNumber = page ?? 1;

            if (articles.Count() == 1)
            {
                var fi = articles.First();
                return RedirectToAction("ArticleDetail", new { url = fi.Url });
            }
            var model = new ArticleCategoryViewModel
            {
                Category = category,
                Articles = articles.ToPagedList(pageNumber, 8),
                Categories = ArticleCategories.Where(a => a.TypePost == TypePost.Article),
            };

            if (category.ParentId != null)
            {
                model.RootCategory = _unitOfWork.ArticleCategoryRepository.GetById(category.ParentId);
            }
            return View(model);
        }
        [Route("blogs")]
        public ActionResult AllArticle(int? page)
        {
            var pageNumber = page ?? 1;
            var articles = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && a.ArticleCategory.TypePost == TypePost.Article, o => o.OrderByDescending(a => a.CreateDate));

            var model = new AllArticleViewModel()
            {
                Articles = articles.ToPagedList(pageNumber, 8),
                Categories = ArticleCategories.Where(a => a.TypePost == TypePost.Article),
            };
            return View(model);
        }
        [ChildActionOnly]
        public PartialViewResult MenuArticle()
        {
            var model = new MenuArticleViewModel
            {
                Articles = _unitOfWork.ArticleRepository.GetQuery(l => l.Active && l.ArticleCategory.TypePost == TypePost.Article, a => a.OrderByDescending(c => c.CreateDate), 5),
            };
            return PartialView(model);
        }
        [Route("tim-kiem")]
        public ActionResult SearchArticle(int? page, string keywords)
        {
            var pageNumber = page ?? 1;
            var pageSize = 8;
            var newkey = keywords.Trim();
            var articles = _unitOfWork.ArticleRepository.GetQuery(l =>
                l.Active && l.Subject.Contains(newkey) && l.ArticleCategory.TypePost == TypePost.Article, c => c.OrderByDescending(a => a.CreateDate));

            if (string.IsNullOrEmpty(newkey))
            {
                return RedirectToAction("Index");
            }

            var model = new ArticleSearchViewModel
            {
                Articles = articles.ToPagedList(pageNumber, pageSize),
                Keywords = keywords,
                Categories = ArticleCategories.Where(a => a.TypePost == TypePost.Article),
            };

            return View(model);
        }
        #endregion

        #region Product
        [Route("san-pham")]
        public ActionResult AllProduct(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 4;
            var products = _unitOfWork.ProductRepository.GetQuery(p => p.Active, o => o.OrderByDescending(p => p.CreateDate));

            var model = new CategoryProductViewModel
            {
                Products = products.ToPagedList(pageNumber, pageSize),
                Categories = ProductCategories,
                ProductResult = products.Count(),
                BeginCount = (pageNumber - 1) * pageSize + 1,
                EndCount = pageNumber * pageSize,
            };
            return View(model);
        }
        public PartialViewResult GetProduct(int? page, string sort = "date-desc")
        {
            var pageNumber = page ?? 1;
            var pageSize = 4;
            var products = _unitOfWork.ProductRepository.GetQuery(l => l.Active, c => c.OrderByDescending(a => a.CreateDate)).AsNoTracking();

            switch (sort)
            {
                case "date-asc":
                    products = products.OrderBy(a => a.CreateDate);
                    break;
                default:
                    products = products.OrderByDescending(a => a.CreateDate);
                    break;
            }

            var model = new CategoryProductViewModel
            {
                Products = products.ToPagedList(pageNumber, pageSize),
                Sort = sort,
            };

            return PartialView(model);
        }
        [Route("{url:regex(^(?!.*(vcms|uploader|article|banner|contact|productvcms|projectvcms)).*$)}", Order = 2)]
        public ActionResult ProductCategory(int? page, string url)
        {
            var pageNumber = page ?? 1;
            var pageSize = 4;
            var category = ProductCategories.FirstOrDefault(a => a.Url == url);
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            var products = _unitOfWork.ProductRepository.GetQuery(
                p => p.Active && (p.ProductCategoryId == category.Id || p.ProductCategory.ParentId == category.Id),
                c => c.OrderByDescending(p => p.CreateDate));

            var model = new CategoryProductViewModel
            {
                Products = products.ToPagedList(pageNumber, pageSize),
                Category = category,
                Categories = ProductCategories,
                ProductResult = products.Count(),
                BeginCount = (pageNumber - 1) * pageSize + 1,
                EndCount = pageNumber * pageSize,
            };
            return View(model);
        }
        public PartialViewResult GetProductCategory(string url, int? page, string sort = "date-desc")
        {
            var pageNumber = page ?? 1;
            var pageSize = 4;
            var category = ProductCategories.FirstOrDefault(a => a.Url == url);

            var products = _unitOfWork.ProductRepository.GetQuery(
                p => p.Active && (p.ProductCategoryId  == category.Id || p.ProductCategory.ParentId == category.Id),
                c => c.OrderByDescending(p => p.CreateDate));

            switch (sort)
            {
                case "date-asc":
                    products = products.OrderBy(a => a.CreateDate);
                    break;
                default:
                    products = products.OrderByDescending(a => a.CreateDate);
                    break;
            }

            var model = new CategoryProductViewModel
            {
                Products = products.ToPagedList(pageNumber, pageSize),
                Category = category,
                Url = url,
                Sort = sort,
            };
            return PartialView(model);
        }
        [Route("{url}.html", Order = 1)]
        public ActionResult ProductDetail(string url)
        {
            var product = _unitOfWork.ProductRepository.GetQuery(p => p.Url == url).FirstOrDefault();
            var products = _unitOfWork.ProductRepository.GetQuery(
                    p => p.Id != product.Id && p.Active && (p.ProductCategoryId == product.ProductCategoryId || p.ProductCategory.ParentId == product.ProductCategoryId),
                    o => o.OrderByDescending(p => Guid.NewGuid()), 4);

            if (product == null)
            {
                return RedirectToAction("Index");
            }
            var model = new ProductDetailViewModel
            {
                Product = product,
                Products = products,
            };
            return View(model);
        }
        [Route("tim-san-pham")]
        public ActionResult SearchProduct(int? page, string keywords)
        {
            var pageNumber = page ?? 1;
            var pageSize = 4;
            var products = _unitOfWork.ProductRepository.GetQuery(p => p.Active && p.Name.Contains(keywords),
                            o => o.OrderByDescending(p => p.CreateDate));

            if (string.IsNullOrEmpty(keywords))
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new SearchProductViewModel
            {
                Products = products.ToPagedList(pageNumber, pageSize),
                Categories = ProductCategories,
                Keywords = keywords,
                ProductResult = products.Count(),
                BeginCount = (pageNumber - 1) * pageSize + 1,
                EndCount = pageNumber * pageSize,
            };
            return View(model);
        }
        public PartialViewResult GetSearchProduct(string keywords, int? page, string sort = "date-desc")
        {
            var pageNumber = page ?? 1;
            var pageSize = 4;
            var products = _unitOfWork.ProductRepository.GetQuery(p => p.Active && p.Name.Contains(keywords),
                            o => o.OrderByDescending(p => p.CreateDate));
            switch (sort)
            {
                case "date-asc":
                    products = products.OrderBy(a => a.CreateDate);
                    break;
                default:
                    products = products.OrderByDescending(a => a.CreateDate);
                    break;
            }

            var model = new SearchProductViewModel
            {
                Products = products.ToPagedList(pageNumber, pageSize),
                Keywords = keywords,
                Sort = sort,
            };
            return PartialView(model);
        }
        #endregion

        #region Service
        [Route("dich-vu")]
        public ActionResult AllService(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 6;
            var services = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && a.ArticleCategory.TypePost == TypePost.Service, o => o.OrderByDescending(a => a.CreateDate));

            var model = new AllServiceViewModel
            {
                Services = services.ToPagedList(pageNumber, pageSize),
                ProjectCategories = ProjectCategories.Where(a => a.ParentId == null),
                Feedbacks = _unitOfWork.FeedbackRepository.GetQuery(a => a.Active, o => o.OrderBy(a => a.Sort)),
            };

            return View(model);
        }
        [Route("dich-vu/{url}.html", Order = 1)]
        public ActionResult ServiceDetail(string url)
        {
            var service = _unitOfWork.ArticleRepository.GetQuery(a => a.Url == url && a.Active).FirstOrDefault();
            if (service == null)
            {
                return RedirectToAction("Index");
            }

            var model = new ServiceDetailsViewModel
            {
                Service = service,
                Previous = _unitOfWork.ArticleRepository.GetQuery(a =>
                    a.Active && a.Id < service.Id && a.ArticleCategory.TypePost == TypePost.Service, o => o.OrderByDescending(a => a.CreateDate)).FirstOrDefault(),
                Next = _unitOfWork.ArticleRepository.GetQuery(a =>
                    a.Active && a.Id > service.Id && a.ArticleCategory.TypePost == TypePost.Service, o => o.OrderBy(a => a.CreateDate)).FirstOrDefault(),
            };
            return View(model);
        }
        #endregion

        #region Project
        [Route("du-an")]
        public ActionResult AllProject(int? page)
        {
            var pageNumber = page ?? 1;
            var projects = _unitOfWork.ProjectRepository.GetQuery(a => a.Active, o => o.OrderByDescending(a => a.CreateDate));

            var model = new ProjectCategoryViewModel()
            {
                Projects = projects.ToPagedList(pageNumber, 5),
            };
            return View(model);
        }
        [Route("du-an/{url:regex(^(?!.*(vcms|uploader|article|banner|contact|product|projectvcms)).*$)}", Order = 2)]
        public ActionResult ProjectCategory(int? page, string url)
        {
            var pageNumber = page ?? 1;
            var category = _unitOfWork.ProjectCategoryRepository.GetQuery(a => a.CategoryActive && a.Url == url).FirstOrDefault();
            if (category == null)
            {
                return RedirectToAction("Index");
            }

            var projects = _unitOfWork.ProjectRepository.GetQuery(
                a => a.Active && (a.ProjectCategoryId == category.Id || a.ProjectCategory.ParentId == category.Id),
                q => q.OrderByDescending(a => a.CreateDate));

            var model = new ProjectCategoryViewModel
            {
                Projects = projects.ToPagedList(pageNumber, 5),
                Category = category,
                Url = url,
            };

            if (category.ParentId != null)
            {
                model.RootCategory = _unitOfWork.ProjectCategoryRepository.GetById(category.ParentId);
            }
            return View(model);
        }
        [Route("du-an/{url}.html", Order = 1)]
        public ActionResult ProjectDetail(string url)
        {
            var project = _unitOfWork.ProjectRepository.GetQuery(a => a.Url == url && a.Active).FirstOrDefault();
            if (project == null)
            {
                return RedirectToAction("Index");
            }

            var model = new ProjectDetailViewModel
            {
                Project = project,
                Previous = _unitOfWork.ProjectRepository.GetQuery(a => a.Active && a.Id < project.Id &&
                    (a.ProjectCategory.Id == project.ProjectCategoryId || a.ProjectCategory.ParentId == project.ProjectCategoryId), o => o.OrderByDescending(a => a.CreateDate)).FirstOrDefault(),
                Next = _unitOfWork.ProjectRepository.GetQuery(a => a.Active && a.Id > project.Id &&
                    (a.ProjectCategory.Id == project.ProjectCategoryId || a.ProjectCategory.ParentId == project.ProjectCategoryId), o => o.OrderBy(a => a.CreateDate)).FirstOrDefault(),
            };
            return View(model);
        }
        #endregion

        [ChildActionOnly]
        public PartialViewResult OrderForm(string url)
        {
            var product = _unitOfWork.ProductRepository.GetQuery(a => a.Url == url).FirstOrDefault();
            var model = new OrderFormViewModel
            {
                Product = product,
            };
            return PartialView(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult OrderForm(OrderFormViewModel model, FormCollection fc)
        {
            var img = "NO PICTUR";
            var quantity = fc["Order.Quantity"];
            if (!ModelState.IsValid)
            {
                return Json(new { status = false, msg = "Hãy điền đúng định dạng." });
            }
            if (string.IsNullOrEmpty(model.Order.CustomerInfo.FullName))
            {
                model.Order.CustomerInfo.FullName = "Unknown";
            }
            if (model.Order.ProductImg != null)
            {
                img = "<img src='" + Request.Url?.GetLeftPart(UriPartial.Authority) + "/images/products/" + model.Order.ProductImg + "?w=100' />";
            }
            model.Order.CreateDate = DateTime.Now;
            _unitOfWork.OrderRepository.Insert(model.Order);
            _unitOfWork.Save();

            var subject = "Email liên hệ từ website: " + Request.Url?.Host;
            var body = $"<p>Tên người liên hệ: {model.Order.CustomerInfo.FullName},</p>" +
                        $"<p>Email liên hệ: {model.Order.CustomerInfo.Email},</p>" +
                        $"<p>Số điện thoại: {model.Order.CustomerInfo.Mobile},</p>" +
                        $"<p>Địa chỉ: {model.Order.CustomerInfo.Address},</p>" +
                        $"<p>Nội dung: {model.Order.CustomerInfo.Body}</p>" +
                        $"<table border='1' cellpadding='10' style='border:1px #ccc solid;border-collapse: collapse'>" +
                        "<tr>" +
                        "<th> Ảnh sản phẩm</th>" +
                        "<th> Tên sản phẩm </th>" +
                        "</tr>" +
                        "<tr>" +
                        $"<th>{img}</th>" +
                        $"<th>{model.Order.ProductName}</th>" +
                        "</tr>" +
                        $"</table>" +
                        $"<p>Đây là hệ thống gửi email tự động, vui lòng không phản hồi lại email này.</p>";

            Task.Run(() => HtmlHelpers.SendEmail("gmail", subject, body, ConfigSite.Email, Email, Email, Password, "Áp Lực Đông Anh", model.Order.CustomerInfo.Email, ConfigSite.Email));
            return Json(new { status = true, msg = "Đặt mua hàng thành công.\nChúng tôi sẽ liên lạc lại với bạn sớm nhất có thể." });
        }

        public PartialViewResult GetProject(int? catId)
        {
            var projects = _unitOfWork.ProjectRepository.GetQuery(p => p.Active && p.Home, o => o.OrderByDescending(p => p.CreateDate), 4).AsNoTracking();

            if (catId > 0)
            {
                projects = projects.Where(p => p.ProjectCategoryId == catId);
            }

            var model = new GetProjectViewModel
            {
                Projects = projects,
            };
            return PartialView(model);
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}