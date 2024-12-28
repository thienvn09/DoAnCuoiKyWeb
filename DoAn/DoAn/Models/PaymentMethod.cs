
namespace DoAn.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; } // Ví dụ: "Momo", "VNPay", "COD"
        public string Description { get; set; } // Mô tả chi tiết phương thức thanh toán (nếu cần)

        public ICollection<Payment> Payments { get; set; } // Mối quan hệ 1-nhiều với bảng Payment

    
    }
}
