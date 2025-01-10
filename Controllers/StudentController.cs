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
        public async Task<IActionResult> MyRentedBooks()
        {
            var rentedBooks = await _bookService.GetRentalsWithDetailsAsync();
            return View(rentedBooks);
        }


        public async Task<IActionResult> Page()
        {
            var lastAddedBooks = await _bookService.GetLastAddedBooksAsync(5);
            int userId = Int32.Parse(HttpContext.User.Identity?.Name ?? "0");
            var lastRentedBooks = await _bookService.GetLastRentedBooksAsync(userId ,5); // Fetch last rented books

            var model = new PageViewModel
            {
                LastAddedBooks = lastAddedBooks,
                LastRentedBooks = lastRentedBooks
            };

            return View(model);
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
    

