using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

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

        // Kitap detayları
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0) // ID boşsa hata mesajı göster
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

        // Kitap kiralama

        [HttpPost]
        public async Task<IActionResult> Rent(int bookId)
        {
            var book = await _bookService.GetBookByIdAsync(bookId);
            if (book == null)
            {
                TempData["ErrorMessage"] = "Book not found.";
                return RedirectToAction("Page");
            }

            if (book.IsRented)
            {
                TempData["ErrorMessage"] = "This book is already rented.";
                return RedirectToAction("Details", new { id = bookId });
            }

            var userId = HttpContext.User.Identity.Name;
            var rentResult = await _bookService.RentBookAsync(book, userId);
            if (!rentResult)
            {
                TempData["ErrorMessage"] = "Failed to rent the book.";
                return RedirectToAction("Details", new { id = bookId });
            }

            TempData["SuccessMessage"] = "Book rented successfully.";
            return RedirectToAction("Page", "Student");
        }


        // Kullanıcının kiraladığı kitapları listeleme
        public async Task<IActionResult> MyRentedBooks()
        {
            var userId = HttpContext.User.Identity.Name; // Şu anda oturum açan kullanıcıyı al
            var rentedBooks = await _bookService.GetRentedBooksByUserAsync(userId);
            return View(rentedBooks);
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

        // ISBN Kitap Silme
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
        [HttpGet]
        public async Task<IActionResult> Update(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                return View();
            }

            var book = await _bookService.GetBookByISBNAsync(isbn);
            if (book == null)
            {
                TempData["ErrorMessage"] = $"No book found with ISBN: {isbn}.";
                return View();
            }

            var bookViewModel = new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                ISBN = book.ISBN
            };

            ViewBag.ISBN = isbn;
            return View(bookViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(BookViewModel model, IFormFile? Image)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data. Please correct the errors.";
                return View(model);
            }

            var book = await _bookService.GetBookByISBNAsync(model.ISBN);
            if (book == null)
            {
                TempData["ErrorMessage"] = "No book found with the given ISBN.";
                return View(model);
            }

            book.Title = model.Title;
            book.Author = model.Author;
            book.Description = model.Description;

            if (Image != null && Image.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(book.ImagePath))
                {
                    var oldFilePath = Path.Combine("wwwroot", book.ImagePath);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                book.ImagePath = "/images/" + fileName;
            }

            var success = await _bookService.UpdateBookAsync(book);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to update the book.";
                return View(model);
            }

            TempData["SuccessMessage"] = "Book updated successfully.";
            return RedirectToAction("Update", "Book");
        }
    }
}
