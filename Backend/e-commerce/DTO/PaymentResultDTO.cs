namespace e_commerce.DTO
{
    public class PaymentResultDTO
    {
        public CartDTO CartItems { get; set; }
        public bool IsSuccess { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal FinalAmount { get; set; }
        public string PaymentMode { get; set; }
        public string Message { get; set; }
    }
}