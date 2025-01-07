using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Page()
        {
            return View(); // Views/Student/Page.cshtml
        }

        public IActionResult ReturnBook()
        {
            return View(); // Views/Student/ReturnBook.cshtml
        }
    }
}
