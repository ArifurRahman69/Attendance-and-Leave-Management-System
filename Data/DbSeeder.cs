using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Attendance_and_Leave_Management_System.DataModel;

namespace Attendance_and_Leave_Management_System.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            // Create a new scope to retrieve scoped services
            using var scope = service.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // 1. Seed Roles
            string[] roles = { "Admin", "Manager", "Employee" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. Seed Admin User
            const string adminEmail = "admin@admin.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "System",
                    LastName = "Administrator",
                    CreatedAt = DateTime.UtcNow
                };

                var createResult = await userManager.CreateAsync(adminUser, "Admin@12345");
                if (!createResult.Succeeded)
                {
                    // Log errors here if necessary
                    return;
                }
            }

            // Assign Admin Role
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // 3. Seed Departments
            if (!context.Departments.Any())
            {
                context.Departments.AddRange(
                    new Department { Name = "Human Resources" },
                    new Department { Name = "IT Development" },
                    new Department { Name = "Finance" },
                    new Department { Name = "Sales & Marketing" },
                    new Department { Name = "Operations" }
                );
                await context.SaveChangesAsync();
            }

            // 4. Seed Designations
            if (!context.Designations.Any())
            {
                context.Designations.AddRange(
                    new Designation { Name = "General Manager" },
                    new Designation { Name = "Project Manager" },
                    new Designation { Name = "Senior Developer" },
                    new Designation { Name = "Junior Developer" },
                    new Designation { Name = "QA Engineer" },
                    new Designation { Name = "HR Executive" },
                    new Designation { Name = "Accountant" },
                    new Designation { Name = "Intern" }
                );
                await context.SaveChangesAsync();
            }

            // 5. Seed Shifts
            if (!context.Shifts.Any())
            {
                context.Shifts.AddRange(
                    new Shift { Name = "Morning", StartTime = "08:00", EndTime = "16:00", WorkHour = "8" },
                    new Shift { Name = "Evening", StartTime = "14:00", EndTime = "22:00", WorkHour = "8" },
                    new Shift { Name = "Night", StartTime = "22:00", EndTime = "06:00", WorkHour = "8" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}