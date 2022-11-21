using Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using ApLucDongAnh.DAL;
using ApLucDongAnh.Models;
using ApLucDongAnh.ViewModel;

namespace ApLucDongAnh.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private IEnumerable<ArticleCategory> ArticleCategories => _unitOfWork.ArticleCategoryRepository.Get();

        #region ArticleCategory
        [ChildActionOnly]
        public ActionResult ListCategory()
        {
            var allcats = _unitOfWork.ArticleCategoryRepository.Get(orderBy: q => q.OrderBy(a => a.CategorySort));
            return PartialView(allcats);
        }
        public ActionResult ArticleCategory(string result = "")
        {
            ViewBag.ArticleCat = result;

            var model = new InsertArticleCatViewModel
            {
                RootCats = new SelectList(_unitOfWork.ArticleCategoryRepository.Get(l => l.ParentId == null, o => o.OrderBy(a => a.CategorySort)), "Id", "CategoryName"),
                ArticleCategory = new ArticleCategory { CategorySort = 1 },
            };

            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ArticleCategory(InsertArticleCatViewModel model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/articleCategory/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "ArticleCategory.Image")
                    {
                        model.ArticleCategory.Image = imgFile;
                    }
                }

                model.ArticleCategory.Url = HtmlHelpers.ConvertToUnSign(null, model.ArticleCategory.Url ?? model.ArticleCategory.CategoryName);
                
                _unitOfWork.ArticleCategoryRepository.Insert(model.ArticleCategory);
                _unitOfWork.Save();
                return RedirectToAction("ArticleCategory", new { result = "success" });
            }
            ViewBag.RootCats = new SelectList(_unitOfWork.ArticleCategoryRepository.Get(l => l.ParentId == null, a => a.OrderBy(c => c.CategorySort)), "Id", "CategoryName");
            return View(model);
        }
        public ActionResult UpdateCategory(int catId = 0)
        {
            var category = _unitOfWork.ArticleCategoryRepository.GetById(catId);
            if (category == null)
            {
                return RedirectToAction("ArticleCategory");
            }

            var model = new InsertArticleCatViewModel
            {
                RootCats = new SelectList(_unitOfWork.ArticleCategoryRepository.Get(l => l.ParentId == null, a => a.OrderBy(c => c.CategorySort)), "Id", "CategoryName"),
                ArticleCategory = category,
            };

            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateCategory(InsertArticleCatViewModel model, FormCollection fc)
        {
            var category = _unitOfWork.ArticleCategoryRepository.GetById(model.ArticleCategory.Id);
            if (ModelState.IsValid)
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/articleCategory/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "ArticleCategory.Image")
                    {
                        category.Image = imgFile;
                    }
                }

                if (fc["CurrentFile"] == "")
                {
                    category.Image = model.ArticleCategory.Image;
                }

                category.Url = HtmlHelpers.ConvertToUnSign(null, model.ArticleCategory.Url ?? model.ArticleCategory.CategoryName);
                category.CategoryName = model.ArticleCategory.CategoryName;
                category.Description = model.ArticleCategory.Description;
                category.CategorySort = model.ArticleCategory.CategorySort;
                category.CategoryActive = model.ArticleCategory.CategoryActive;
                category.ParentId = model.ArticleCategory.ParentId;
                category.ShowMenu = model.ArticleCategory.ShowMenu;
                category.ShowFooter = model.ArticleCategory.ShowFooter;
                category.TitleMeta = model.ArticleCategory.TitleMeta;
                category.DescriptionMeta = model.ArticleCategory.DescriptionMeta;
                category.TypePost = model.ArticleCategory.TypePost;

                _unitOfWork.Save();
                return RedirectToAction("ArticleCategory", new { result = "update" });
            }
            ViewBag.RootCats = new SelectList(_unitOfWork.ArticleCategoryRepository.Get(a => a.ParentId == null, q => q.OrderBy(a => a.CategorySort)), "Id", "CategoryName");
            return View(model);
        }
        [HttpPost]
        public bool DeleteCategory(int catId = 0)
        {

            var category = _unitOfWork.ArticleCategoryRepository.GetById(catId);
            if (category == null)
            {
                return false;
            }
            _unitOfWork.ArticleCategoryRepository.Delete(category);
            _unitOfWork.Save();
            return true;
        }
        public bool UpdateArticleCat(int sort = 1, bool active = false, bool menu = false, int articleCatId = 0)
        {
            var articleCat = _unitOfWork.ArticleCategoryRepository.GetById(articleCatId);
            if (articleCat == null)
            {
                return false;
            }
            articleCat.CategorySort = sort;
            articleCat.CategoryActive = active;
            articleCat.ShowMenu = menu;

            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Article
        public ActionResult ListArticle(int? page, string name, int? catId, int? childId, string result = "")
        {
            ViewBag.Result = result;
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var article = _unitOfWork.ArticleRepository.GetQuery(orderBy: l => l.OrderByDescending(a => a.Id));

            if (childId.HasValue)
            {
                article = article.Where(l => l.ArticleCategoryId == childId);
            }
            else if (catId.HasValue)
            {
                article = article.Where(l => l.ArticleCategoryId == catId || l.ArticleCategory.ParentId == catId);
            }
            if (!string.IsNullOrEmpty(name))
            {
                article = article.Where(l => l.Subject.ToLower().Contains(name.ToLower()));
            }
            var model = new ListArticleViewModel
            {
                SelectCategories = new SelectList(ArticleCategories.Where(a => a.ParentId == null), "Id", "CategoryName"),
                Articles = article.ToPagedList(pageNumber, pageSize),
                CatId = catId,
                ChildId = childId,
                Name = name
            };

            if (catId.HasValue)
            {
                model.ChildCategoryList =
                    new SelectList(ArticleCategories.Where(a => a.ParentId == catId), "Id", "CategoryName");
            }
            return View(model);
        }
        public ActionResult Article()
        {
            var model = new InsertArticleViewModel
            {
                Categories = ArticleCategories,
                Article = new Article { Active = true }
            };
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Article(InsertArticleViewModel model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;
                    
                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/articles/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "Article.CoverImage")
                    {
                        model.Article.CoverImage = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "Article.Image")
                    {
                        model.Article.Image = imgFile;
                    }
                }

                model.Article.Url = HtmlHelpers.ConvertToUnSign(null, model.Article.Url ?? model.Article.Subject);
                var articles = _unitOfWork.ArticleRepository.GetQuery().AsNoTracking();
                if (articles.Any(p => p.Url.Trim() == model.Article.Url.Trim()))
                {
                    model.Article.Url += "-" + DateTime.Now.Millisecond;
                }
                model.Article.ArticleCategoryId = Convert.ToInt32(fc["CategoryId"]);
                _unitOfWork.ArticleRepository.Insert(model.Article);
                _unitOfWork.Save();
                return RedirectToAction("ListArticle", new { result = "success" });
            }

            model.Categories = ArticleCategories;
            return View(model);
        }
        public ActionResult UpdateArticle(int articleId = 0)
        {
            var article = _unitOfWork.ArticleRepository.GetById(articleId);
            if (article == null)
            {
                return RedirectToAction("ListArticle");
            }
            var model = new InsertArticleViewModel
            {
                Article = article,
                Categories = ArticleCategories,
                SelectCategories = new SelectList(ArticleCategories, "Id", "CategoryName")

            };
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateArticle(InsertArticleViewModel model, FormCollection fc)
        {
            var article = _unitOfWork.ArticleRepository.GetById(model.Article.Id);
            if (article == null)
            {
                return RedirectToAction("ListArticle");
            }
            if (ModelState.IsValid)
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/articles/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "Article.CoverImage")
                    {
                        article.CoverImage = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "Article.Image")
                    {
                        article.Image = imgFile;
                    }
                }

                if (fc["CurrentFile"] == "")
                {
                    article.CoverImage = model.Article.CoverImage;
                }
                if (fc["CurrentFile2"] == "")
                {
                    article.Image = model.Article.Image;
                }

                article.Url = HtmlHelpers.ConvertToUnSign(null, model.Article.Url ?? model.Article.Subject);
                article.ArticleCategoryId = Convert.ToInt32(fc["CategoryId"]);
                article.Subject = model.Article.Subject;
                article.Description = model.Article.Description;
                article.Body = model.Article.Body;
                article.Active = model.Article.Active;
                article.Home = model.Article.Home;
                article.TitleMeta = model.Article.TitleMeta;
                article.DescriptionMeta = model.Article.DescriptionMeta;

                var articles = _unitOfWork.ArticleRepository.GetQuery().AsNoTracking();
                if (articles.Any(p => p.Url.ToLower().Trim() == model.Article.Url.ToLower().Trim() && p.Id != model.Article.Id))
                {
                    model.Article.Url += "-" + DateTime.Now.Millisecond;
                }
                _unitOfWork.Save();
                return RedirectToAction("ListArticle", new { result = "update" });
            }
            model.Categories = ArticleCategories;
            return View(model);
        }
        [HttpPost]
        public bool DeleteArticle(int articleId = 0)
        {

            var article = _unitOfWork.ArticleRepository.GetById(articleId);
            if (article == null)
            {
                return false;
            }
            _unitOfWork.ArticleRepository.Delete(article);
            _unitOfWork.Save();
            return true;
        }
        public JsonResult GetChildCategory(int? parentId)
        {
            var categories = ArticleCategories.Where(a => a.ParentId == parentId);
            return Json(categories.Select(a => new { a.Id, Name = a.CategoryName }), JsonRequestBehavior.AllowGet);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}