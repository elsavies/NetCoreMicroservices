using Auth_API.Contracts;
using Auth_API.Helpers;
using Auth_Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth_API.Adapters
{
    public class IdentityAdapter : IIdentityAdapter
    {
        private readonly AppSettings _appSettings;
        
        public UserManager<ApplicationUser> _userManager { get; set; }
        public RoleManager<ApplicationRole> _roleManager { get; set; }
        public SignInManager<ApplicationUser> _signInManager { get; set; }

        public IdentityAdapter(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager, IOptions<AppSettings> appSettings)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._signInManager = signInManager;            
            this._appSettings = appSettings.Value;
        }

        #region UserManager

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await this._userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser> FindUserByNameAsync(string name)
        {
            return await this._userManager.FindByNameAsync(name);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            return await this._userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return await this._userManager.AddToRoleAsync(user, role);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await this._userManager.GetRolesAsync(user);
        }     
        #endregion

        #region RoleManager

        public async Task<ApplicationRole> FindRoleByNameAsync(string name)
        {
            return await this._roleManager.FindByNameAsync(name);
        }

        public Task<IdentityResult> CreateRoleAsync(ApplicationRole role)
        {
            return this._roleManager.CreateAsync(role);
        }

        #endregion

        public async Task<JwtSecurityToken> GenerateJwtTokenAsync(ApplicationUser user, string[] roles)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(System.Convert.ToBase64String(Encoding.UTF8.GetBytes(_appSettings.JwtParameters.JwtKey)));
            var issuers = this._appSettings.JwtParameters.JwtIssuers.ToArray();
            var audiences = this._appSettings.JwtParameters.JwtAudiences.ToArray();

            var audienceClaims = new List<Claim>();
            var roleClaims = new List<Claim>();

            for (int i = 0; i <= audiences.Count() - 1; i++)
            {
                audienceClaims.Add(new Claim("aud", audiences[i]));
            }

            for (int i = 0; i <= roles.Count() - 1; i++)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, roles[i]));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuers[0],
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Fullname),
                    new Claim("username", user.UserName)
                }),
                Expires = DateTime.UtcNow.AddHours(_appSettings.JwtParameters.JwtExpiryInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
            };

            tokenDescriptor.Subject.AddClaims(audienceClaims);
            tokenDescriptor.Subject.AddClaims(roleClaims);

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return token;
        }
    }
}
