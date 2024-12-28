namespace DoAn.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime CreateAt { get; set; }
        public float Total { get; set; }

        // Foreign Key to PaymentMethod
        public string PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }  // Liên kết với PaymentMethod
    }
}
