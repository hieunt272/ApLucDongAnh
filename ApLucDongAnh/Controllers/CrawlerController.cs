using ApLucDongAnh.DAL;
using ApLucDongAnh.Models;
using Helpers;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApLucDongAnh.Controllers
{
    public class CrawlerController : Controller
    {
        private readonly HtmlWeb _hw = new HtmlWeb();

        // GET: Crawler
        public void Index(string urlSource, int page, int catId, int bg = 2)
        {
            var urlList = new List<string> { urlSource };
            for (var i = bg; i <= page; i++)
            {
                urlList.Add(urlSource + "/page/" + i + "/");
            }
            using (var unitOfwork = new UnitOfWork())
            {
                var allList = new List<ItemProduct>();
                foreach (var url in urlList)
                {
                    var urlListOnePage = GetUrlProduct(url);
                    allList.AddRange(urlListOnePage);
                }
                var i = 1;
                foreach (var item in allList)
                {
                    var productInfo = UpdateProductInfo(item.Url, item.Subject, item.Image, catId, i);
                    if (productInfo == null) continue;

                    unitOfwork.ProductRepository.Insert(productInfo);
                    unitOfwork.Save();
                    i++;
                }
            }
        }

        public void CrawArticle()
        {
            var urlList = new List<string>();
            //for (var i = 3; i > 1; i--)
            //{
            //    urlList.Add("https://lohoidonganh.com/category/cong-trinh-tieu-bieu/page/" + i + "/");
            //}
            //urlList.Add("https://lohoidonganh.com/category/cong-trinh-tieu-bieu");

            using (var unitOfwork = new UnitOfWork())
            {
                foreach (var url in urlList)
                {
                    var urlListOnePage = GetUrl(url);

                    foreach (var article in urlListOnePage)
                    {
                        var articleResult = GetContentFromUrl(article.Url, article.Title, article.Description, article.Image, 2);
                        if (articleResult == null) continue;
                        unitOfwork.ArticleRepository.Insert(articleResult);
                        unitOfwork.Save();
                    }
                }
            }
        }

        public void CrawProject()
        {
            var urlList = new List<string>();
            for (var i = 3; i > 1; i--)
            {
                urlList.Add("https://lohoidonganh.com/category/cong-trinh-tieu-bieu/page/" + i + "/");
            }
            urlList.Add("https://lohoidonganh.com/category/cong-trinh-tieu-bieu");

            using (var unitOfwork = new UnitOfWork())
            {
                foreach (var url in urlList)
                {
                    var urlListOnePage = GetUrl(url);

                    foreach (var item in urlListOnePage)
                    {
                        var project = GetProjectContentFromUrl(item.Url, item.Title, item.Description, item.Image, 4);
                        if (project == null) continue;
                        unitOfwork.ProjectRepository.Insert(project);
                        unitOfwork.Save();
                    }
                }
            }
        }

        public List<ItemArticle> GetUrl(string pageurl)
        {
            var doc = _hw.Load(pageurl);
            var itemnode = doc.DocumentNode.SelectNodes(".//div[@class='sh-blog-shortcode style-3']//article");
            if (itemnode == null) return null;

            var listUrl = new List<ItemArticle>();
            foreach (var t in itemnode)
            {
                var item = new ItemArticle();
                var nodeurl = t.SelectSingleNode(".//h3[@class='entry-title']/a");
                if (nodeurl == null) continue;

                var url = nodeurl.Attributes["href"].Value;
                item.Url = url;

                var titleNode = t.SelectSingleNode(".//h3[@class='entry-title']");
                if (titleNode != null)
                {
                    item.Title = titleNode.InnerText.Replace("\t", "");
                }

                var imgNode = t.SelectSingleNode(".//div[@class='entry-thumb']//noscript/img");
                item.Image = imgNode?.Attributes["src"].Value;

                //var nodeDesc = t.SelectSingleNode(".//div[@class='elementor-post__excerpt']");
                //if (nodeDesc != null)
                //{
                //    item.Description = nodeDesc.InnerText.Replace("\t", "");
                //}
                listUrl.Add(item);
            }

            return listUrl;
        }

        public Article GetContentFromUrl(string url, string subject, string description, string image, int catId)
        {
            if (CheckExist(url)) return null;

            var doc = _hw.Load(url);
            var node = doc.DocumentNode.SelectSingleNode("//*[@id='main']");
            if (node == null) return null;

            var moreNode = node.SelectSingleNode(".//div[@class='sh-blog-shortcode style-3']");
            moreNode?.Remove();

            var article = new Article();

            var newsubject = subject.Trim();

            if (newsubject.Length < 5) return null;
            if (newsubject.Length > 150)
            {
                newsubject = newsubject.Substring(1, 150);
            }

            article.Subject = HttpUtility.HtmlDecode(newsubject);
            article.Description = description ?? article.Subject;

            var published = doc.DocumentNode.SelectSingleNode("//meta[@property='article:published_time']");
            if (published != null)
            {
                var dateString = published.Attributes["content"].Value;
                var datetime = Convert.ToDateTime(dateString);

                article.CreateDate = datetime;
            }
            else
            {
                var dateNode = doc.DocumentNode.SelectSingleNode("//meta[@property='og:modified_time']");
                //var updateNode = doc.DocumentNode.SelectSingleNode(".//time[@class='updated']");

                if (dateNode != null)
                {
                    var dateString = dateNode.Attributes["content"].Value;
                    var datetime = Convert.ToDateTime(dateString);

                    article.CreateDate = datetime;
                }
            }

            var bodyNode = node.SelectSingleNode(".//div[@class='entry-content']");
            var tocs = bodyNode.SelectSingleNode(".//div[@id='ez-toc-container']");
            tocs?.Remove();

            var allImages = bodyNode.SelectNodes(".//img");
            if (allImages != null)
            {
                foreach (var imageItem in allImages)
                {
                    imageItem.NextSibling?.Remove();

                    var src = imageItem.Attributes["src"];
                    src?.Remove();

                    var lazyNode = imageItem.Attributes["data-lazy-srcset"];
                    //lazyNode?.Remove();
                    if (lazyNode != null)
                    {
                        lazyNode.Name = lazyNode.Name.Replace("data-lazy-srcset", "srcset");
                    }

                    var lazySize = imageItem.Attributes["data-lazy-sizes"];
                    lazySize?.Remove();

                    var dataSrc = imageItem.Attributes["data-lazy-src"];
                    if (dataSrc != null)
                    {
                        dataSrc.Name = dataSrc.Name.Replace("data-lazy-src", "src");
                        //dataSrc.Value = dataSrc.Value.Replace("", "");
                    }
                }
            }

            var bodycontent = bodyNode.InnerHtml.Trim();
            //bodycontent = Regex.Replace(bodycontent, @"\<[\/]*(h|a|span|strong|p|div|img|ul|li|blockquote)[^\>]*\>", "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //bodycontent = Regex.Replace(bodycontent, @"<\s*\w*\s*style.*?>", "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //bodycontent = Regex.Replace(bodycontent, @"(<br?/?>)+", "<br />", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //bodycontent = Regex.Replace(bodycontent, @"(<br ?/?>)+", "<br />", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            bodycontent = HttpUtility.HtmlDecode(bodycontent).Trim();

            if (bodycontent.Length < 5)
            {
                return null;
            }
            //string imgFileName = null;
            //if (!string.IsNullOrEmpty(img))
            //{
            //    if (HtmlHelpers.CheckFileExt(img, "jpg|jpeg|png|gif"))
            //    {
            //        var rq = new WebClient();
            //        var dataImg = rq.DownloadData(img);

            //        using (var mem = new MemoryStream(dataImg))
            //        {
            //            using (var newImage = Image.FromStream(mem))
            //            {
            //                var imgPath = "/images/articles/" + DateTime.Now.ToString("yyyy/MM/dd");
            //                HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
            //                imgFileName = Path.GetFileNameWithoutExtension(img) + "-" + DateTime.Now.Millisecond + Path.GetExtension(img);
            //                var fixSizeImage = HtmlHelpers.FixedSize(newImage, 900, 900, false);
            //                HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
            //            }
            //        }
            //    }
            //}

            var title = doc.DocumentNode.SelectSingleNode("//title");
            if (title != null)
            {
                article.TitleMeta = HtmlHelpers.CutString(null, title.InnerText, 90);
            }
            var descNode = doc.DocumentNode.SelectSingleNode("//meta[@name='description']");
            if (descNode != null)
            {
                article.DescriptionMeta = HtmlHelpers.CutString(null, descNode.Attributes["content"].Value, 400);
            }
            //var imgNode = doc.DocumentNode.SelectSingleNode("//meta[@property='og:image']");
            //if (imgNode != null)
            //{
            //    article.Image = imgNode.Attributes["content"].Value;
            //}
            article.Image = image;
            article.Body = bodycontent;
            article.ArticleCategoryId = catId;
            article.Active = true;
            article.Url = url.Replace("https://lohoidonganh.com/", "").Replace(".html", "");
            article.UrlSource = url;
            return article;
        }

        public Project GetProjectContentFromUrl(string url, string subject, string description, string image, int catId)
        {
            if (CheckProjectExist(url)) return null;

            var doc = _hw.Load(url);
            var node = doc.DocumentNode.SelectSingleNode("//*[@id='main']");
            if (node == null) return null;

            var moreNode = node.SelectSingleNode(".//div[@class='sh-blog-shortcode style-3']");
            moreNode?.Remove();

            var project = new Project();

            var newsubject = subject.Trim();

            if (newsubject.Length < 5) return null;
            if (newsubject.Length > 150)
            {
                newsubject = newsubject.Substring(1, 150);
            }

            project.ProjectName = HttpUtility.HtmlEncode(newsubject);
            project.Description = description ?? project.ProjectName;

            var published = doc.DocumentNode.SelectSingleNode("//meta[@property='article:published_time']");
            if (published != null)
            {
                var dateString = published.Attributes["content"].Value;
                var datetime = Convert.ToDateTime(dateString);

                project.CreateDate = datetime;
            }
            else
            {
                var dateNode = doc.DocumentNode.SelectSingleNode("//meta[@property='og:modified_time']");
                //var updateNode = doc.DocumentNode.SelectSingleNode(".//time[@class='updated']");

                if (dateNode != null)
                {
                    var dateString = dateNode.Attributes["content"].Value;
                    var datetime = Convert.ToDateTime(dateString);

                    project.CreateDate = datetime;
                }
            }

            var bodyNode = node.SelectSingleNode(".//div[@class='entry-content']");
            var tocs = bodyNode.SelectSingleNode(".//div[@id='ez-toc-container']");
            tocs?.Remove();

            var allImages = bodyNode.SelectNodes(".//img");
            if (allImages != null)
            {
                foreach (var imageItem in allImages)
                {
                    imageItem.NextSibling?.Remove();

                    var src = imageItem.Attributes["src"];
                    src?.Remove();

                    var lazyNode = imageItem.Attributes["data-lazy-srcset"];
                    //lazyNode?.Remove();
                    if (lazyNode != null)
                    {
                        lazyNode.Name = lazyNode.Name.Replace("data-lazy-srcset", "srcset");
                    }

                    var lazySize = imageItem.Attributes["data-lazy-sizes"];
                    lazySize?.Remove();

                    var dataSrc = imageItem.Attributes["data-lazy-src"];
                    if (dataSrc != null)
                    {
                        dataSrc.Name = dataSrc.Name.Replace("data-lazy-src", "src");
                        //dataSrc.Value = dataSrc.Value.Replace("", "");
                    }
                }
            }

            var bodycontent = bodyNode.InnerHtml.Trim();
            //bodycontent = Regex.Replace(bodycontent, @"\<[\/]*(h|a|span|strong|p|div|img|ul|li|blockquote)[^\>]*\>", "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //bodycontent = Regex.Replace(bodycontent, @"<\s*\w*\s*style.*?>", "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //bodycontent = Regex.Replace(bodycontent, @"(<br?/?>)+", "<br />", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //bodycontent = Regex.Replace(bodycontent, @"(<br ?/?>)+", "<br />", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            bodycontent = HttpUtility.HtmlDecode(bodycontent).Trim();

            if (bodycontent.Length < 5)
            {
                return null;
            }
            //string imgFileName = null;
            //if (!string.IsNullOrEmpty(img))
            //{
            //    if (HtmlHelpers.CheckFileExt(img, "jpg|jpeg|png|gif"))
            //    {
            //        var rq = new WebClient();
            //        var dataImg = rq.DownloadData(img);

            //        using (var mem = new MemoryStream(dataImg))
            //        {
            //            using (var newImage = Image.FromStream(mem))
            //            {
            //                var imgPath = "/images/articles/" + DateTime.Now.ToString("yyyy/MM/dd");
            //                HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
            //                imgFileName = Path.GetFileNameWithoutExtension(img) + "-" + DateTime.Now.Millisecond + Path.GetExtension(img);
            //                var fixSizeImage = HtmlHelpers.FixedSize(newImage, 900, 900, false);
            //                HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
            //            }
            //        }
            //    }
            //}

            var title = doc.DocumentNode.SelectSingleNode("//title");
            if (title != null)
            {
                project.TitleMeta = HtmlHelpers.CutString(null, title.InnerText, 90);
            }
            var descNode = doc.DocumentNode.SelectSingleNode("//meta[@name='description']");
            if (descNode != null)
            {
                project.DescriptionMeta = HtmlHelpers.CutString(null, descNode.Attributes["content"].Value, 400);
            }
            //var imgNode = doc.DocumentNode.SelectSingleNode("//meta[@property='og:image']");
            //if (imgNode != null)
            //{
            //    article.Image = imgNode.Attributes["content"].Value;
            //}
            project.ListImage = image;
            project.Body = bodycontent;
            project.ProjectCategoryId = catId;
            project.Active = true;
            project.Url = url.Replace("https://lohoidonganh.com/", "").Replace(".html", "");
            project.UrlSource = url;
            return project;
        }

        public class ItemArticle
        {
            public string Url { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Image { get; set; }
        }

        public Product UpdateProductInfo(string url, string name, string image, int catId, int sort)
        {
            if (CheckProductExist(url)) return null;

            var doc = _hw.Load(url);
            var product = new Product
            {
                Name = HttpUtility.HtmlDecode(name).Trim(),
                UrlSource = url,
                Url = url.Replace("https://lohoidonganh.com/san-pham/", ""),
                Sort = sort,
                ProductCategoryId = catId,
                ListImage = image
            };

            //var code = doc.DocumentNode.SelectSingleNode("//p[@class='msp']");
            //if (code != null)
            //{
            //    var newCode = code.InnerText.Trim().Replace("Mã sản phẩm : ", "");
            //    product.Code = newCode == "" ? null : newCode;
            //}
            //var price = doc.DocumentNode.SelectSingleNode("//div[@class='giashop']/strong");
            //if (price != null)
            //{
            //    var newPrice = price.InnerText.Replace(".", "").Replace("đ", "").Trim();
            //    product.Price = newPrice == "" ? (decimal?)null : Convert.ToDecimal(newPrice);
            //}

            //var imageNode = doc.DocumentNode.SelectSingleNode("//meta[@property='og:image']");
            //if (imageNode != null)
            //{
            //    product.ListImage = imageNode.Attributes["content"].Value;
            //}

            var dateNode = doc.DocumentNode.SelectSingleNode("//meta[@property='og:modified_time']");
            if (dateNode != null)
            {
                var dateString = dateNode.Attributes["content"].Value;
                var datetime = Convert.ToDateTime(dateString);

                product.CreateDate = datetime;
            }
            var description = doc.DocumentNode.SelectSingleNode("//div[@class='product-short-description']");
            if (description != null)
            {
                product.ShortDescription = description.InnerHtml;
            }
            var body = doc.DocumentNode.SelectSingleNode(".//div[@id='tab-description']");
            if (body != null)
            {
                var titleNode = body.SelectSingleNode(".//h2[@class='woocommerce-description-title']");
                titleNode?.Remove();

                var allImages = body.SelectNodes(".//img");
                if (allImages != null)
                {
                    foreach (var imageItem in allImages)
                    {
                        imageItem.NextSibling?.Remove();

                        var src = imageItem.Attributes["src"];
                        src?.Remove();

                        var lazyNode = imageItem.Attributes["data-lazy-srcset"];
                        //lazyNode?.Remove();
                        if (lazyNode != null)
                        {
                            lazyNode.Name = lazyNode.Name.Replace("data-lazy-srcset", "srcset");
                        }

                        var lazySize = imageItem.Attributes["data-lazy-sizes"];
                        lazySize?.Remove();

                        var dataSrc = imageItem.Attributes["data-lazy-src"];
                        if (dataSrc != null)
                        {
                            dataSrc.Name = dataSrc.Name.Replace("data-lazy-src", "src");
                            //dataSrc.Value = dataSrc.Value.Replace("", "");
                        }
                    }
                }

                product.Description = body.InnerHtml;
            }
            var specs = doc.DocumentNode.SelectSingleNode(".//div[@id='tab-tech']");
            if (specs != null)
            {
                var allImages = specs.SelectNodes(".//img");
                if (allImages != null)
                {
                    foreach (var imageItem in allImages)
                    {
                        imageItem.NextSibling?.Remove();

                        var src = imageItem.Attributes["src"];
                        src?.Remove();

                        var lazyNode = imageItem.Attributes["data-lazy-srcset"];
                        //lazyNode?.Remove();
                        if (lazyNode != null)
                        {
                            lazyNode.Name = lazyNode.Name.Replace("data-lazy-srcset", "srcset");
                        }

                        var lazySize = imageItem.Attributes["data-lazy-sizes"];
                        lazySize?.Remove();

                        var dataSrc = imageItem.Attributes["data-lazy-src"];
                        if (dataSrc != null)
                        {
                            dataSrc.Name = dataSrc.Name.Replace("data-lazy-src", "src");
                            //dataSrc.Value = dataSrc.Value.Replace("", "");
                        }
                    }
                }
                product.Specification = specs.InnerHtml;
            }

            var title = doc.DocumentNode.SelectSingleNode("//title");
            if (title != null)
            {
                product.TitleMeta = HtmlHelpers.CutString(null, title.InnerText, 97);
            }
            var desc = doc.DocumentNode.SelectSingleNode("//meta[@name='description']");
            if (desc != null)
            {
                product.DescriptionMeta = HtmlHelpers.CutString(null, desc.Attributes["content"].Value, 400);
            }
            return product;
        }

        public List<ItemProduct> GetUrlProduct(string pageurl)
        {
            var doc = _hw.Load(pageurl);
            var root = doc.DocumentNode.SelectSingleNode("//ul[@class='row list-products']");

            if (root == null) return null;
            var itemnode = root.SelectNodes("./li");
            if (itemnode == null) return null;

            var listUrl = new List<ItemProduct>();
            foreach (var t in itemnode)
            {
                var item = new ItemProduct();
                var nodeurl = t.SelectSingleNode(".//a");
                if (nodeurl == null) continue;
                var url = nodeurl.Attributes["href"].Value;
                item.Url = url;
                var subject = t.SelectSingleNode(".//h3[@class='woocommerce-loop-product__title']");
                if (subject != null)
                {
                    item.Subject = subject.InnerText.Trim();
                }
                var imgNode = t.SelectSingleNode(".//div[@class='image-product']//noscript/img");
                item.Image = imgNode?.Attributes["src"].Value;

                listUrl.Add(item);
            }
            return listUrl;
        }
        public CategoryInfo CategoryInfoReturn(string pageurl)
        {
            var info = new CategoryInfo();

            var doc = _hw.Load(pageurl);
            var body = doc.DocumentNode.SelectSingleNode("//div[@class='term-description']");
            if (body != null)
            {
                var allImages = body.SelectNodes("//img");
                if (allImages != null)
                {
                    foreach (var image in allImages)
                    {
                        var lazyNode = image.Attributes["data-lazy-srcset"];
                        lazyNode?.Remove();

                        var lazySize = image.Attributes["data-lazy-sizes"];
                        lazySize?.Remove();

                        var src = image.Attributes["src"];
                        src?.Remove();

                        var dataSrc = image.Attributes["data-lazy-src"];
                        if (dataSrc != null)
                        {
                            dataSrc.Name = dataSrc.Name.Replace("data-lazy-src", "src");
                            dataSrc.Value = dataSrc.Value.Replace("quatangep.techdigi.dev", "quatangep.vn");
                        }
                    }
                }

                info.Body = body.InnerHtml;
            }
            var title = doc.DocumentNode.SelectSingleNode("//title");
            if (title != null)
            {
                info.Title = HtmlHelpers.CutString(null, title.InnerText, 97);
            }
            var desc = doc.DocumentNode.SelectSingleNode("//meta[@name='description']");
            if (desc != null)
            {
                info.Description = HtmlHelpers.CutString(null, desc.Attributes["content"].Value, 400);
            }
            return info;
        }

        public class CategoryInfo
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Body { get; set; }

        }
        public class ItemProduct
        {
            public string Url { get; set; }
            public string Subject { get; set; }
            public string Image { get; set; }
        }

        public bool CheckExist(string url)
        {
            using (var unitOfwork = new UnitOfWork())
            {
                var exists = unitOfwork.ArticleRepository.GetQuery(a => a.UrlSource == url).FirstOrDefault();
                return exists != null;
            }
        }
        public bool CheckProductExist(string url)
        {
            using (var unitOfwork = new UnitOfWork())
            {
                var exists = unitOfwork.ProductRepository.GetQuery(a => a.UrlSource == url).FirstOrDefault();
                return exists != null;
            }
        }
        public bool CheckProjectExist(string url)
        {
            using (var unitOfwork = new UnitOfWork())
            {
                var exists = unitOfwork.ProjectRepository.GetQuery(a => a.UrlSource == url).FirstOrDefault();
                return exists != null;
            }
        }
    }
}