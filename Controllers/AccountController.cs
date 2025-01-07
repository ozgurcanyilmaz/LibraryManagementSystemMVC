using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AdminLogin()
        {
            return View(); // Views/Account/AdminLogin.cshtml
        }

        public IActionResult Register()
        {
            return View(); // Views/Account/Register.cshtml
        }

        public IActionResult StudentLogin()
        {
            return View(); // Views/Account/StudentLogin.cshtml
        }
    }
}
