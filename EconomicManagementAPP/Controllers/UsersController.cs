using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;

namespace EconomicManagementAPP.Controllers
{
    public class UsersController : Controller
    {
        private readonly IRepositorieUsers repositorieUsers;

        public UsersController(IRepositorieUsers repositorieUsers)
        {
            this.repositorieUsers = repositorieUsers;
        }

        public async Task<IActionResult> Index()
        {
            var users = await repositorieUsers.GetUsers();
            return View(users);
        }
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(Users user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            user.StandarEmail = user.Email.ToUpper();

            var userExist =
               await repositorieUsers.Exist(user.Email);

            if (userExist)
            {
                ModelState.AddModelError(nameof(user.Email),
                    $"Any Email already use.");

                return View(user);
            }
            await repositorieUsers.Create(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificaryUser(string email)
        {
            var userExist = await repositorieUsers.Exist(email);

            if (userExist)
            {
                return Json($"The account {email} already exist");
            }

            return Json(true);
        }

        //Actualizar
        [HttpGet]
        public async Task<ActionResult> Modify(int id)
        {
            var user = await repositorieUsers.GetUserById(id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(user);
        }
        [HttpPost]
        public async Task<ActionResult> Modify(Users user)
        {
            var userExists = await repositorieUsers.GetUserById(user.Id);

            if (userExists is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieUsers.Modify(user);
            return RedirectToAction("Index");
        }
        // Eliminar
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await repositorieUsers.GetUserById(id);

            if (user is null)
            {
                return RedirectToAction("NotFount", "Home");
            }

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await repositorieUsers.GetUserById(id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieUsers.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
