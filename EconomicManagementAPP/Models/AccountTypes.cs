using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class AccountTypes
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string Name { get; set; }
        public int UserId { get; set; }
        public int OrderAccount { get; set; }
    }
}
