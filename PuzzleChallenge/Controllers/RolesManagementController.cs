using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PuzzleChallenge.Data;
using PuzzleChallenge.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuzzleChallenge.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesManagementController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        

        public RolesManagementController(
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager
            )
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleDto rol)
        {
            if (ModelState.IsValid)
            {
                IdentityRole newRole = new IdentityRole
                {
                    Name = rol.RoleName
                };

                var result = await _roleManager.CreateAsync(newRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "RolesManagement");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(rol);

        }

        
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles.OrderBy(x => x.Name).ToList();
            return View(roles);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var existingRole = await _roleManager.FindByIdAsync(id);

            if(existingRole == null)
            {
                ViewBag.ErrorMessage = $"Role cannot be found";
                return View("NotFound");

            }

            var role = new EditRoleDto
            {
                Id = existingRole.Id,
                RoleName = existingRole.Name
            };

            foreach(var user in _userManager.Users)
            {
                var exists = await _userManager.IsInRoleAsync(user, existingRole.Name);

                if (exists)
                {
                    role.Users.Add(user.UserName);
                }
            }

            return View(role);
        
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleDto request)
        {
            var role = await _roleManager.FindByIdAsync(request.Id);

            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role cannot be found";
                return View("NotFound");
            }

            role.Name = request.RoleName;

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role cannot be found";
                return View("NotFound");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("ListRoles");
        }



    }
}
