using Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApLucDongAnh.DAL;
using ApLucDongAnh.Models;
using ApLucDongAnh.ViewModel;

namespace ApLucDongAnh.Controllers
{
    [Authorize]
    public class ProjectVcmsController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private IEnumerable<ProjectCategory> ProjectCategories => _unitOfWork.ProjectCategoryRepository.Get(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort));

        #region ProjectCategory
        [ChildActionOnly]
        public ActionResult ListCategory()
        {
            var allcats = _unitOfWork.ProjectCategoryRepository.Get(orderBy: q => q.OrderBy(a => a.CategorySort));
            return PartialView(allcats);
        }
        public ActionResult ProjectCategory(string result = "")
        {
            ViewBag.NewsCat = result;

            var model = new InsertProjectCatViewModel
            {
                RootCats = new SelectList(ProjectCategories.Where(a => a.ParentId == null), "Id", "CategoryName"),
                ProjectCategory = new ProjectCategory { CategorySort = 1 }
            };

            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ProjectCategory(InsertProjectCatViewModel model, FormCollection fc)
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
                    var imgPath = "/images/projectCategory/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "ProjectCategory.CoverImage")
                    {
                        model.ProjectCategory.CoverImage = imgFile;
                    }
                }

                model.ProjectCategory.Url = HtmlHelpers.ConvertToUnSign(null, model.ProjectCategory.Url ?? model.ProjectCategory.CategoryName);
                _unitOfWork.ProjectCategoryRepository.Insert(model.ProjectCategory);
                _unitOfWork.Save();
                return RedirectToAction("ProjectCategory", new { result = "success" });
            }
            ViewBag.RootCats = new SelectList(ProjectCategories.Where(a => a.ParentId == null), "Id", "CategoryName");
            return View(model);
        }
        public ActionResult UpdateCategory(int catId = 0)
        {
            var category = _unitOfWork.ProjectCategoryRepository.GetById(catId);
            if (category == null)
            {
                return RedirectToAction("ProjectCategory");
            }

            var model = new InsertProjectCatViewModel()
            {
                RootCats = new SelectList(ProjectCategories.Where(a => a.ParentId == null), "Id", "CategoryName"),
                ProjectCategory = category,
            };

            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateCategory(InsertProjectCatViewModel model, FormCollection fc)
        {
            var category = _unitOfWork.ProjectCategoryRepository.GetById(model.ProjectCategory.Id);
            if (ModelState.IsValid)
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/projectCategory/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "ProjectCategory.CoverImage")
                    {
                        category.CoverImage = imgFile;
                    }
                }

                var file = Request.Files["ProjectCategory.CoverImage"];

                if (file != null && file.ContentLength == 0)
                {
                    category.CoverImage = fc["CurrentFile"] == "" ? null : fc["CurrentFile"];
                }

                model.ProjectCategory.Url = HtmlHelpers.ConvertToUnSign(null, model.ProjectCategory.Url ?? model.ProjectCategory.CategoryName);
                category.CategoryName = model.ProjectCategory.CategoryName;
                category.Description = model.ProjectCategory.Description;
                category.CategorySort = model.ProjectCategory.CategorySort;
                category.CategoryActive = model.ProjectCategory.CategoryActive;
                category.ParentId = model.ProjectCategory.ParentId;
                category.ShowMenu = model.ProjectCategory.ShowMenu;
                category.TitleMeta = model.ProjectCategory.TitleMeta;
                category.DescriptionMeta = model.ProjectCategory.DescriptionMeta;

                _unitOfWork.Save();
                return RedirectToAction("ProjectCategory", new { result = "update" });
            }
            ViewBag.RootCats = new SelectList(ProjectCategories.Where(a => a.ParentId == null), "Id", "CategoryName");
            return View(model);
        }
        [HttpPost]
        public bool DeleteCategory(int catId = 0)
        {
            var category = _unitOfWork.ProjectCategoryRepository.GetById(catId);
            if (category == null)
            {
                return false;
            }
            _unitOfWork.ProjectCategoryRepository.Delete(category);
            _unitOfWork.Save();
            return true;
        }
        [HttpPost]
        public bool UpdateProjectCat(int sort = 1, bool active = false, bool menu = false, int projectCatId = 0)
        {
            var projectCat = _unitOfWork.ProjectCategoryRepository.GetById(projectCatId);
            if (projectCat == null)
            {
                return false;
            }
            projectCat.CategorySort = sort;
            projectCat.CategoryActive = active;
            projectCat.ShowMenu = menu;

            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Project 
        public ActionResult ListProject(int? page, string name, int? parentId, int catId = 0, string sort = "date-desc", string result = "")
        {
            ViewBag.Result = result;
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var projects = _unitOfWork.ProjectRepository.GetQuery().AsNoTracking();
            if (catId > 0)
            {
                projects = projects.Where(a => a.ProjectCategoryId == catId);
            }
            else if (parentId > 0)
            {
                projects = projects.Where(a => a.ProjectCategoryId == parentId);
            }
            if (!string.IsNullOrEmpty(name))
            {
                projects = projects.Where(l => l.ProjectName.Contains(name));
            }

            switch (sort)
            {
                case "sort-asc":
                    projects = projects.OrderBy(a => a.Sort);
                    break;
                case "sort-desc":
                    projects = projects.OrderByDescending(a => a.Sort);
                    break;
                case "date-asc":
                    projects = projects.OrderBy(a => a.CreateDate);
                    break;
                default:
                    projects = projects.OrderByDescending(a => a.CreateDate);
                    break;
            }
            var model = new ListProjectViewModel
            {
                SelectCategories = new SelectList(ProjectCategories.Where(a => a.ParentId == null), "Id", "CategoryName"),
                Projects = projects.ToPagedList(pageNumber, pageSize),
                CatId = catId,
                ParentId = parentId,
                Name = name,
                Sort = sort,
            };
            if (parentId.HasValue)
            {
                model.ChildCategoryList = new SelectList(ProjectCategories.Where(a => a.ParentId == parentId), "Id", "Categoryname");
            }
            return View(model);
        }
        public ActionResult Project()
        {
            var model = new InsertProjectViewModel
            {
                Project = new Project { Sort = 1, Active = true },
                Categories = ProjectCategories,
                Products = _unitOfWork.ProductRepository.GetQuery(a => a.Active, o => o.OrderBy(a => a.Sort)),
            };
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Project(InsertProjectViewModel model, FormCollection fc)
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
                    var imgPath = "/images/projects/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "Project.CoverImage")
                    {
                        model.Project.CoverImage = imgFile;
                    }
                }

                model.Project.ListImage = fc["Pictures"];
                model.Project.Url = HtmlHelpers.ConvertToUnSign(null, model.Project.Url ?? model.Project.ProjectName);
                model.Project.ProjectCategoryId = model.CategoryId ?? model.ParentId;
                model.Project.ProductId = model.ProductId;

                _unitOfWork.ProjectRepository.Insert(model.Project);
                _unitOfWork.Save();

                return RedirectToAction("ListProject", new { result = "success" });
            }
            model.Categories = ProjectCategories;
            return View(model);
        }
        public ActionResult UpdateProject(int proId = 0)
        {
            var project = _unitOfWork.ProjectRepository.GetById(proId);
            if (project == null)
            {
                return RedirectToAction("ListProject");
            }
            var model = new InsertProjectViewModel
            {
                Project = project,
                Categories = ProjectCategories,
                ParentId = project.ProjectCategory.ParentId ?? project.ProjectCategoryId,
                CategoryId = project.ProjectCategoryId,
                Products = _unitOfWork.ProductRepository.GetQuery(a => a.Active, o => o.OrderBy(a => a.Sort)),
            };
            if (model.ParentId > 0)
            {
                model.ChildCategoryList = new SelectList(ProjectCategories.Where(a => a.ParentId == model.ParentId), "Id", "CategoryName");
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateProject(InsertProjectViewModel model, FormCollection fc)
        {
            var project = _unitOfWork.ProjectRepository.GetById(model.Project.Id);
            if (project == null)
            {
                return RedirectToAction("ListProject");
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
                    var imgPath = "/images/projects/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "Project.CoverImage")
                    {
                        project.CoverImage = imgFile;
                    }
                }

                var file = Request.Files["Project.CoverImage"];

                if (file != null && file.ContentLength == 0)
                {
                    project.CoverImage = fc["CurrentFile"] == "" ? null : fc["CurrentFile"];
                }

                project.ListImage = fc["Pictures"] == "" ? null : fc["Pictures"];
                project.Url = HtmlHelpers.ConvertToUnSign(null, model.Project.Url ?? model.Project.ProjectName);
                project.ProjectCategoryId = model.CategoryId ?? model.ParentId;
                project.ProductId = model.ProductId;
                project.ProjectName = model.Project.ProjectName;
                project.Description = model.Project.Description;
                project.Body = model.Project.Body;
                project.Active = model.Project.Active;
                project.Home = model.Project.Home;
                project.TitleMeta = model.Project.TitleMeta;
                project.DescriptionMeta = model.Project.DescriptionMeta;
                project.Sort = model.Project.Sort;
                project.Place = model.Project.Place;
                project.StartDate = model.Project.StartDate;
                project.EndDate = model.Project.EndDate;
                project.Client = model.Project.Client;

                _unitOfWork.Save();
                return RedirectToAction("ListProject", new { result = "update" });
            }
            model.Categories = ProjectCategories;
            if (model.ParentId > 0)
            {
                model.ChildCategoryList = new SelectList(ProjectCategories.Where(a => a.ParentId == model.ParentId), "Id", "CategoryName");
            }
            return View(model);
        }
        [HttpPost]
        public bool DeleteProject(int proId = 0)
        {
            var project = _unitOfWork.ProjectRepository.GetById(proId);
            if (project == null)
            {
                return false;
            }
            _unitOfWork.ProjectRepository.Delete(project);
            _unitOfWork.Save();
            return true;
        }
        [HttpPost]
        public bool QuickUpdate(bool active, bool home, int sort = 0, int projectId = 0)
        {
            var project = _unitOfWork.ProjectRepository.GetById(projectId);
            if (project == null)
            {
                return false;
            }
            project.Sort = sort;
            project.Active = active;
            project.Home = home;

            _unitOfWork.Save();
            return true;
        }
        #endregion

        public JsonResult GetProjectCategory(int? parentId)
        {
            var categories = ProjectCategories.Where(a => a.ParentId == parentId);
            return Json(categories.Select(a => new { a.Id, Name = a.CategoryName }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetChildCategory(int parentId = 0)
        {
            var childCategories = ProjectCategories.Where(a => a.ParentId == parentId);
            return Json(childCategories.Select(a => new { a.Id, a.CategoryName }), JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}