﻿using OnlineShop.Models;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Data.Models.Identity
{
    public class Address : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } 
        public ApplicationUser ApplicationUser { get; set; }
    }
}