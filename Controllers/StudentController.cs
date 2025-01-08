using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly IBookService _bookService;

        public StudentController(IBookService bookService)
        {
            _bookService = bookService;
        }
        public IActionResult Page()
        {
            return View();
        }


    }
}