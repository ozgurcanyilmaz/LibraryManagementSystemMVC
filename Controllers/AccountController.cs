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
        public IActionResult Logout()
        {
            // Kullanıcı oturumunu kapatma işlemleri
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        // Admin Login GET
        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View(); // Varsayılan olarak Views/Account/AdminLogin.cshtml aranır
        }

        // Admin Login POST
        [HttpPost]
        public IActionResult AdminLogin(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == HashPassword( password) && u.Role == "Admin");

            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View();
            }

            return RedirectToAction("Portal", "Admin");
        }

        // Student Login GET
        [HttpGet]
        public IActionResult StudentLogin()
        {
            return View(); // Varsayılan olarak Views/Account/StudentLogin.cshtml aranır
        }

        // Student Login POST
        [HttpPost]
        public IActionResult StudentLogin(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password ==HashPassword(password) && u.Role == "Student");

            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View();
            }

            return RedirectToAction("Page", "Student");
        }

        // Register GET
        [HttpGet]
        public IActionResult Register()
        {
            return View(); // Varsayılan olarak Views/Account/Register.cshtml aranır
        }

        // Register POST
        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "A user with this email already exists.");
                }

                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "A user with this username already exists.");
                }

                if (ModelState.ErrorCount > 0)
                {
                    return View(model);
                }

                model.Role = "Student";
                model.Password = HashPassword(model.Password);
                _context.Users.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("StudentLogin", "Account");
            }

            return View(model);
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}