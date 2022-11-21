﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApLucDongAnh.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Display(Name = "Tên sản phẩm", Description = "Tên sản phẩm dài tối đa 200 ký tự")
            , Required(ErrorMessage = "Hãy nhập Tên sản phẩm"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("TextBox")]
        public string Name { get; set; }
        [Display(Name = "Trích dẫn ngắn"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string ShortDescription { get; set; }

        [Display(Name = "Mô tả sản phẩm"), UIHint("EditorBox")]
        public string Description { get; set; }

        [Display(Name = "Thông tin sản phẩm"), UIHint("EditorBox")]
        public string Infomation { get; set; }
        [Display(Name = "Chi tiết kỹ thuật"), UIHint("EditorBox")]
        public string Specification { get; set; }

        [Display(Name = "Ảnh bìa"), StringLength(500)]
        public string CoverImage { get; set; }
        [Display(Name = "Danh sách ảnh"), UIHint("UploadMultiFile")]
        public string ListImage { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [Display(Name = "Ngày đăng")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự")
            , RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Sort { get; set; }
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }

        [Display(Name = "Sản phẩm bán chạy")]
        public bool Hot { get; set; }

        [StringLength(300)]
        public string Url { get; set; }
        [Display(Name = "Thẻ tiêu đề"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string TitleMeta { get; set; }
        [Display(Name = "Thẻ mô tả"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string DescriptionMeta { get; set; }

        [Display(Name = "Danh mục sản phẩm"), Required(ErrorMessage = "Hãy chọn danh mục sản phẩm")]
        public int ProductCategoryId { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }

        public Product()
        {
            CreateDate = DateTime.Now;
            Active = true;
        }
    }
}