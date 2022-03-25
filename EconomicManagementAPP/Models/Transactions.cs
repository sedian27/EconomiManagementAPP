namespace EconomicManagementAPP.Models
{
    public class Transactions
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime TransactionDate { get; set; }
        public Decimal Total { get; set; }
        public int OperationTypedId { get; set; }
        public string Description { get; set; }
        public int AccountId { get; set; }
        public int CategoryId { get; set; }
    }
}
