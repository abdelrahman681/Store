using Microsoft.AspNetCore.Identity;
using Store.CoreLayer.Entirty;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.IService
{
    public interface ITokenService
    {
       Task<string> GenerateTokenAsync(AppUser user,UserManager<AppUser> userManager);
       string GenerateRefreshToken();
    }
}
