using System.ComponentModel.DataAnnotations;

namespace DoAn.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Tên")]
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "Tối đa 100 kí tự")]
        public string FirstName { get; set; }
        [Display(Name = "Họ")]
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "Tối đa 100 kí tự")]
        public string LastName { get; set; }
        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "*")]
        [MaxLength(500, ErrorMessage = "Tối đa 500 kí tự")]
        public string Address { get; set; }
        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "*")]
        [MaxLength(15, ErrorMessage = "Tối đa 15 kí tự")]
        public string Phone { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "*")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string Email { get; set; }
        public string Img { get; set; }
        [Display(Name = "Ngày sinh")]
        public DateOnly? DateOfBirth { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "*")]
        [MaxLength(200, ErrorMessage = "Tối đa 200 kí tự")]
        public string Password { get; set; }
        public string RandomKey { get; set; }
        public DateTime RegisterAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public int IsActive { get; set; }
        public int Role { get; set; }
    }
}
