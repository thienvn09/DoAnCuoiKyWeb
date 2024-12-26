using System.ComponentModel.DataAnnotations;

namespace DoAn.Models
{
    public class CustomerSignIn
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "*")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "*")]
        [MaxLength(200, ErrorMessage = "Tối đa 200 kí tự")]
        public string Password { get; set; }

    }
}
