using PuzzleChallenge.Data;
using PuzzleChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuzzleChallenge.AppService
{
    public class CategoriesAppService
    {
        private readonly ApplicationDbContext _context;

        public CategoriesAppService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<bool> AddCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
