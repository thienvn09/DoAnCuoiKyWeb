using DoAn.Areas.Admin.Models;
using System.ComponentModel.DataAnnotations;

namespace DoAn.Models
{
    public class MomoPayment : PaymentAdmin
    {
        [MaxLength(100)]
        public string MomoTransactionId { get; set; } // ID giao dịch MoMo

        [MaxLength(500)]
        public string PayUrl { get; set; } // URL thanh toán của MoMo

        [MaxLength(50)]
        public string PaymentStatus { get; set; } // Trạng thái thanh toán (Pending, Completed, Failed)

        public DateTime? PaymentDate { get; set; } // Ngày thanh toán

        [MaxLength(500)]
        public string OrderInfo { get; set; } // Thông tin đơn hàng (mô tả)
    }
}
