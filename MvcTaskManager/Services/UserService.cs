using Microsoft.Extensions.Options;
using MvcTaskManager.Identity;
using MvcTaskManager.JWT;
using MvcTaskManager.ServiceContracts;
using MvcTaskManager.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MvcTaskManager.Models;

namespace MvcTaskManager
{
    public class UserService : IUserService
    {
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly ApplicationSignInManager _applicationSingnInManager;
        private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationUserManager applicationUserManager, ApplicationSignInManager applicationSingnInManager, IOptions<AppSettings> appSettings, ApplicationDbContext context)
        {
            _applicationUserManager = applicationUserManager;
            _applicationSingnInManager = applicationSingnInManager;
            _appSettings = appSettings.Value; // fetching appsettings value i.e AppSettings, secret
            _context = context;
        }

        public async Task<ApplicationUser> Authenticate(LoginViewModel loginViewModel)
        {
            var result = await _applicationSingnInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);
            if(result.Succeeded)
            {
                var applicationUser = await _applicationUserManager.FindByNameAsync(loginViewModel.Username);
                applicationUser.PasswordHash = null;

                //if (await this._applicationUserManager.IsInRoleAsync(applicationUser, "Admin")) applicationUser.Role = "admin";
                //else if (await this._applicationUserManager.IsInRoleAsync(applicationUser, "Employee")) applicationUser.Role = "employee";

                //applicationUser.Role = "admin";
                applicationUser.Role = "employee";

                #region For JWT Generate - Step 2
                var tokenHandler = new JwtSecurityTokenHandler();

              //var key = System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret); // secretKey to byte array

                var key = System.Text.Encoding.ASCII.GetBytes("ThisIsTheMostSecret");
                // Specifiying payload of jwt token
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    // object of securoty token descriptor
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                        // payload
                        new Claim(ClaimTypes.Name, applicationUser.Id),
                        new Claim(ClaimTypes.Email, applicationUser.Email),
                        new Claim(ClaimTypes.Role, applicationUser.Role)
                    }),
                    // specifying expiry time
                    Expires = DateTime.UtcNow.AddHours(8),
                    // specifying algorothm to use
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                applicationUser.Token = tokenHandler.WriteToken(token);
                #endregion

                return applicationUser;
            }
            else
            {
                return null;
            }
        }

        
        // Register
        public async Task<ApplicationUser> Register(SignUpViewModel signUpViewModel)
        {
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.FirstName = signUpViewModel.PersonName.FirstName;
            applicationUser.LastName = signUpViewModel.PersonName.LastName;
            applicationUser.Email = signUpViewModel.Email;
            applicationUser.PhoneNumber = signUpViewModel.Mobile;
            applicationUser.ReceiveNewsLetters = signUpViewModel.ReceiveNewsLetters;
            applicationUser.CountryID = signUpViewModel.CountryID;
            applicationUser.Role = "employee";
            applicationUser.UserName = signUpViewModel.Email;

            var result = await _applicationUserManager.CreateAsync(applicationUser, signUpViewModel.Password);
            if(result.Succeeded)
            {
                var result2 = await _applicationSingnInManager.PasswordSignInAsync(signUpViewModel.Email, signUpViewModel.Password, false, false);
                if(result2.Succeeded)
                {
                    applicationUser.PasswordHash = null;

                    //token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = System.Text.Encoding.ASCII.GetBytes("ThisIsTheMostSecret");
                    var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, applicationUser.Id),
                            new Claim(ClaimTypes.Email, applicationUser.Email),
                            new Claim(ClaimTypes.Role, applicationUser.Role)
                        }),
                        Expires = DateTime.UtcNow.AddHours(8),
                        SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    applicationUser.Token = tokenHandler.WriteToken(token);

                    // Inserting Skills
                    foreach (var sk in signUpViewModel.Skills)
                    {
                        Skill skill = new Skill();
                        skill.SkillName = sk.SkillName;
                        skill.Level = sk.Level;
                        skill.Id = applicationUser.Id;
                        skill.ApplicationUser = null;

                        _context.Skills.Add(skill);
                        _context.SaveChanges();
                    }

                    return applicationUser;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        public async Task<ApplicationUser> GetUserByEmail(string Email)
        {
            return await _applicationUserManager.FindByEmailAsync(Email);
        }
    }
}
