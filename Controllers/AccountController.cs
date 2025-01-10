using System.Security.Claims;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult AdminLogin()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult StudentLogin()
        {
            return View();
        }


        // Authenticate API 
        [HttpPost]
        [Route("api/authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            Console.WriteLine($"API called with Username: {request.Username}");


            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);


            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var claimsIdentity = new ClaimsIdentity(claims, "LibraryAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("LibraryAuth", claimsPrincipal);



            return Ok(new
            {
                Username = user.Username,
                Role = user.Role,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(LoginRequest request)
        {
            if (ModelState.IsValid)
            {

                if (_context.Users.Any(u => u.Username == request.Username))
                {
                    TempData["ErrorMessage"] = "This username is already taken. Please choose a different one.";
                    return View(request);
                }


                var newUser = new User
                {
                    Username = request.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Role = "Student"
                };


                _context.Users.Add(newUser);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Registration successful";
                return RedirectToAction("Register", "Account");
            }

            return View(request);
        }

       


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("LibraryAuth");
            return RedirectToAction("StudentLogin", "Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
