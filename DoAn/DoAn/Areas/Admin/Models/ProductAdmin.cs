using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DoAn.Areas.Admin.Models
{
    public class ProductAdmin
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "Tối đa 100 kí tự")]
        public string Title { get; set; }

        [Display(Name = "Content")]
        [Required(ErrorMessage = "*")]
        [MaxLength(500, ErrorMessage = "Tối đa 500 kí tự")]
        public string Content { get; set; }

        [Display(Name = "Image")]
        public string? Img { get; set; }


        [Display(Name = "Price")]
        [Required(ErrorMessage = "*")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public int Price { get; set; }

        [Display(Name = "Rating")]
        [Required(ErrorMessage = "*")]
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
        public double Rate { get; set; }
    
        [Display(Name = "Create At")]
        public DateTime CreateAt { get; set; }

        [Display(Name = "Update At")]
        public DateTime UpdateAt { get; set; }

        [Display(Name = "Category Id")]
        [Required(ErrorMessage = "*")]
        public int CategoryId { get; set; }

        [Display(Name = "Category Title")]
        [Required(ErrorMessage = "*")]
        public string CategoryTitle { get; set; }
    }
    public class ProductFormAdmin : ProductAdmin
    {
        public string IdCategorySelected { get; set; }
        public List<SelectListItem>? ListCategory { get; set; }

    }
    public class ProductAdminModel
    {
        ///<summary>
        /// Gets or sets ProductAdmin.
        ///</summary>
        public List<ProductAdmin> ProductAdmins { get; set; }

        ///<summary>
        /// Gets or sets CurrentPageIndex.
        ///</summary>
        public int CurrentPageIndex { get; set; }

        ///<summary>
        /// Gets or sets PageCount.
        ///</summary>
        public int PageCount { get; set; }
    }

}

