using HrManagementSystem.Data;
using HrManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HrManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        //private readonly HrDbContext _dbContext;
        //private readonly IConfiguration _configuration;

        public AccountController( UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            //_dbContext = dbContext;
           // _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        

        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {


                if (ModelState.IsValid)
                {

                    //var user = await _userManager.FindByNameAsync(model.UserName);
                    var isEmail = model.UserName.Contains("@");
                    User user = null;// _context.User.Where(x=>x.UserName==model.UserName).FirstOrDefault();

                    if (isEmail)
                    {
                        user = await _userManager.FindByEmailAsync(model.UserName.ToUpper());
                    }
                    else
                    {
                        user = await _userManager.FindByNameAsync(model.UserName);
                    }
                    if (user != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                        if (result.Succeeded)
                        {
                            var roles = await _userManager.GetRolesAsync(user);
                            //if (roles.Contains("Super Admin")) return RedirectToAction("Index", "SuperAdmin");
                            //if (roles.Contains("Manager")) return RedirectToAction("Index", "Manager");
                            //if (roles.Contains("Employee")) return RedirectToAction("Index", "Employee");
                            //if (roles.Contains("Client")) return RedirectToAction("Index", "Client");


                            if (await _userManager.IsInRoleAsync(user, "Super Admin"))
                                return RedirectToAction("Index", "SuperAdmin");

                            if (await _userManager.IsInRoleAsync(user, "Manager"))
                                return RedirectToAction("Index", "Manager");

                            if (await _userManager.IsInRoleAsync(user, "Employee"))
                                return RedirectToAction("Index", "Employee");

                            if (await _userManager.IsInRoleAsync(user, "Client"))
                                return RedirectToAction("Index", "Client");
                        }
                    }

                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> SeedRoles()
        {
            string[] roles = { "Super Admin", "Manager", "Employee", "Client" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new Role { Name = role });
                }
            }
            return Content("Roles Seeded Successfully");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password, string role)
        {
            var user = new User { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
                return RedirectToAction("Login");
            }
            ModelState.AddModelError("", "Registration failed.");
            return View();
        }
        public async Task<IActionResult> UserList()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("UserList");
        }
    }
}
