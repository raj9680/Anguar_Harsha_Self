using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using MvcTaskManager.Identity;
using MvcTaskManager.ServiceContracts;
using MvcTaskManager.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userService;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _context;
        private readonly ApplicationSignInManager _applicationSignInManager;
        private readonly IAntiforgery antiforgery;
        public AccountController(IUserService userService, IAntiforgery _antiforgery, ApplicationSignInManager applicationSignInManager, ApplicationDbContext context, ApplicationUserManager userManager)
        {
            _userService = userService;
            antiforgery = _antiforgery;
            _applicationSignInManager = applicationSignInManager;
            _context = context;
            _userManager = userManager;
        }


        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginViewModel loginViewModel)
        {
            var user = await _userService.Authenticate(loginViewModel);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            HttpContext.User = await _applicationSignInManager.CreateUserPrincipalAsync(user);
            var tokens = antiforgery.GetAndStoreTokens(HttpContext);
            Response.Headers.Add("XSRF-REQUEST-TOKEN", tokens.RequestToken);
            Response.Headers.Add("Access-Control-Expose-Headers","XSRF-REQUEST-TOKEN");

            return Ok(user);
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] SignUpViewModel signUpViewModel)
        {
            var user = await _userService.Register(signUpViewModel);
            if (user == null)
                return BadRequest(new { message = "Invalid Data" });

            HttpContext.User = await _applicationSignInManager.CreateUserPrincipalAsync(user);
            var tokens = antiforgery.GetAndStoreTokens(HttpContext);
            Response.Headers.Add("XSRF-REQUEST-TOKEN", tokens.RequestToken);
            Response.Headers.Add("Access-Control-Expose-Headers", "XSRF-REQUEST-TOKEN");

            return Ok(user);
        }


        [HttpGet]
        [Route("api/getUserByEmail/{Email}")]
        public async Task<IActionResult> GetUserByEmail(string Email)
        {
            var user = await _userService.GetUserByEmail(Email);
            if (user == null)
                return BadRequest(new { message = "Invalid Data" });
            return Ok(user);
        }


        [Route("api/getallemployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            List<ApplicationUser> users = _context.Users.ToList();
            List<ApplicationUser> employeeUsers = new List<ApplicationUser>();

            foreach (var item in users)
            {
                //if((await _userManager.IsInRoleAsync(item, "Employee")))
                //{
                //    employeeUsers.Add(item);
                //}
                if (item.Email != "admin@admin.com")
                {
                    employeeUsers.Add(item);
                }
            }

            return Ok(employeeUsers);
        }
    }
}
