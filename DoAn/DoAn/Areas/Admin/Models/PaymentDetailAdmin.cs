using DoAn.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Areas.Admin.Models
{
    public class PaymentDetailAdmin
    {
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("Payment")]
        public int PaymentId { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }

        public float? Total { get; set; }

        public DateTime? CreateAt { get; set; }

        // Navigation Properties
        public virtual Product Product { get; set; }

        public virtual Payment Payment { get; set; }
    }
}
