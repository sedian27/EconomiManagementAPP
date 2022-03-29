﻿using AutoMapper;
using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EconomicManagementAPP.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IRepositorieCategories repositorieCategories;
        private readonly IRepositorieOperationTypes repositorieOperationTypes;
        private readonly IRepositorieUsers repositorieUsers;
        private readonly IMapper mapper;

        public CategoriesController(IRepositorieCategories repositorieCategories,
                                    IRepositorieOperationTypes repositorieOperationTypes,
                                    IRepositorieUsers repositorieUsers,
                                    IMapper mapper)
        {
            this.repositorieCategories = repositorieCategories;
            this.repositorieOperationTypes = repositorieOperationTypes;
            this.repositorieUsers = repositorieUsers;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var userId = repositorieUsers.GetUserId();
            var categorie = await repositorieCategories.GetCategories(userId);
            return View(categorie);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CategoriesViewModel();
            model.OperationTypes = await GetOperationTypes();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Categories categorie)
        {
            var userId = repositorieUsers.GetUserId();
            categorie.UserId = userId;

            if (!ModelState.IsValid)
            {
                return View(categorie);
            }

            var categorieExist =
               await repositorieCategories.Exist(categorie.Name, userId);

            if (categorieExist)
            {
                ModelState.AddModelError(nameof(categorie.Name),
                    $"The categorie {categorie.Name} already exist.");

                return View(categorie);
            }
            await repositorieCategories.Create(categorie);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificaryCategorie(string Name)
        {
            var UserId = 1;
            var categorieExist = await repositorieCategories.Exist(Name, UserId);

            if (categorieExist)
            {
                return Json($"The categorie {Name} already exist");
            }

            return Json(true);
        }

        [HttpGet]
        public async Task<ActionResult> Modify(int id)
        {
            var userId = repositorieUsers.GetUserId();
            var categorie = await repositorieCategories.GetCategorieById(id, userId);

            if (categorie is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var model = mapper.Map<CategoriesViewModel>(categorie);
            model.OperationTypes = await GetOperationTypes();

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Modify(Categories categorie)
        {
            var userId = repositorieUsers.GetUserId();
            var categorieExists = await repositorieCategories.GetCategorieById(categorie.Id, userId);

            if (categorieExists is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieCategories.Modify(categorie);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = repositorieUsers.GetUserId();
            var categorie = await repositorieCategories.GetCategorieById(id, userId);

            if (categorie is null)
            {
                return RedirectToAction("NotFount", "Home");
            }

            return View(categorie);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var userId = repositorieUsers.GetUserId();
            var categorie = await repositorieCategories.GetCategorieById(id, userId);

            if (categorie is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieCategories.Delete(id);
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> GetOperationTypes() 
        {
            var operationTypes = await repositorieOperationTypes.GetOperationTypes();
            return operationTypes.Select(x => new SelectListItem(x.Description, x.Id.ToString()));
        }
    }
}
