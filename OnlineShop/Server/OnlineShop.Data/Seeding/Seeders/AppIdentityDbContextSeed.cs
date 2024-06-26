﻿using Microsoft.AspNetCore.Identity;
using OnlineShop.Common;
using OnlineShop.Data.Models.Identity;

namespace OnlineShop.Data.Seeding.Seeders
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser
                {
                    DisplayName = "Bob",
                    Email = "bob@test.com",
                    UserName = "bob@test.com",
                    Address = new Address
                    {
                        FirstName = "Bob",
                        LastName = "Bobbity",
                        Street = "10 The street",
                        City = "New York",
                        State = "NY",
                        ZipCode = "90210"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRolesAsync(user, [Roles.Admin]);

            }
        }
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {

            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }
            if (!await roleManager.RoleExistsAsync(Roles.User))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.User));
            }
        }
        
    }
}
