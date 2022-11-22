using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApLucDongAnh.Models
{
    public class About
    {
        public int Id { get; set; }
        [Display(Name = "Ảnh bìa"), StringLength(500)]
        public string CoverImage { get; set; }
        [Display(Name = "Ảnh năng lực sản xuất"), UIHint("UploadMultiFile")]
        public string ProductionImage { get; set; }
        [Display(Name = "Mô tả ngắn"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string Description { get; set; }
        [Display(Name = "Nội dung"), UIHint("EditorBox")]
        public string Body { get; set; }
        [Display(Name = "Sứ mệnh"), UIHint("EditorBox")]
        public string SuMenh { get; set; }
        [Display(Name = "Tầm nhìn"), UIHint("EditorBox")]
        public string TamNhin { get; set; }
        [Display(Name = "Giá trị cốt lõi"), UIHint("EditorBox")]
        public string GiaTriCotLoi { get; set; }
        [Display(Name = "Năng lực sản xuất"), UIHint("EditorBox")]
        public string Production { get; set; }
    }
}