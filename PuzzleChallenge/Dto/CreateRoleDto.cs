﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PuzzleChallenge.Dto
{
    public class CreateRoleDto
    {
        [Required]
        public string RoleName { get; set; }
    }
}
