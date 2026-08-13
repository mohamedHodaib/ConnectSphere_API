using ConnectSphere.Application.Common.Interfaces;
using ConnectSphere.Application.Common.Settings;
using ConnectSphere.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ConnectSphere.Infrastructure.Identity;

public class IdentitySeeder : IIdentitySeeder
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ILogger<IdentitySeeder> _logger;
    private readonly AdminInfo _adminInfo; 

    public IdentitySeeder(
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        ILogger<IdentitySeeder> logger,
        IOptions<AdminInfo> AdminInfoOptions)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _adminInfo = AdminInfoOptions.Value;
    }

    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedAdminUserAsync();
    }

    private async Task SeedRolesAsync()
    {
        var roles = new[] { "Admin", "User" };

        foreach (var roleName in roles)
        {
            //  Check if exists first
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var role = new IdentityRole<Guid>
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                };

                var result = await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));

                if (result.Succeeded)
                {
                    _logger.LogInformation("Role {RoleName} created successfully", roleName);
                }
                else
                {
                    var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));

                    _logger.LogCritical("Failed to create role {RoleName}: {Errors}",
                        roleName, errorMessage);

                    throw new Exception($"Critical Seeding Error: Unable to create role '{roleName}': {errorMessage}");
                }
            }
        }
    }

    private async Task SeedAdminUserAsync()
    {

        //  Check if admin exists
        var adminUser = await _userManager.FindByEmailAsync(_adminInfo.Email);

        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = _adminInfo.Email,
                Email = _adminInfo.Email,
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Administrator",
                JointDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(adminUser,_adminInfo.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                _logger.LogInformation("Admin user created successfully");
            }
            else
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));

                _logger.LogCritical("Failed to create admin user: {Errors}",errorMessage);

                throw new Exception($"Critical Seeding Error: Unable to create Admin User. {errorMessage}");
            }
        }
    }
}
