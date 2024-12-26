using DoAn.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DoAn.Areas.Admin.Models
{
    public class PaymentAdmin
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        public DateTime? CreateAt { get; set; }

        public float? Total { get; set; }

        
        // Navigation Property
        public virtual Customer Customer { get; set; }

        // Thuộc tính PaymentDetails
        public List<PaymentDetailAdmin> PaymentDetails { get; set; } = new List<PaymentDetailAdmin>(); // Khởi tạo danh sách
    }

}

