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

        // Authenticate API (Hashleme ile Doğrulama)
        [HttpPost]
        [Route("api/authenticate")]
        public IActionResult Authenticate([FromBody] LoginRequest request)
        {
            Console.WriteLine($"API called with Username: {request.Username}");

            // Kullanıcıyı username'e göre bul
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);

            // Kullanıcı yoksa veya şifre yanlışsa
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            // Giriş başarılı
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
                // Kullanıcı adı mevcut mu kontrol et
                if (_context.Users.Any(u => u.Username == request.Username))
                {
                    TempData["ErrorMessage"] = "This username is already taken. Please choose a different one.";
                    return View(request);
                }

                // Yeni kullanıcıyı oluştur
                var newUser = new User
                {
                    Username = request.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password), // Şifreyi hashle
                    Role = "Student" // Rolü "Student" olarak ayarla
                };

                // Veritabanına kaydet
                _context.Users.Add(newUser);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Registration successful";
                return RedirectToAction("Register", "Account");
            }

            return View(request);
        }

        // Mevcut Şifreleri Hashlemek için (Bir kere çalıştır)
        /*   public IActionResult RunHashing()
         {
             var users = _context.Users.ToList();
             foreach (var user in users)
             {
                 // Eğer şifre zaten hashlenmemişse (genelde $2a$ ile başlar)
                 if (!user.Password.StartsWith("$2a$"))
                 {
                     user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); // Şifreyi hashle
                 }
             }

             _context.SaveChanges();
             return Ok("All existing passwords have been hashed successfully.");
         }
         */
    }
}
