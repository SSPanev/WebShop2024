﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using WebShop2024.Core.Contracts;
using WebShop2024.Infrastructure.Data.Entities;
using WebShop2024.Models.Client;

namespace WebShop2024.Controllers
{
    
    public class ClientController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderService _orderService;

        public ClientController(UserManager<ApplicationUser> userManager, IOrderService orderService)
        {
            _userManager = userManager;
            _orderService = orderService;
        }
        // GET: ClientController
        public async Task<IActionResult> Index()
        {
            var allUsers = _userManager.Users
                .Select(u => new ClientIndexVM()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Address = u.Address,
                    Email = u.Email
                }).ToList();

            var adminIds = (await _userManager.GetUsersInRoleAsync("Administrator"))
                .Select(a => a.Id).ToList();

            foreach (var user in allUsers)
            {
                user.IsAdmin = adminIds.Contains(user.Id);
            }

            var users = allUsers.Where(x => !x.IsAdmin)
                .OrderBy(x => x.UserName).ToList();

            return View(users);
        }

        // GET: ClientController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClientController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClientController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClientController/Delete/5
        public ActionResult Delete(string id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound();

            ClientDeleteVM userToDelete = new ClientDeleteVM()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Email = user.Email
            };
            return View(userToDelete);
        }

        // POST: ClientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ClientDeleteVM bindingModel)
        {
            string id = bindingModel.Id;
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            if (_orderService.GetOrdersByUser(id) != default(List<Order>))
                return RedirectToAction("Error", new {reason = "Orders"});

            IdentityResult result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return RedirectToAction("Success");

            return NotFound();

        }

        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Error(string? reason)
        {
            string errorMessage;

            switch (reason?.ToLower())
            {
                default:
                    // DON'T EVER USE THIS!!!!! It is here just as a failsafe!!!
                    errorMessage = "Unspecified error!";
                    break;
                case "orders":
                    errorMessage = "The user has orders!";
                    break;
            }

            ClientErrorVM error = new ClientErrorVM()
            {
                ErrorMessage = errorMessage
            };
            return View(error);
        }
    }
}
