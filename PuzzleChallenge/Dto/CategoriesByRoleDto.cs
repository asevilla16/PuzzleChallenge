using PuzzleChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuzzleChallenge.Dto
{
    public class CategoriesByRoleDto
    {
        public CategoriesByRoleDto()
        {
            Categories = new List<Category>();
        }
        public string RoleId { get; set; }
        public List<Category> Categories { get; set; }
    }
}
