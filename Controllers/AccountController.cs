using LibraryManagementSystem.Models;
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
        public IActionResult Authenticate([FromBody] LoginRequest request)
        {
            Console.WriteLine($"API called with Username: {request.Username}, Password: {request.Password}");

            var user = _context.Users
                .FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            return Ok(new
            {
                Username = user.Username,
                Role = user.Role,
            });
        }

    }
}
