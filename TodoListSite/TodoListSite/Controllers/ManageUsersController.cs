using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TodoListSite.Models;
using Microsoft.EntityFrameworkCore;

namespace TodoListSite.Controllers
{
    [Authorize(Roles = Constants.AdministratorRole)]
    //[Authorize]
    public class ManageUsersController : Controller
    {

        private readonly UserManager<IdentityUser> _UserManager;

        public ManageUsersController(UserManager<IdentityUser> userManager)
        {
            _UserManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var admins = (await _UserManager
                .GetUsersInRoleAsync( Constants.AdministratorRole))
                .ToArray();

            var everyone = await _UserManager.Users
                .ToArrayAsync();

            var model = new ManageUsersViewModel
            {
                Administrators = admins,
                Everyone = everyone
            };

            return View(model);
        }
    }
}