
namespace e_commerce.DTO
{
    public class PaymentsDTO
    {
        //public int ProductId { get; set; }
        public int OrderId { get; set; } // This will be set automatically
        public int PaymentId { get; set; } // This will be set automatically
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string CardNo { get; set; }
        public string ExpiryDate { get; set; }
        public string UPIId { get; set; }
    }
}

