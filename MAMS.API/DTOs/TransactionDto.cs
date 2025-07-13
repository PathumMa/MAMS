namespace MAMS.API.DTOs
{
    public class TransactionDto
    {
        public decimal? Doctor_fee { get; set; }
        public decimal? Hospital_fee { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
    }
}
