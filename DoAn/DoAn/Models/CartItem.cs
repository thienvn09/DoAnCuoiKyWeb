namespace DoAn.Models
{
    public class CartItem
    {
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public int Price { get; set; }
        public double Rate { get; set; }
        public int Quantity { get; set; }
        public int Total => Price * Quantity;
    }
}
