using ApLucDongAnh.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace ApLucDongAnh.Controllers
{
    public class SitemapController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        [Route("sitemap.xml", Order = 0)]
        public ActionResult Index()
        {
            Response.AddHeader("Content-Type", "text/xml");
            return PartialView();
        }
        #region Sitemap - Product
        [Route("sitemap/products.xml")]
        public ContentResult ProductSitemap()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var items = _unitOfWork.ProductRepository.GetQuery(a => a.Active, q => q.OrderByDescending(a => a.Id)).Select(a => new { url = a.Url, time = a.CreateDate }).Take(2000).ToList();
            var itemSitemap = (from item in items
                               select new XElement(ns + "url", new XElement(ns + "loc", Request.Url?.GetLeftPart(UriPartial.Authority) + Url.Action("ProductDetail", "Home", new
                               {
                                   item.url
                               })), new XElement(ns + "lastmod", item.time.ToString("yyyy-MM-dd")), new XElement(ns + "changefreq", "daily"), new XElement(ns + "priority", "0.8"))).ToList();
            var sitemap = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement(ns + "urlset", itemSitemap));
            return Content(sitemap.ToString(), "text/xml");
        }
        #endregion
        #region Sitemap - ProductCategory
        [Route("sitemap/products-categories.xml")]
        public ContentResult ProductCategorySitemap()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var items = _unitOfWork.ProductCategoryRepository.GetQuery(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort)).Select(a => new { a.Url }).ToList();
            var itemSitemap = (from item in items
                               select new XElement(ns + "url", new XElement(ns + "loc", Request.Url?.GetLeftPart(UriPartial.Authority) + Url.Action("ProductCategory", "Home", new
                               {
                                   url = item.Url
                               })), new XElement(ns + "lastmod", DateTime.Now.ToString("yyyy-MM-dd")), new XElement(ns + "changefreq", "daily"), new XElement(ns + "priority", "0.8"))).ToList();
            var sitemap = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement(ns + "urlset", itemSitemap));
            return Content(sitemap.ToString(), "text/xml");
        }
        #endregion
        #region Sitemap - Article
        [Route("sitemap/articles.xml")]
        public ContentResult ArticleSitemap()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var items = _unitOfWork.ArticleRepository.GetQuery(a => a.Active, q => q.OrderByDescending(a => a.Id)).Select(a => new { a.Url, catUrl = a.ArticleCategory.Url, time = a.CreateDate }).Take(100).ToList();
            var itemSitemap = (from item in items
                               select new XElement(ns + "url", new XElement(ns + "loc", Request.Url?.GetLeftPart(UriPartial.Authority) + Url.Action("ArticleDetail", "Home", new
                               {
                                   url = item.Url
                               })), new XElement(ns + "lastmod", item.time.ToString("yyyy-MM-dd")), new XElement(ns + "changefreq", "daily"), new XElement(ns + "priority", "0.8"))).ToList();
            var sitemap = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement(ns + "urlset", itemSitemap));
            return Content(sitemap.ToString(), "text/xml");
            //sitemap.Save(Server.MapPath("/Sitemap/ArticleSitemap.xml"));
        }
        #endregion
        #region Sitemap - ArticleCategory
        [Route("sitemap/article-categories.xml")]
        public ContentResult ArticleCategorySitemap()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var items = _unitOfWork.ArticleCategoryRepository.GetQuery(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort)).Select(a => new { a.Url }).ToList();
            var itemSitemap = (from item in items
                               select new XElement(ns + "url", new XElement(ns + "loc", Request.Url?.GetLeftPart(UriPartial.Authority) + Url.Action("ArticleCategory", "Home", new
                               {
                                   url = item.Url
                               })), new XElement(ns + "lastmod", DateTime.Now.ToString("yyyy-MM-dd")), new XElement(ns + "changefreq", "daily"), new XElement(ns + "priority", "0.8"))).ToList();
            var sitemap = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement(ns + "urlset", itemSitemap));
            return Content(sitemap.ToString(), "text/xml");
            //sitemap.Save(Server.MapPath("/Sitemap/ArticleCategorySitemap.xml"));
        }
        #endregion

        [Route("sitemap/article-images.xml")]
        public ContentResult ArticleImageSitemap()
        {
            XNamespace nsSitemap = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XNamespace nsImage = "http://www.google.com/schemas/sitemap-image/1.1";

            var articles = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && a.Image != null, q => q.OrderBy(a => a.Id)).Select(a => new
            {
                url = a.Url,
                image = a.Image
            });
            var elements = new List<XElement>();
            foreach (var article in articles)
            {
                var itemUrl = Request.Url?.GetLeftPart(UriPartial.Authority) + Url.Action("ArticleDetail", "Home", new { article.url });

                var images = new List<string>();

                if (article.image != null)
                {
                    images.Add(Request.Url?.GetLeftPart(UriPartial.Authority) + Path.Combine("/images/articles/", article.image));
                }
                //if (article.imageList != null)
                //{
                //    var img = Array.ConvertAll(article.imageList.Split(','),
                //        c => c.Insert(0, Request.Url?.GetLeftPart(UriPartial.Authority) + Path.Combine("/images/projects/")));
                //    images.AddRange(img);
                //}
                var urlSet = new XElement(new XElement(nsSitemap + "url",
                        new XElement(nsSitemap + "loc", itemUrl),
                        from urlNode in images
                        select new XElement(nsImage + "image",
                            new XElement(nsImage + "loc", urlNode)
                        )));
                elements.Add(urlSet);
            }

            var sitemap = new XDocument(new XDeclaration("1.0", "UTF-8", ""), new XElement(nsSitemap + "urlset", new XAttribute("xmlns", nsSitemap), new XAttribute(XNamespace.Xmlns + "image", nsImage), elements));
            return Content(sitemap.ToString(), "text/xml");
        }
        [Route("sitemap/product-images.xml")]
        public ContentResult ProductImageSitemap()
        {
            XNamespace nsSitemap = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XNamespace nsImage = "http://www.google.com/schemas/sitemap-image/1.1";

            var articles = _unitOfWork.ProductRepository.GetQuery(a => a.Active && a.ListImage != null, q => q.OrderBy(a => a.Id)).Select(a => new
            {
                url = a.Url,
                image = a.ListImage
            });
            var elements = new List<XElement>();
            foreach (var article in articles)
            {
                var itemUrl = Request.Url?.GetLeftPart(UriPartial.Authority) + Url.Action("ProductDetail", "Home", new { article.url });
                var images = Array.ConvertAll(article.image.Split(','),
                    c => c.Insert(0, Request.Url?.GetLeftPart(UriPartial.Authority) + Path.Combine("/images/products/")));

                var urlItem = new XElement(new XElement(nsSitemap + "url",
                        new XElement(nsSitemap + "loc", itemUrl),
                        from urlNode in images
                        select new XElement(nsImage + "image",
                            new XElement(nsImage + "loc", urlNode)
                        )));
                elements.Add(urlItem);
            }

            var sitemap = new XDocument(new XDeclaration("1.0", "UTF-8", ""), new XElement(nsSitemap + "urlset", new XAttribute("xmlns", nsSitemap), new XAttribute(XNamespace.Xmlns + "image", nsImage), elements));
            return Content(sitemap.ToString(), "text/xml");
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}