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
            try
            {
                // Apply migrations safely - this is idempotent
                // EF Core tracks which migrations have been applied, so this is safe to run multiple times
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                // Log migration errors but continue with seeding
                // This prevents migration conflicts from blocking the app startup
                System.Diagnostics.Debug.WriteLine($"Migration warning: {ex.Message}");
            }

            try
            {
                // Create roles if they don't exist
                if (!await roleManager.RoleExistsAsync("Admin"))
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                if (!await roleManager.RoleExistsAsync("Student"))
                    await roleManager.CreateAsync(new IdentityRole("Student"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Role creation warning: {ex.Message}");
            }

            try
            {
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
                    var result = await userManager.CreateAsync(admin, "Admin@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Admin user seeding warning: {ex.Message}");
            }

            try
            {
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
                    var result = await userManager.CreateAsync(student, "Student@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(student, "Student");
                        await SeedSampleData(context, student.Id);
                    }
                }
                else
                {
                    // Student exists, seed sample data if empty
                    await SeedSampleData(context, student.Id);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Student user seeding warning: {ex.Message}");
            }
        }

        private static async Task SeedSampleData(ApplicationDbContext context, string studentId)
        {
            try
            {
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
                            UserId = studentId
                        },
                        new Textbook
                        {
                            Title = "Database Systems",
                            Author = "Elmasri",
                            ISBN = "978-0-13-397077-7",
                            Condition = "Like New",
                            Price = 600,
                            Campus = "APK",
                            UserId = studentId
                        }
                    );
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Textbook seeding warning: {ex.Message}");
            }

            try
            {
                // Seed a forum thread if empty
                if (!context.ForumThreads.Any())
                {
                    context.ForumThreads.Add(new ForumThread
                    {
                        Title = "Welcome to SomaShare Forum",
                        Content = "Use this forum to discuss textbooks, study tips, and connect with classmates.",
                        UserId = studentId
                    });
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Forum thread seeding warning: {ex.Message}");
            }
        }
    }
}
