using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApLucDongAnh.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Display(Name = "Id sản phẩm")]
        public int ProductId { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [Display(Name = "Đường dẫn sản phẩm")]
        public string ProductUrl { get; set; }

        [Display(Name = "Ảnh sản phẩm")]
        public string ProductImg { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ICollection<Product> Product { get; set; }
        public CustomerInfo CustomerInfo { get; set; }

    }

    [ComplexType]
    public class CustomerInfo
    {
        [Display(Name = "Họ và tên *"), Required(ErrorMessage = "Hãy nhập họ tên"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("Textbox")]
        public string FullName { get; set; }
        [Display(Name = "Địa chỉ"), Required(ErrorMessage = "Hãy nhập địa chỉ"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("Textbox")]
        public string Address { get; set; }
        [Display(Name = "Số điện thoại"), Required(ErrorMessage = "Vui lòng nhập số điện thoại"),
            RegularExpression(@"^\(?(09|03|07|08|05)\)?[-. ]?([0-9]{8})$", ErrorMessage = "Số điện thoại không đúng định dạng!"),
            StringLength(10, ErrorMessage = "Tối đa 11 ký tự"), UIHint("TextBox")]
        public string Mobile { get; set; }
        [Display(Name = "Email"), EmailAddress(ErrorMessage = "Email không hợp lệ"), Required(ErrorMessage = "Hãy nhập Email"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("Textbox")]
        public string Email { get; set; }
        [Display(Name = "Yêu cầu thêm"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), DataType(DataType.MultilineText)]
        public string Body { get; set; }
    }
}