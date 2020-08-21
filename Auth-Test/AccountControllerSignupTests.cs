using Auth_API.Contracts;
using Auth_API.Models.Request;
using Auth_Data.Entities;
using Auth_API.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Xunit;

namespace Auth_Test
{
    public class AccountControllerSignupTests
    {
        IConfiguration _configuration;
        Mock<IIdentityAdapter> _identityAdapter = new Mock<IIdentityAdapter>();


        public AccountControllerSignupTests()
        {
            this._configuration = Configuration.InitConfiguration();
        }

        [Fact]
        public async Task AccountController_SignUp_CreatedResponse_RoleRegistered()
        {
            #region arrange                   

            //Endpoint PostModel 
            SignUpRequestModel model = new SignUpRequestModel
            {
                Email = "elsavies@hotmail.com",
                Fullname = "Ely Saul Vicente Espinal",
                Username = "Elsavies",
                Password = "Password.123",
                Key = "PasswordForAdminSignUp"
            };

            var accountController = new accountController(this._identityAdapter.Object, this._configuration);

            #endregion

            #region setup                                                                      

            this._identityAdapter.Setup(x => x.FindRoleByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationRole());
            this._identityAdapter.Setup(x => x.CreateUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            this._identityAdapter.Setup(x => x.GenerateJwtTokenAsync(It.IsAny<ApplicationUser>(), It.IsAny<string[]>())).ReturnsAsync(new JwtSecurityToken());
            this._identityAdapter.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>());

            #endregion

            #region action            

            var result = await accountController.SignUp(model);

            #endregion

            #region assert

            Assert.Equal(typeof(OkObjectResult), result.GetType());

            #endregion
        }

        [Fact]
        public async Task AccountController_SignUp_CreatedResponse_RoleNoRegistered()
        {
            #region arrange                   

            //Endpoint PostModel 
            SignUpRequestModel model = new SignUpRequestModel
            {
                Email = "elsavies@hotmail.com",
                Fullname = "Ely Saul Vicente Espinal",
                Username = "Elsavies",
                Password = "Password.123",
                Key = "PasswordForAdminSignUp"
            };

            var accountController = new accountController(this._identityAdapter.Object, this._configuration);

            #endregion

            #region setup                                                                      

            this._identityAdapter.Setup(x => x.CreateUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            this._identityAdapter.Setup(x => x.GenerateJwtTokenAsync(It.IsAny<ApplicationUser>(), It.IsAny<string[]>())).ReturnsAsync(new JwtSecurityToken());
            this._identityAdapter.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>());

            #endregion

            #region action            

            var result = await accountController.SignUp(model);

            #endregion

            #region assert

            Assert.Equal(typeof(OkObjectResult), result.GetType());

            #endregion
        }

        [Fact]
        public async Task AccountController_SignUp_BadRequestResponse_InvalidEmail()
        {
            #region arrange                   

            //Endpoint PostModel 
            SignUpRequestModel model = new SignUpRequestModel
            {
                Email = "elsavies@hotmail",
                Fullname = "Ely Saul Vicente Espinal",
                Username = "Elsavies",
                Password = "Password.123",
                Key = "PasswordForAdminSignUp"
            };

            var accountController = new accountController(this._identityAdapter.Object, this._configuration);

            #endregion

            #region setup                                                                      

            #endregion

            #region action            

            var result = await accountController.SignUp(model);

            #endregion

            #region assert

            Assert.Equal(typeof(BadRequestObjectResult), result.GetType());

            #endregion
        }

        [Fact]
        public async Task AccountController_SignUp_BadRequestResponse_InvalidUsername_LittleLength()
        {
            #region arrange                   

            //Endpoint PostModel 
            SignUpRequestModel model = new SignUpRequestModel
            {
                Email = "elsavies@hotmail.com",
                Fullname = "Ely Saul Vicente Espinal",
                Username = "Els",
                Password = "Password.123",
                Key = "PasswordForAdminSignUp"
            };

            var accountController = new accountController(this._identityAdapter.Object, this._configuration);

            #endregion

            #region setup                                                                      

            #endregion

            #region action            

            var result = await accountController.SignUp(model);

            #endregion

            #region assert

            Assert.Equal(typeof(BadRequestObjectResult), result.GetType());

            #endregion
        }

        [Fact]
        public async Task AccountController_SignUp_BadRequestResponse_InvalidUsername_LongLength()
        {
            #region arrange                   

            //Endpoint PostModel 
            SignUpRequestModel model = new SignUpRequestModel
            {
                Email = "elsavies@hotmail.com",
                Fullname = "Ely Saul Vicente Espinal",
                Username = "Elsaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                Password = "Password.123",
                Key = "PasswordForAdminSignUp"
            };

            var accountController = new accountController(this._identityAdapter.Object, this._configuration);

            #endregion

            #region setup                                                                      

            #endregion

            #region action            

            var result = await accountController.SignUp(model);

            #endregion

            #region assert

            Assert.Equal(typeof(BadRequestObjectResult), result.GetType());

            #endregion
        }

        [Fact]
        public async Task AccountController_SignUp_BadRequestResponse_InvalidUsername_InvalidChars()
        {
            #region arrange                   

            //Endpoint PostModel 
            SignUpRequestModel model = new SignUpRequestModel
            {
                Email = "elsavies@hotmail.com",
                Fullname = "Ely Saul Vicente Espinal",
                Username = "Elsavies!@#!",
                Password = "Password.123",
                Key = "PasswordForAdminSignUp"
            };

            var accountController = new accountController(this._identityAdapter.Object, this._configuration);

            #endregion

            #region setup                                                                      

            #endregion

            #region action            

            var result = await accountController.SignUp(model);

            #endregion

            #region assert

            Assert.Equal(typeof(BadRequestObjectResult), result.GetType());

            #endregion
        }

        [Fact]
        public async Task AccountController_SignUp_BadRequestResponse_InvalidFullname_InvalidChars()
        {
            #region arrange                   

            //Endpoint PostModel 
            SignUpRequestModel model = new SignUpRequestModel
            {
                Email = "elsavies@hotmail.com",
                Fullname = "Elyngeniero3020",
                Username = "Elsavies",
                Password = "Password.123",
                Key = "PasswordForAdminSignUp"
            };

            var accountController = new accountController(this._identityAdapter.Object, this._configuration);

            #endregion

            #region setup                                                                      

            #endregion

            #region action            

            var result = await accountController.SignUp(model);

            #endregion

            #region assert

            Assert.Equal(typeof(BadRequestObjectResult), result.GetType());

            #endregion
        }

        [Fact]
        public async Task AccountController_SignUp_BadRequestResponse_UserExists()
        {
            #region arrange                   

            //Endpoint PostModel 
            SignUpRequestModel model = new SignUpRequestModel
            {
                Email = "elsavies@hotmail.com",
                Fullname = "Elyngeniero Vicente",
                Username = "Elsavies",
                Password = "Password.123",
                Key = "PasswordForAdminSignUp"
            };

            var accountController = new accountController(this._identityAdapter.Object, this._configuration);

            #endregion

            #region setup                                                                      

            this._identityAdapter.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            this._identityAdapter.Setup(x => x.FindUserByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());

            #endregion

            #region action            

            var result = await accountController.SignUp(model);

            #endregion

            #region assert

            Assert.Equal(typeof(BadRequestObjectResult), result.GetType());

            #endregion
        }

        [Fact]
        public async Task AccountController_SignUp_BadRequestResponse_KeyNotFound()
        {
            #region arrange                   

            //Endpoint PostModel 
            SignUpRequestModel model = new SignUpRequestModel
            {
                Email = "elsavies@hotmail.com",
                Fullname = "Ely Saul Vicente Espinal",
                Username = "Elsavies",
                Password = "Password.123",
                Key = "abcdef"
            };

            var accountController = new accountController(this._identityAdapter.Object, this._configuration);

            #endregion

            #region setup                                                                                 

            #endregion

            #region action            

            var result = await accountController.SignUp(model);

            #endregion

            #region assert

            Assert.Equal(typeof(BadRequestObjectResult), result.GetType());

            #endregion
        }

        [Fact]
        public async Task AccountController_SignUp_BadRequestResponse_CreateFail()
        {
            #region arrange                   

            //Endpoint PostModel 
            SignUpRequestModel model = new SignUpRequestModel
            {
                Email = "elsavies@hotmail.com",
                Fullname = "Ely Saul Vicente Espinal",
                Username = "Elsavies",
                Password = "Password.123",
                Key = "PasswordForAdminSignUp"
            };

            var accountController = new accountController(this._identityAdapter.Object, this._configuration);

            #endregion

            #region setup                                                                                 

            this._identityAdapter.Setup(x => x.CreateUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

            #endregion

            #region action            

            var result = await accountController.SignUp(model);

            #endregion

            #region assert

            Assert.Equal(typeof(BadRequestObjectResult), result.GetType());

            #endregion
        }
    }
}
