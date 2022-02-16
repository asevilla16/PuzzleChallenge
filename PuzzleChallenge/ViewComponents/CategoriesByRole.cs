using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PuzzleChallenge.Data;
using PuzzleChallenge.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuzzleChallenge.ViewComponents
{
    public class CategoriesByRole : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CategoriesByRole(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.Categories.ToListAsync();

            var roles = await _context.Categories.Select(x => x.RoleAuthorized)
                                                        .Distinct()
                                                        .ToListAsync();

            List<CategoriesByRoleDto> categoriesByRoles = new List<CategoriesByRoleDto>();

            roles.ForEach(rol =>
            {
                var categoriasPorRol = new CategoriesByRoleDto
                {
                    RoleId = rol,
                };

                categories.ForEach(category =>
                {
                    if(category.RoleAuthorized == rol)
                    {
                        categoriasPorRol.Categories.Add(category);
                    }
                });

                categoriesByRoles.Add(categoriasPorRol);
            });

            return View("Default", categoriesByRoles);
        }
    }
}
