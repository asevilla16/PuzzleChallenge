using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuzzleChallenge.Dto
{
    public class EditUserDto
    {
        public EditUserDto()
        {
            Roles = new List<UserRoleDto>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public List<UserRoleDto> Roles { get; set; }
    }

    public class UserRoleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }
}
