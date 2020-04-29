namespace QuizSystem.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using QuizSystem.Common;
    using QuizSystem.Data.Models;

    internal class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            await SeedUsersAsync(userManager, roleManager, GlobalConstants.AdministratorRoleName);
        }

        private static async Task SeedUsersAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));

                if (!result.Succeeded)
                {
                    throw new Exception(
                        string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            // TODO: REFACTOR THIS
            var user = await userManager.FindByNameAsync("StoyanShopov");
            var user2 = await userManager.FindByNameAsync("miroLLL");

            if (user != null)
            {
                await userManager.AddToRoleAsync(user, roleName);
            }

            if (user2 != null)
            {
                await userManager.AddToRoleAsync(user2, roleName);
            }
        }
    }
}
