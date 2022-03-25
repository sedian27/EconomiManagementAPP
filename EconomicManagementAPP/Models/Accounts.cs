namespace EconomicManagementAPP.Models
{
    public class Accounts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AccountTypeId { get; set; }
        public Decimal Balance { get; set; }
        public string Description { get; set; }
    }
}
