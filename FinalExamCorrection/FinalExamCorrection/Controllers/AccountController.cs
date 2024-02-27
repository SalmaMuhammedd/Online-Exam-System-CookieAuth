using FinalExamCorrection.Models;
using FinalExamCorrection.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace FinalExamCorrection.Controllers
{

    public class AccountController : Controller
    {
        private readonly FinalOnlineExamSystemContext _dbContext;
        public AccountController(FinalOnlineExamSystemContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Home()
        {
            string role;
            if (User.Identity.IsAuthenticated)
            {

                role = User.IsInRole("Instructor") ? "Instructor" : "Student";
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                return RedirectToAction("Home", role, new {id = userId});
            }
            return RedirectToAction("Login");
        }
        public async Task<User> AuthenticateAsync(string email, string password)
        {
            // Find user by email
            var student = await _dbContext.Students.SingleOrDefaultAsync(u => u.Email == email);

            // Check if user exists and password is correct
            if (student != null && student.Password == password)
                return student;

            else
            {
                var instructor = await _dbContext.Instructors.SingleOrDefaultAsync(u => u.Email == email);
                if (instructor != null && instructor.Password == password)
                    // Authentication successful
                    return instructor;
            }

            // Authentication failed
            return null;
        }


        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            // Validate user credentials against the database
            var user = await AuthenticateAsync(model.Email, model.Password);
            Debug.WriteLine(user.Id);
            string role, controller;
            if (user != null)
            {
                if (user is Student)
                    role = controller = "Student";

                else
                    role = controller = "Instructor";


                // Create claims
                var claims = new List<Claim>
                {

                        new Claim(ClaimTypes.Name, user.Id),
                        new Claim(ClaimTypes.Role, role),

                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);
                // Sign in user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Home", controller);
            }

            // Handle invalid login
            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
