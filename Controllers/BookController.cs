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
                return RedirectToAction("Portal", "Admin"); // Kitap eklendikten sonra kitaplar listesini gör
            }
            return View(model);
        }

        // Kitap silme
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound("The book you are trying to delete does not exist.");
            }

            await _bookService.DeleteBookAsync(id);
            return RedirectToAction("Portal", "Admin"); // Kitap silindikten sonra listeyi güncelle
        }

        // Kitap güncelleme
        public async Task<IActionResult> Update(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var bookViewModel = new BookViewModel
            {
                Id = book.Id,
                ISBN = book.ISBN,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description
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
