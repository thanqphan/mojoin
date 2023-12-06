namespace mojoin.ViewModel
{
    public class UserPackageTransactionViewModel
    {
        public int UserID { get; set; }
        public string ?TransactionType { get; set; }
        public decimal ?Amount { get; set; }
        public DateTime ?TransactionDate { get; set; }
        public string ?PaymentMethod { get; set; }
        public string ?TransactionReference { get; set; }
        public decimal ?PromotionAmount { get; set; }
        public decimal ?ReceivedAmount { get; set; }
        public string ?Note { get; set; }
        public string ?Status { get; set; }

        // Thuộc tính liên quan đến UserPackage
        public int ? TransactionID { get; set; }
        public int ?PackageID { get; set; }
        public int ?RoomID { get; set; }
        public DateTime ?StartDate { get; set; }
        public DateTime ?EndDate { get; set; }
        public int ?Duration { get; set; }
    }
}
