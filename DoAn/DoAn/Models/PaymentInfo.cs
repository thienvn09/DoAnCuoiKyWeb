namespace DoAn.Models
{
    public class PaymentInfo
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public string RequestId { get; set; }
        public decimal Amount { get; set; }
        public string OrderInfo { get; set; }
        public string TransId { get; set; }
        public DateTime ResponseTime { get; set; }
        public int ResultCode { get; set; }
        public string Message { get; set; }
    }
}
