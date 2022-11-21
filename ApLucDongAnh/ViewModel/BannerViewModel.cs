using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApLucDongAnh.Models;

namespace ApLucDongAnh.ViewModel
{
    public class BannerViewModel
    {
        public Banner Banner { get; set; }
        public SelectList SelectGroup { get; set; }
        public BannerViewModel()
        {
            var listgroup = new Dictionary<int, string>
            {
                { 1, "Banner" },
                { 2, "Đối tác - Khách hàng" },
                { 3, "Kết nối kinh doanh" },
                { 4, "Lợi ích" },
                { 5, "Slide Video" },
                { 6, "Slide Nội dung giới thiệu" },
                { 7, "Banner Slide giới thiệu" },
                { 8, "Lịch sử hình thành" },
                { 9, "Thành tựu" },
            };
            SelectGroup = new SelectList(listgroup, "Key", "Value");
        }
    }
    public class ListBannerViewModel
    {
        public PagedList.IPagedList<Banner> Banners { get; set; }
        public int? GroupId { get; set; }
        public SelectList SelectGroup { get; set; }
        public ListBannerViewModel()
        {
            var listgroup = new Dictionary<int, string>
            {
                { 1, "Banner" },
                { 2, "Đối tác - Khách hàng" },
                { 3, "Kết nối kinh doanh" },
                { 4, "Lợi ích" },
                { 5, "Slide Video" },
                { 6, "Slide Nội dung giới thiệu" },
                { 7, "Banner Slide giới thiệu" },
                { 8, "Lịch sử hình thành" },
                { 9, "Thành tựu" },
            };
            SelectGroup = new SelectList(listgroup, "Key", "Value");
        }
    }
}