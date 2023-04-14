using IntexFagElGamous.Models.ViewModels;
using IntexFagElGamous.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IntexFagElGamous.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        //DELETING
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index");
                //return RedirectToAction("Index", "Home");

            }

            return RedirectToAction("Users", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }

            var model = new EditRoleViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(string roleId, string roleName)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }

            role.Name = roleName;

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return RedirectToAction("Roles");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return RedirectToAction("Roles");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUserFromRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();
            var roles = allRoles.Where(r => userRoles.Contains(r.Name)).ToList();
            ViewBag.roles = roles;

            //var user = await _userManager.FindByIdAsync(userId);

            var model = new DeleteUserFromRoleViewModel
            {
                UserId = userId,
            };
            return View(model);
        }

        //DELETING USER ROLES
        [HttpPost]
        public async Task<IActionResult> DeleteUserFromRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return RedirectToAction("Users");
        }

        [HttpGet]
        public async Task<IActionResult> AddUserToRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();
            var roles = allRoles.Where(r => !userRoles.Contains(r.Name)).ToList();
            ViewBag.roles = roles;
            //var user = await _userManager.FindByIdAsync(userId);

            var model = new AddUserToRoleViewModel
            {
                UserId = userId,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return RedirectToAction("Users");
        }

        //UDATE USER
        //[HttpGet("api/[action]")]
        [HttpGet]
        //public async Task<IActionResult> UpdateUser(string userId, string email)
        public async Task<IActionResult> UpdateUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            //user.Email = email;

            //var result = await _userManager.UpdateAsync(user);
            //if (!result.Succeeded)
            //{
            //    return BadRequest(result.Errors);
            //}

            //return RedirectToAction("Index", "Admin");

            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Roles = userRoles
            };

            return View(model);
        }

        [HttpPost]
        [Route("Admin/UpdateUser")]
        public async Task<IActionResult> UpdateUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return NotFound();
            }
            else
            {
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }

                return View(model);
            }
        }

        //CREATE ROLE
        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (result.Succeeded)
                {
                    return RedirectToAction("Roles", "Admin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(roleName);
        }

        //PAGES
        public async Task<IActionResult> Roles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        //public async Task<IActionResult> Users()
        //{
        //    var users = await _userManager.Users.ToListAsync();
        //    return View(users);
        //}

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var model = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var viewModel = new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = userRoles.FirstOrDefault() ?? "No role assigned"
                };
                model.Add(viewModel);
            }

            // Get the list of available roles for each user
            var rolesForUsers = new Dictionary<string, List<string>>();
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var availableRoles = _roleManager.Roles.Where(r => !userRoles.Contains(r.Name)).Select(r => r.Name).ToList();
                rolesForUsers.Add(user.Id, availableRoles);
            }

            ViewBag.RolesForUsers = rolesForUsers;
            return View(model);
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
