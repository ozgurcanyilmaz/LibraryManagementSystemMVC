using LibraryManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IBookService _bookService;

        public StudentController(IBookService bookService)
        {
            _bookService = bookService;
        }
        public async Task<IActionResult> Page()
        {
            // Son eklenen 5 kitabı al
            var lastAddedBooks = await _bookService.GetLastAddedBooksAsync(5);

            // Kitap listesini doğrudan modele bağlayarak döndür
            return View(lastAddedBooks);
        }


        public IActionResult ReturnBook()
        {
            return View(); // Views/Student/ReturnBook.cshtml
        }
    }
}