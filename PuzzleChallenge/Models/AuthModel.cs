using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PuzzleChallenge.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }

    public class UserModel
    {
        public UserModel()
        {
            Roles = new List<Role>();
        }
        public string Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        
        [Phone]
        public string PhoneNumber { get; set; }
        public List<Role> Roles { get; set; }

    }
}
