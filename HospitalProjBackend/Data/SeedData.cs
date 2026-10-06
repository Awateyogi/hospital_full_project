
//using HospitalProjBackend.Helpers;
//using HospitalProjBackend.Models;
//using Microsoft.AspNetCore.Identity;

//namespace HospitalProjBackend.Data;

//public static class SeedData
//{
//    public static async Task EnsureSeedDataAsync(IServiceProvider services)
//    {
//        using var scope = services.CreateScope();
//        var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

//        string[] roles = new[] { "Admin", "Staff" };
//        foreach (var r in roles)
//            if (!await roleMgr.RoleExistsAsync(r))
//                await roleMgr.CreateAsync(new IdentityRole(r));

//        var admin = await userMgr.FindByNameAsync("admin");
//        if (admin == null)
//        {
//            var u = new User { Username = "admin", PasswordHash = "Admin@123", Role = "Admin" };
//            var res = await userMgr.CreateAsync(u, "Admin@123");
//            if (res.Succeeded) await userMgr.AddToRoleAsync(u, "Admin");
//        }
//        var staff = new User
//        {
//            Username = "staff",
//            PasswordHash = PasswordHelper.Hash("Staff@123"),
//            Role = "Staff"
//        };
//        context.Users.Add(admin);
//        context.Users.Add(staff);

//        context.SaveChanges();
//    }
//}
using HospitalProjBackend.Helpers;
using HospitalProjBackend.Models;
using Microsoft.AspNetCore.Identity;
using HospitalProjBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalProjBackend.Data;

public static class SeedData
{
    public static void Seed(AppDbContext context)
    {
        context.Database.Migrate();

        if (!context.Users.Any())
        {
            var admin = new User
            {
                Username = "admin",
                //Email = "admin@example.com",
                PasswordHash = PasswordHelper.Hash("Admin@123"),
                Role = "Admin"
            };

            var staff = new User
            {
                Username = "staff",
               // Email = "staff@example.com",
                PasswordHash = PasswordHelper.Hash("Staff@123"),
                Role = "Staff"
            };

            context.Users.Add(admin);
            context.Users.Add(staff);

            context.SaveChanges();
        }
    }
}