using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager; // ✅ Use custom User class
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            // Apply pending migrations
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {
                // You can log this if needed
            }

            // Skip if roles already exist (assumes already initialized)
            if (_roleManager.Roles.Any())
            {
                return;
            }

            // Create roles
            _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.EmployeeRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();

            // Create default admin user using your custom User class
            _userManager.CreateAsync(new User
            {
                UserName = "rfry@weber.edu",
                Email = "rfry@weber.edu",
                FirstName = "Richard",
                LastName = "Fry",
                PhoneNumber = "8015556919",
            }, "Admin123*").GetAwaiter().GetResult();

            User user = _db.User.FirstOrDefault(u => u.Email == "rfry@weber.edu");

            _userManager.AddToRoleAsync(user, SD.AdminRole).GetAwaiter().GetResult();

        }
    }
}
