﻿using System.ComponentModel.DataAnnotations;

namespace MAMS.API.DTOs
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
