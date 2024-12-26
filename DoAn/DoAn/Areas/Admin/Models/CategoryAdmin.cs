using System.ComponentModel.DataAnnotations;

namespace DoAn.Areas.Admin.Models
{
    public class CategoryAdmin
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "Tối đa 100 kí tự")]
        public string Title { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(500, ErrorMessage = "Tối đa 500 kí tự")]
        public string Content { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
