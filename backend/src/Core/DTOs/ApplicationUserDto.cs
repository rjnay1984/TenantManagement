﻿using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class ApplicationUserDto
    {
        public ApplicationUserDto()
        {
        }

        public ApplicationUserDto(ApplicationUser user, string role)
        {
            Id = user.Id;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Role = role;
        }

        public string Id { get; set; }

        [EmailAddress]
        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Role { get; set; }
    }
}