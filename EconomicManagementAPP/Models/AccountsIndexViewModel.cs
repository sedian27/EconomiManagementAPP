namespace EconomicManagementAPP.Models
{
    public class AccountsIndexViewModel
    {
        public string AccountType { get; set; }
        public IEnumerable<Accounts> Accounts { get; set; }
        public decimal Balance => Accounts.Sum(x => x.Balance);
    }
}
