using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SomaShare.Models;

namespace SomaShare.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Apply migrations and wait until complete
            await context.Database.MigrateAsync();

            // Create roles if they don't exist
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await roleManager.RoleExistsAsync("Student"))
                await roleManager.CreateAsync(new IdentityRole("Student"));

            // Seed admin user
            var adminEmail = "admin@somaafrika.edu";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Platform Admin",
                    Institution = "Soma Afrika",
                    Course = "Administration"
                };
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // Seed a sample student
            var studentEmail = "student@uni.ac.za";
            var student = await userManager.FindByEmailAsync(studentEmail);
            if (student == null)
            {
                student = new ApplicationUser
                {
                    UserName = studentEmail,
                    Email = studentEmail,
                    FullName = "Thabo Molefe",
                    Institution = "University of Johannesburg",
                    Course = "Computer Science"
                };
                await userManager.CreateAsync(student, "Student@123");
                await userManager.AddToRoleAsync(student, "Student");
            }

            // Seed textbooks if empty
            if (!context.Textbooks.Any())
            {
                context.Textbooks.AddRange(
                    new Textbook
                    {
                        Title = "Introduction to Algorithms",
                        Author = "Cormen",
                        ISBN = "978-0-262-03384-8",
                        Condition = "Good",
                        Price = 450,
                        Campus = "APK",
                        UserId = student.Id
                    },
                    new Textbook
                    {
                        Title = "Database Systems",
                        Author = "Elmasri",
                        ISBN = "978-0-13-397077-7",
                        Condition = "Like New",
                        Price = 600,
                        Campus = "APK",
                        UserId = student.Id
                    }
                );
            }

            // Seed a forum thread if empty
            if (!context.ForumThreads.Any())
            {
                context.ForumThreads.Add(new ForumThread
                {
                    Title = "Welcome to SomaShare Forum",
                    Content = "Use this forum to discuss textbooks, study tips, and connect with classmates.",
                    UserId = student.Id
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
