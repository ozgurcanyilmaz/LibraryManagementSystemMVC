using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IRentalService _rentalService;
        public StudentController(IBookService bookService, IRentalService rentalService)
        {
            _bookService = bookService;
            _rentalService = rentalService;
        }
        public async Task<IActionResult> Page()
        {

            // Son eklenen 5 kitabı al
            var lastAddedBooks = await _bookService.GetLastAddedBooksAsync(5);

            // Kitap listesini doğrudan modele bağlayarak döndür
            return View(lastAddedBooks);
        }

        public async Task<IActionResult> RentBook()
        {
            var books = await _bookService.GetAvailableBooksAsync();
            return View(books);
        }

        [HttpPost]

        public async Task<IActionResult> RentBookProcess(string bookISBN)
        {
            // Kullanıcı bilgilerini al
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.Identity.Name;

            // Kitabı ISBN ile bul
            var book = await _bookService.GetBookByISBNAsync(bookISBN);
            if (book == null)
            {
                TempData["ErrorMessage"] = "Book not found";
                return RedirectToAction("RentBook");
            }

            if (book.IsRented)
            {
                TempData["ErrorMessage"] = "Book is already rented";
                return RedirectToAction("RentBook");
            }

            // Kitabı kiralama kaydı oluştur
            var rental = new Rental
            {
                BookId = book.Id,
                UserId = userId,
                RentalDate = DateTime.Now,
                BookISBN = book.ISBN,
                BookName = book.Title,
                Username = username
            };

            // Kiralama kaydını Rentals tablosuna ekle
            await _rentalService.AddRentalAsync(rental);

            // Kitabı kiralanmış olarak işaretle
            book.IsRented = true;
            await _bookService.UpdateBookAsync(book);

            TempData["SuccessMessage"] = $"{book.Title} rented successfully";
            return RedirectToAction("RentBook");
        }

        [Authorize]
        public async Task<IActionResult> ReturnBook()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to view your rented books.";
                return RedirectToAction("Page");
            }

            var rentals = await _rentalService.GetRentalsWithDetailsByUserIdAsync(userId);

            return View(rentals);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ReturnBookProcess(int rentalId)
        {
            // Kullanıcı bilgilerini kontrol et
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to return a book.";
                return RedirectToAction("ReturnBook");
            }

            // Kiralama kaydını bul
            var rental = await _rentalService.GetRentalByIdAsync(rentalId);
            if (rental == null || rental.UserId != userId)
            {
                TempData["ErrorMessage"] = "Rental record not found or you are not authorized to return this book.";
                return RedirectToAction("ReturnBook");
            }

            try
            {
                // Kitap durumunu güncelle (kiralanmamış olarak işaretle)
                var book = await _bookService.GetBookByIdAsync(rental.BookId);
                if (book != null)
                {
                    book.IsRented = false;
                    await _bookService.UpdateBookAsync(book);
                }

                // Kiralama kaydını kaldır
                await _rentalService.DeleteRentalAsync(rentalId);

                TempData["SuccessMessage"] = $"{book?.Title ?? "Book"} successfully returned.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while returning the book. Please try again.";
                Console.WriteLine($"Error: {ex.Message}");
            }

            return RedirectToAction("ReturnBook");
        }



    }
}
