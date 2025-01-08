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
           public async Task<IActionResult> Portal()
    {
        // Son eklenen 5 kitabı al
        var lastAddedBooks = await _bookService.GetLastAddedBooksAsync(5);

        // Kitap listesini doğrudan modele bağlayarak döndür
        return View(lastAddedBooks);
    }
}


    }
