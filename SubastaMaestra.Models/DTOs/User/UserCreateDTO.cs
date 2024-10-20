﻿using SubastaMaestra.Entities.Core;
using SubastaMaestra.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Models.DTOs.User
{
    public class UserCreateDTO
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public DocumType DocumentType { get; set; }
        [Required]
        public string DocumentNumber { get; set; }
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public PersonType? PersonType { get; set; } // Juridica o Fisica
    }
}
