using StackOverload.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using StackOverload.Models;

namespace StackOverload.Models
{
    public class SeedData
    {

        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
               serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            /////

            // Initial Tags
            if (!context.Tags.Any())
            {
                List<Tag> initialTags = new List<Tag>()
                {
                    new Tag { Name = "c#" },
                    new Tag { Name = "css" },
                    new Tag { Name = "html" },
                    new Tag { Name = "java-script" },
                    new Tag { Name = ".net" },
                    new Tag { Name = "game-development" },
                    new Tag { Name = "full-stack" },
                    new Tag { Name = "software" },
                    new Tag { Name = "hardware" },
                    new Tag { Name = "pets" },
                };
                context.Tags.AddRange(initialTags);
            }

            // Initial Roles
            if(!context.Roles.Any())
            {
                List<string> initialRoles = new List<string>()
                {
                    "Admin",
                    "Verified",
                    "Basic"
                };

                foreach(string role in initialRoles)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Initial Users
            if (!context.Users.Any())
            {
                var passwordHasher = new PasswordHasher<ApplicationUser>();


                // Admin User
                ApplicationUser adminUser = new ApplicationUser()
                {
                    Email = "admin@stackoverload.com",
                    NormalizedEmail = "ADMIN@STACKOVERLOAD.COM",
                    UserName = "admin@stackoverload.com",
                    NormalizedUserName = "ADMIN@STACKOVERLOAD.COM",
                    EmailConfirmed = true,
                };

                var hashPassword = passwordHasher.HashPassword(adminUser, "admin123_");
                adminUser.PasswordHash = hashPassword;
                await userManager.CreateAsync(adminUser);
                await userManager.AddToRoleAsync(adminUser, "Admin");


                // Verified User
                ApplicationUser verifiedUser = new ApplicationUser()
                {
                    Email = "verfied@gmail.com",
                    NormalizedEmail = "VERIFIED@GMAIL.COM",
                    UserName = "verfied@gmail.com",
                    NormalizedUserName = "VERIFIED@GMAIL.COM",
                    EmailConfirmed = true,
                };

                var hashPassword2 = passwordHasher.HashPassword(verifiedUser, "verfied123_");
                verifiedUser.PasswordHash = hashPassword;
                await userManager.CreateAsync(verifiedUser);
                await userManager.AddToRoleAsync(verifiedUser, "Verified");


                // Basic User
                ApplicationUser basicUser = new ApplicationUser()
                {
                    Email = "basic@gmail.com",
                    NormalizedEmail = "BASIC@GMAIL.COM",
                    UserName = "basic@gmail.com",
                    NormalizedUserName = "BASIC@GMAIL.COM",
                    EmailConfirmed = true,
                };

                var hashPassword3 = passwordHasher.HashPassword(basicUser, "basic123_");
                basicUser.PasswordHash = hashPassword2;
                await userManager.CreateAsync(basicUser);
                await userManager.AddToRoleAsync(basicUser, "Basic");

            }

            context.SaveChanges();
        }
    }
}
