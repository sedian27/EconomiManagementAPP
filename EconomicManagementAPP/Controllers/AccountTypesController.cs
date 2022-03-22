using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;

namespace EconomicManagementAPP.Controllers
{
    public class AccountTypesController : Controller
    {
        private readonly IRepositorieAccountTypes repositorieAccountTypes;

        public AccountTypesController(IRepositorieAccountTypes repositorieAccountTypes)
        {
            this.repositorieAccountTypes = repositorieAccountTypes;
        }

        // Creamos index para ejecutar la interfaz
        public async Task<IActionResult> Index()
        {
            var userId = 1;
            var accountTypes = await repositorieAccountTypes.getAccounts(userId);
            return View(accountTypes);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountTypes accountTypes)
        {
            if (!ModelState.IsValid)
            {
                return View(accountTypes);
            }

            accountTypes.UserId = 1;
            accountTypes.OrderAccount = 1;

            // Validamos si ya existe antes de registrar
            var accountTypeExist =
               await repositorieAccountTypes.Exist(accountTypes.Name, accountTypes.UserId);

            if (accountTypeExist)
            {
                // AddModelError ya viene predefinido en .net
                // nameOf es el tipo del campo
                ModelState.AddModelError(nameof(accountTypes.Name),
                    $"The account {accountTypes.Name} already exist.");

                return View(accountTypes);
            }
            await repositorieAccountTypes.Create(accountTypes);
            // Redireccionamos a la lista
            return RedirectToAction("Index");
        }

        // Hace que la validacion se active automaticamente desde el front
        [HttpGet]
        public async Task<IActionResult> VerificaryAccountType(string Name)
        {
            var UserId = 1;
            var accountTypeExist = await repositorieAccountTypes.Exist(Name, UserId);

            if (accountTypeExist)
            {
                // permite acciones directas entre front y back
                return Json($"The account {Name} already exist");
            }

            return Json(true);
        }
    }
}
