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


        public async Task<IActionResult> Page()
        {

            // Son eklenen 5 kitabı al
            var lastAddedBooks = await _bookService.GetLastAddedBooksAsync(5);

            // Kitap listesini doğrudan modele bağlayarak döndür
            return View(lastAddedBooks);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) // Eğer ID geçerli değilse hata döndür
            {
                TempData["ErrorMessage"] = "Invalid book ID.";
                return RedirectToAction("Page");
            }

            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                TempData["ErrorMessage"] = "Book not found.";
                return RedirectToAction("Page");
            }

            return View(book);
        }


        public IActionResult ReturnBook()
        {
            return View(); // Views/Student/ReturnBook.cshtml
        }
    }
}
    

