using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PuzzleChallenge.Data;
using PuzzleChallenge.Dto;
using PuzzleChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuzzleChallenge.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.OrderBy(x => x.Email).Select(x => new UserModel
            {
                Id = x.Id,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber
            }).ToListAsync();

            return View(users);
        }
        
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User cannot be found";
                return View("NotFound");
            }

            var usuario = new EditUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber
            };

            foreach (var role in _roleManager.Roles)
            {
                var exists = await _userManager.IsInRoleAsync(user, role.Name);

                if (exists)
                {
                    var rol = new UserRoleDto
                    {
                        Id = role.Id,
                        Name = role.Name,
                        IsChecked = exists
                    };

                    usuario.Roles.Add(rol);
                }
            }


            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Details(EditUserDto request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Role cannot be found";
                return View("NotFound");
            }

            user.UserName = request.UserName;
            user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(request);
        }

        public async Task<IActionResult> EditRolesInUser(string userId)
        {
            ViewBag.userId = userId;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User cannot be found";
                return View("NotFound");
            }

            var roles = new List<UserRoleDto>();

            foreach (var rol in _roleManager.Roles)
            {
                var role = new UserRoleDto
                {
                    Id = rol.Id,
                    Name = rol.Name
                };

                var exists = await _userManager.IsInRoleAsync(user, rol.Name);

                role.IsChecked = exists;

                roles.Add(role);
            }

            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> EditRolesInUser(List<UserRoleDto> roles, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User cannot be found";
                return View("NotFound");
            }

            foreach (var rol in roles)
            {
                var dbRole = await _roleManager.FindByIdAsync(rol.Id);

                IdentityResult result = null;

                bool exists = await _userManager.IsInRoleAsync(user, rol.Name);

                if (!rol.IsChecked && !exists) continue;

                if (rol.IsChecked && !exists)
                {
                    result = await _userManager.AddToRoleAsync(user, rol.Name);
                }
                
                if (!rol.IsChecked && exists)
                {
                    result = await _userManager.RemoveFromRoleAsync(user, rol.Name);
                }
            }

            return RedirectToAction("Details", new { Id = userId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User cannot be found";
                return View("NotFound");
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("Index");
        }



    }
}
