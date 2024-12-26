namespace DoAn.Models
{
    public class PaymentDetail
    {
        public int ProductId { get; set; }
        public int PaymentId { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public float Total { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
