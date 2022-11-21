using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApLucDongAnh.Models
{
    public class Recruit
    {
        public int Id { get; set; }
        [Display(Name = "Mô tả ngắn"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string Description { get; set; }
        [Display(Name = "Khả năng là vô tận"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string Ability { get; set; }
        [Display(Name = "Năng lực sản xuất"), UIHint("EditorBox")]
        public string Position { get; set; }
        [Display(Name = "Ảnh bìa"), StringLength(500)]
        public string CoverImage { get; set; }
        [Display(Name = "Ảnh giới thiệu"), StringLength(500)]
        public string Image { get; set; }
    }

    public class RecruitPosition
    {
        public int Id { get; set; }
        [Display(Name = "Vị trí"), Required(ErrorMessage = "Hãy nhập vị trí"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string PositionName { get; set; }
        [Display(Name = "Giới thiệu nhanh"), StringLength(150, ErrorMessage = "Tối đa 150 ký tự"), UIHint("TextBox")]
        public string Introduction { get; set; }
        [Display(Name = "Nội dung"), UIHint("EditorBox")]
        public string Body { get; set; }
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), 
         RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Sort { get; set; }

        public RecruitPosition()
        {
            Active = true;
        }
    }
}