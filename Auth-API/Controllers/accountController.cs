using Auth_Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Auth_API.Models.Request;
using Auth_API.Contracts;
using Auth_API.Models.Results;

namespace Auth_API.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class accountController : ControllerBase
    {
        private readonly IIdentityAdapter _identityAdapter;
        private readonly IConfiguration _configuration;

        #region RegexPatterns
        private readonly string _invalidRequestMessage = "The request is invalid!";
        private readonly string _emailPattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
        private readonly string _usernamePattern = "^[a-zA-Z0-9]{4,32}$";
        private readonly string _fullnamePattern = @"^[\p{L}\p{M}' \.\-]+$";

        #endregion

        public accountController(IIdentityAdapter identityAdapter, IConfiguration configuration)
        {
            this._identityAdapter = identityAdapter;
            this._configuration = configuration;
        }

        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseModel(this._invalidRequestMessage));

            if (!Regex.IsMatch(model.Email, this._emailPattern))
                return BadRequest(new ApiResponseModel(this._invalidRequestMessage) { Data = "Invalid Email" });

            if (!Regex.IsMatch(model.Username, this._usernamePattern))
                return BadRequest(new ApiResponseModel(this._invalidRequestMessage) { Data = "Invalid UserName(at least 4 chars and 32 chars maximun, alphanumeric only)" });

            if (!Regex.IsMatch(model.Fullname, this._fullnamePattern))
                return BadRequest(new ApiResponseModel(this._invalidRequestMessage) { Data = "Invalid FullName(alphabetic only)" });

            bool doesUserExist = await this._identityAdapter.FindByEmailAsync(model.Email) != null
                                 || await this._identityAdapter.FindUserByNameAsync(model.Username) != null;

            if (doesUserExist)
                return BadRequest(new ApiResponseModel(this._invalidRequestMessage) { Data = "Invalid User!" });

            var currentDate = DateTime.Now.ToUniversalTime();

            var newUser = new ApplicationUser()
            {
                Email = model.Email.ToLower(),
                UserName = model.Username,
                Fullname = model.Fullname,
                ProfilePicture = model.ProfilePicture,
                ProfileThumbnail = model.ProfileThumbnail,
                CreatedDate = currentDate,
                UpdateDate = currentDate,
                Version = Guid.NewGuid().ToString(),
                Active = true
            };

            //Getting Roles Keys
            var keys = this?._configuration?.GetSection("SignUpKeys")?.GetChildren()?.Select(x => x.Key);

            bool roleFound = false;
            var roleName = string.Empty;

            foreach (var k in keys)
            {
                var keyValue = this._configuration["SignUpKeys:" + k];

                //Verifying SignUp Key against Role Keys
                if (model.Key.Equals(keyValue))
                {
                    var roleExist = (await this._identityAdapter.FindRoleByNameAsync(k)) != null;

                    //Create the Role if doesn't exist 
                    if (!roleExist)
                    {
                        var newRole = new ApplicationRole();
                        newRole.Name = k;

                        await this._identityAdapter.CreateRoleAsync(newRole);

                        roleExist = true;
                    }

                    //Assigning Role Key to the User
                    if (roleExist)
                    {
                        roleName = k;
                        roleFound = true;

                        break;
                    }
                }
            }

            if (!roleFound)
                return BadRequest(new ApiResponseModel(this._invalidRequestMessage) { Data = "Signup Error!" });

            var result = await _identityAdapter.CreateUserAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                var data = "";

                foreach (var e in errors)
                {
                    data += e;
                }

                return BadRequest(new ApiResponseModel<string>("SignUp error, try again!") { Data = data });
            }

            //Redirecting results
            if (result.Succeeded)
            {
                if (roleFound && !roleName.Equals(string.Empty))
                    await this._identityAdapter.AddToRoleAsync(newUser, roleName);

                var roles = await this._identityAdapter.GetRolesAsync(newUser);

                var token = await this._identityAdapter.GenerateJwtTokenAsync(newUser, roles.ToArray());

                return Ok(new ApiResponseModel(newUser.Email) { Data = token.RawData });
            }
            else
                return BadRequest(new ApiResponseModel(this._invalidRequestMessage) { Data = "SignUp error, try again!" });
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LogInRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseModel(this._invalidRequestMessage) { Data = this._invalidRequestMessage });

            string username = model.UserName;

            ApplicationUser loggedUser = null;

            if (Regex.IsMatch(model.UserName, this._emailPattern))
            {
                var tempUser = await this._identityAdapter.FindByEmailAsync(model.UserName);
                if (tempUser != null)
                    loggedUser = tempUser;
                else
                    return BadRequest(new ApiResponseModel(this._invalidRequestMessage) { Data = this._invalidRequestMessage });
            }

            loggedUser = loggedUser ?? await this._identityAdapter.FindUserByNameAsync(username);

            if (loggedUser == null)
                return BadRequest(new ApiResponseModel(this._invalidRequestMessage) { Data = this._invalidRequestMessage });
            else
            {
                var result = await this._identityAdapter._signInManager.PasswordSignInAsync(loggedUser, model.Password, true, true);

                if (result == Microsoft.AspNetCore.Identity.SignInResult.Success)
                {
                    var roles = await this._identityAdapter.GetRolesAsync(loggedUser);

                    var token = await this._identityAdapter.GenerateJwtTokenAsync(loggedUser, roles.ToArray());

                    return Ok(new ApiResponseModel<string>(loggedUser.NormalizedEmail.ToLower()) { Data = token.RawData });
                }
                if (result == Microsoft.AspNetCore.Identity.SignInResult.Failed)
                    return BadRequest(new ApiResponseModel(this._invalidRequestMessage) { Data = "Invalid Username or Password" });
                if (result == Microsoft.AspNetCore.Identity.SignInResult.LockedOut)
                    return BadRequest(new ApiResponseModel(this._invalidRequestMessage) { Data = "User is locked out" });
                if (result == Microsoft.AspNetCore.Identity.SignInResult.TwoFactorRequired)
                    return Accepted(new ApiResponseModel(this._invalidRequestMessage) { Data = "Requires verification" });
            }

            return BadRequest(new ApiResponseModel(this._invalidRequestMessage) { Data = "Invalid Username or Password" });
        }
        
    }
}
