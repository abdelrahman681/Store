using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.CoreLayer.Entirty;
using System.Security.Claims;

namespace Store.Helpers
{
    public static class UserMangerExtentions
    {
        public static async Task<AppUser?> FindUserWithAddressByEmailAsync(this UserManager<AppUser> userManager, ClaimsPrincipal claims)
        {
            var email=claims.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

    }
}
