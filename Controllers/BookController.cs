using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        // Controller'a BookService enjekte ediyoruz
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // Kitap listeleme
        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllBooksAsync();
            return View(books); // Books listesini view'e gönder
        }

        // Kitap ekleme
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _bookService.AddBookAsync(model);
                TempData["SuccessMessage"] = "Book successfully added.";
                return RedirectToAction("Add", "Book");
            }
            return View(model);
        }

        // Delete book with ISBN confirmation
        [HttpGet]
        public IActionResult Delete(string isbn)
        {
            ViewBag.Message = $"Are you sure you want to delete the book with ISBN: {isbn}?";
            ViewBag.ISBN = isbn;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string isbn, bool confirm)
        {
            Console.WriteLine($"ISBN received: {isbn}");

            if (!confirm)
            {
                ViewBag.Message = $"Are you sure you want to delete the book with ISBN: {isbn}?";
                ViewBag.ISBN = isbn;
                return View();
            }

            var result = await _bookService.DeleteBookByISBNAsync(isbn);
            TempData["SuccessMessage"] = "Book successfully deleted.";
            return RedirectToAction("Delete", "Book");
        }



        // Kitap güncelleme
        public async Task<IActionResult> Update(int id)
        {
            Console.WriteLine($"Received ID: {id}");
            if (id == 0)
            {
                Console.WriteLine("ID is 0. This might be a routing or form issue.");
                return BadRequest("Invalid ID.");
            }

            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                Console.WriteLine($"Book with ID {id} not found.");
                return NotFound($"Book with ID {id} not found.");
            }

            var bookViewModel = new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                ISBN = book.ISBN
            };

            return View(bookViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Update(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _bookService.UpdateBookAsync(model);
                return RedirectToAction("Portal", "Admin"); // Kitap güncellendikten sonra kitaplar listesini gör
            }
            return View(model);
        }

        // Kiralanmış kitapları görüntüle
        public async Task<IActionResult> Rented()
        {
            var rentedBooks = await _bookService.GetRentedBooksAsync();
            return View(rentedBooks); // Kiralanmış kitapları görüntüle
        }
    }
}
