using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EconomicManagementAPP.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly IRepositorieTransactions repositorieTransactions;
        private readonly IRepositorieOperationTypes repositorieOperationTypes;
        private readonly IRepositorieAccounts repositorieAccounts;
        private readonly IRepositorieCategories repositorieCategories;
        private readonly IRepositorieUsers repositorieUsers;


        public TransactionsController(IRepositorieTransactions repositorieTransactions,
                                      IRepositorieOperationTypes repositorieOperationTypes,
                                      IRepositorieAccounts repositorieAccounts,
                                      IRepositorieCategories repositorieCategories,
                                      IRepositorieUsers repositorieUsers)
        {
            this.repositorieTransactions = repositorieTransactions;
            this.repositorieOperationTypes = repositorieOperationTypes;
            this.repositorieAccounts = repositorieAccounts;
            this.repositorieCategories = repositorieCategories;
            this.repositorieUsers = repositorieUsers;
        }

        public async Task<IActionResult> Index()
        {
            var userId = repositorieUsers.GetUserId();
            var transaction = await repositorieTransactions.GetTransactions(userId);
            return View(transaction);
        }

        public async Task<IActionResult> Create()
        {
            var userId = repositorieUsers.GetUserId();
            var OperationTypes = await repositorieOperationTypes.GetOperationTypes();
            var Accounts = await repositorieAccounts.GetAccounts(userId);
            var model = new TransactionsViewModel();

            model.OperationTypes = OperationTypes.Select(x => new SelectListItem(x.Description, x.Id.ToString()));
            model.Accounts = Accounts.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            model.Categories = await GetCategories(userId, 1);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Transactions transaction)
        {
            if (!ModelState.IsValid)
            {
                return View(transaction);
            }

            await repositorieTransactions.Create(transaction);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Modify(int id)
        {
            var userId = repositorieUsers.GetUserId();

            var transaction = await repositorieTransactions.GetTransactionById(id, userId);

            if (transaction is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(transaction);
        }

        [HttpPost]
        public async Task<ActionResult> Modify(Transactions transaction)
        {
            var userId = repositorieUsers.GetUserId();
            var transactionExists = await repositorieTransactions.GetTransactionById(transaction.Id, userId);

            if (transactionExists is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieTransactions.Modify(transaction);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = repositorieUsers.GetUserId();
            var transaction = await repositorieTransactions.GetTransactionById(id, userId);

            if (transaction is null)
            {
                return RedirectToAction("NotFount", "Home");
            }

            return View(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var userId = repositorieUsers.GetUserId();
            var transaction = await repositorieTransactions.GetTransactionById(id, userId);

            if (transaction is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieTransactions.Delete(id);
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> GetCategories(int userId, int operationTypes)
        {
            var categories = await repositorieCategories.GetCategories(userId, operationTypes);
            return categories.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }

        [HttpPost]
        public async Task<IActionResult> GetCategories([FromBody]int operationTypeId) 
        {
            var userId = repositorieUsers.GetUserId();
            var categories = await GetCategories(userId, operationTypeId);
            return Ok(categories);
        }
    }
}
