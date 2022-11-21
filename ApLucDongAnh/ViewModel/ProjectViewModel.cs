using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApLucDongAnh.Models;

namespace ApLucDongAnh.ViewModel
{

    public class InsertProjectCatViewModel
    {
        public SelectList RootCats { get; set; }
        public ProjectCategory ProjectCategory { get; set; }
    }
    public class ListProjectViewModel
    {
        public PagedList.IPagedList<Project> Projects { get; set; }
        public SelectList SelectCategories { get; set; }
        public SelectList ChildCategoryList { get; set; }
        public int? ParentId { get; set; }
        public int? CatId { get; set; }
        public string Name { get; set; }
        public string Sort { get; set; }

        public ListProjectViewModel()
        {
            ChildCategoryList = new SelectList(new List<ProjectCategory>(), "Id", "CategoryName");
        }
    }
    public class InsertProjectViewModel
    {
        public Project Project { get; set; }
        [Display(Name = "Danh mục dự án con"), Required(ErrorMessage = "Hãy chọn danh mục dự án")]
        public int ParentId { get; set; }
        [Display(Name = "Danh mục dự án")]
        public int? CategoryId { get; set; }
        [Display(Name = "Sản Phẩm")]
        public int? ProductId { get; set; }
        public IEnumerable<ProjectCategory> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public SelectList SelectCategories { get; set; }
        public SelectList ChildCategoryList { get; set; }
        public SelectList ProjectCategoryList { get; set; }
        public InsertProjectViewModel()
        {
            ChildCategoryList = new SelectList(new List<ProjectCategory>(), "Id", "CategoryName");
        }
    }
}