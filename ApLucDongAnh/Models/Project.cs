using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApLucDongAnh.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Display(Name = "Tên dự án", Description = "Tên dự án dài tối đa 150 ký tự"),
         Required(ErrorMessage = "Hãy nhập tên dự án"), StringLength(150, ErrorMessage = "Tối đa 150 ký tự"),
         UIHint("TextBox")]
        public string ProjectName { get; set; }
        [Display(Name = "Ảnh bìa"), StringLength(500)]
        public string CoverImage { get; set; }
        [Display(Name = "Danh sách ảnh"), UIHint("UploadMultiFile")]
        public string ListImage { get; set; }
        [Display(Name = "Khách hàng"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự")]
        public string Client { get; set; }
        [Display(Name = "Ngày bắt đầu")]
        public string StartDate { get; set; }
        [Display(Name = "Ngày kết thúc")]
        public string EndDate { get; set; }
        [Display(Name = "Địa điểm"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("TextBox")]
        public string Place { get; set; }
        [Display(Name = "Giới thiệu ngắn"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string Description { get; set; }
        [Display(Name = "Mô tả chi tiết"), UIHint("EditorBox")]
        public string Body { get; set; }
        [Display(Name = "Ngày đăng")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Sort { get; set; }
        [Display(Name = "Danh mục dự án"), Required(ErrorMessage = "Hãy chọn danh mục dự án")]
        public int ProjectCategoryId { get; set; }
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
        [Display(Name = "Hiện trang chủ")]
        public bool Home { get; set; }
        [Display(Name="Đường dẫn"),StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextBox")]
        public string Url { get; set; }
        [Display(Name = "Thẻ tiêu đề"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string TitleMeta { get; set; }
        [Display(Name = "Thẻ mô tả"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string DescriptionMeta { get; set; }
        public virtual ProjectCategory ProjectCategory { get; set; }
        [Display(Name = "Sản phẩm")]
        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }
        public Project()
        {
            CreateDate = DateTime.Now;
            Active = true;
        }
    }
}