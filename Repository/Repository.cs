using HrManagementSystem.Data;
using HrManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HrManagementSystem
{
    public class Repository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<Role> roleManager;
        private readonly HrDbContext _context;
        public Repository(HrDbContext hrDbContext,UserManager<User> userManager)
        {
            _userManager = userManager;
            signInManager = signInManager;
            roleManager = roleManager;

            _context = hrDbContext;
        }
        public string GenerateAlphaNumericPwd()
        {
            string numbers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz!@#$%^&*()-=";
            Random objrandom = new Random();
            string passwordString = "";
            string strrandom = string.Empty;
            for (int i = 0; i < 8; i++)
            {
                int temp = objrandom.Next(0, numbers.Length);
                passwordString = numbers.ToCharArray()[temp].ToString();
                strrandom += passwordString;
            }
            return strrandom;
        }

    }
}
