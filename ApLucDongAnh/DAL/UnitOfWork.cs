using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApLucDongAnh.Models;

namespace ApLucDongAnh.DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly DataEntities _context = new DataEntities();
        private GenericRepository<Admin> _adminRepository;
        private GenericRepository<ArticleCategory> _articategoryRepository;
        private GenericRepository<Article> _articleRepository;
        private GenericRepository<Banner> _bannerRepository;
        private GenericRepository<Contact> _contactRepository;
        private GenericRepository<ConfigSite> _configRepository;
        private GenericRepository<Product> _productRepository;
        private GenericRepository<ProductCategory> _productCategoryRepository;
        private GenericRepository<Subcribe> _subcribeRepository;
        private GenericRepository<Feedback> _feedbackRepository;
        private GenericRepository<Order> _orderRepository;
        private GenericRepository<Project> _projectRepository;
        private GenericRepository<ProjectCategory> _projectCategoryRepository;
        private GenericRepository<About> _aboutRepository;
        private GenericRepository<Recruit> _recruitRepository;
        private GenericRepository<RecruitPosition> _recruitPositionRepository;

        public GenericRepository<About> AboutRepository =>
            _aboutRepository ?? (_aboutRepository = new GenericRepository<About>(_context));
        public GenericRepository<Recruit> RecruitRepository =>
            _recruitRepository ?? (_recruitRepository = new GenericRepository<Recruit>(_context));
        public GenericRepository<RecruitPosition> RecruitPositionRepository =>
            _recruitPositionRepository ?? (_recruitPositionRepository = new GenericRepository<RecruitPosition>(_context));
        public GenericRepository<Project> ProjectRepository =>
            _projectRepository ?? (_projectRepository = new GenericRepository<Project>(_context));
        public GenericRepository<ProjectCategory> ProjectCategoryRepository =>
            _projectCategoryRepository ?? (_projectCategoryRepository = new GenericRepository<ProjectCategory>(_context));
        public GenericRepository<Order> OrderRepository =>
            _orderRepository ?? (_orderRepository = new GenericRepository<Order>(_context));
        public GenericRepository<Feedback> FeedbackRepository =>
            _feedbackRepository ?? (_feedbackRepository = new GenericRepository<Feedback>(_context));
        public GenericRepository<Subcribe> SubcribeRepository =>
            _subcribeRepository ?? (_subcribeRepository = new GenericRepository<Subcribe>(_context));
        public GenericRepository<Product> ProductRepository =>
            _productRepository ?? (_productRepository = new GenericRepository<Product>(_context));
        public GenericRepository<ProductCategory> ProductCategoryRepository =>
            _productCategoryRepository ?? (_productCategoryRepository = new GenericRepository<ProductCategory>(_context));
        public GenericRepository<ConfigSite> ConfigSiteRepository =>
            _configRepository ?? (_configRepository = new GenericRepository<ConfigSite>(_context));
        public GenericRepository<Contact> ContactRepository =>
            _contactRepository ?? (_contactRepository = new GenericRepository<Contact>(_context));
        public GenericRepository<Banner> BannerRepository =>
            _bannerRepository ?? (_bannerRepository = new GenericRepository<Banner>(_context));
        public GenericRepository<Article> ArticleRepository =>
            _articleRepository ?? (_articleRepository = new GenericRepository<Article>(_context));
        public GenericRepository<ArticleCategory> ArticleCategoryRepository =>
            _articategoryRepository ?? (_articategoryRepository = new GenericRepository<ArticleCategory>(_context));
        public GenericRepository<Admin> AdminRepository =>
            _adminRepository ?? (_adminRepository = new GenericRepository<Admin>(_context));
        public void Save()
        {
            _context.SaveChanges();
        }
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}