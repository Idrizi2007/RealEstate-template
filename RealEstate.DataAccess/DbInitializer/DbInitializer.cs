using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RealEstate.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        public  async Task InitializeAsync()
        {
            if ((await _db.Database.GetPendingMigrationsAsync()).Any())
            {
                await _db.Database.MigrateAsync();
            }
            if (!await _roleManager.RoleExistsAsync(SD.AdminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.AdminRole));
            }
            string email = _configuration["AdminUser:Email"];
            string password = _configuration["AdminUser:Password"];

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Admin user email or password is not configured.");
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

            }
            if (!await _userManager.IsInRoleAsync(user, SD.AdminRole))
            {
                await _userManager.AddToRoleAsync(user, SD.AdminRole);
            }
        }
    }
}
