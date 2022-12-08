using ApLucDongAnh.Models;
using PagedList;
using System.Collections.Generic;

namespace ApLucDongAnh.ViewModel
{
    public class HomeViewModel
    {
        public Banner Banner { get; set; }
        public IEnumerable<Banner> Banners { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
        public IEnumerable<Article> Services { get; set; }
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<Feedback> Feedbacks { get; set; }
        public Subcribe Subcribe { get; set; }

        public IEnumerable<ArticleCategory> ArticleCategories { get; set; }
        public IEnumerable<ArticleCategory> ServiceCategories { get; set; }
        public IEnumerable<ProductCategory> MenuProductCategories { get; set; }
        public IEnumerable<ProjectCategory> ProjectCategories { get; set; }
    }

    public class HeaderViewModel
    {
        public IEnumerable<ArticleCategory> ArticleCategories { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
        public IEnumerable<ProjectCategory> ProjectCategories { get; set; }
    }
    public class FooterViewModel
    {
        public IEnumerable<ArticleCategory> ArticleCategories { get; set; }
        public IEnumerable<Article> Policies { get; set; }
    }
    public class AllArticleViewModel
    {
        public IPagedList<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> Categories { get; set; }
    }
    public class ArticleCategoryViewModel
    {
        public ArticleCategory RootCategory { get; set; }
        public ArticleCategory Category { get; set; }
        public IPagedList<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> Categories { get; set; }
        public string Sort { get; set; }
    }
    public class ArticleDetailsViewModel
    {
        public Article Article { get; set; }
        public Article Previous { get; set; }
        public Article Next { get; set; }
    }
    public class MenuArticleViewModel
    {
        public IEnumerable<Article> Articles { get; set; }
    }
    public class ArticleSearchViewModel
    {
        public string Keywords { get; set; }
        public IPagedList<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> Categories { get; set; }
    }

    public class ProductDetailViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public Product Product { get; set; }
    }
    public class CategoryProductViewModel
    {
        public ProductCategory Category { get; set; }
        public IEnumerable<ProductCategory> Categories { get; set; }
        public IPagedList<Product> Products { get; set; }
        public string Url { get; set; }
        public string Sort { set; get; }
        public int ProductResult { get; set; }
        public int BeginCount { get; set; }
        public int EndCount { get; set; }
    }

    public class SearchProductViewModel
    {
        public string Keywords { get; set; }
        public IPagedList<Product> Products { get; set; }
        public IEnumerable<ProductCategory> Categories { get; set; }
        public int? CatId { get; set; }
        public string Name { get; set; }
        public string Sort { get; set; }
        public int ProductResult { get; set; }
        public int BeginCount { get; set; }
        public int EndCount { get; set; }
    }

    public class OrderFormViewModel
    {
        public Order Order { get; set; }
        public Product Product { get; set; }
    }

    public class AllServiceViewModel
    {
        public IPagedList<Article> Services { get; set; }
        public IEnumerable<ProjectCategory> ProjectCategories { get; set; }
        public IEnumerable<Feedback> Feedbacks { get; set; }
    }

    public class ServiceDetailsViewModel
    {
        public Article Service { get; set; }
        public Article Previous { get; set; }
        public Article Next { get; set; }
    }

    public class ProjectCategoryViewModel
    {
        public ProjectCategory RootCategory { get; set; }
        public ProjectCategory Category { get; set; }
        public IPagedList<Project> Projects { get; set; }
        public string Url { get; set; }
        public Subcribe Subcribe { get; set; }
    }

    public class ProjectDetailViewModel
    {
        public Project Project { get; set; }
        public Project Previous { get; set; }
        public Project Next { get; set; }
    }

    public class GetProjectViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
    }

    public class IntroduceViewModel
    {
        public About About { get; set; }
        public IEnumerable<Banner> Banners { get; set; }
    }

    public class RecruitViewModel
    {
        public Recruit Recruit { get; set; }
        public IEnumerable<RecruitPosition> RecruitPositions { get; set; }
    }

    public class ArticleItemPartial
    {
        public Article Article { get; set; }
        public int Delay { get; set; }
    }
}