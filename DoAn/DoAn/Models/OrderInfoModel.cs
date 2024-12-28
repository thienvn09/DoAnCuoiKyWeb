namespace DoAn.Models
{
    public class OrderInfoModel
    {
        public string FullName {  get; set; }
        public string OrderId { get; set; }
        public string OrderInfo { get; set; }
        public List<CartItem> CartItems { get; set; }
        public string Amount { get; set; }
    }
}
