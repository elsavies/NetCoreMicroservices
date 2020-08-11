using Auth_Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Auth_API.Contracts
{
    public interface IIdentityAdapter
    {
        SignInManager<ApplicationUser> _signInManager { get; set; }       
        Task<JwtSecurityToken> GenerateJwtTokenAsync(ApplicationUser user, string[] roles);

        #region UserManager

        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ApplicationUser> FindUserByNameAsync(string name);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);               

        #endregion

        #region RoleManager

        Task<ApplicationRole> FindRoleByNameAsync(string name);
        Task<IdentityResult> CreateRoleAsync(ApplicationRole role);

        #endregion
    }
}
