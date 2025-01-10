using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Tüm kitapları getir
        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        // ID'ye göre kitap getir
        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        // Yeni kitap ekle
        public async Task AddBookAsync(BookViewModel model)
        {
            var book = new Book
            {
                ISBN = model.ISBN,
                Title = model.Title,
                Author = model.Author,
                Description = model.Description,
                CreatedAt = DateTime.Now,
                IsRented = false
            };

            if (model.Image != null)
            {
                var fileName = Path.GetFileName(model.Image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                book.ImagePath = $"/images/{fileName}"; // Veritabanına görsel yolu kaydediyoruz
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Book>> GetLastAddedBooksAsync(int count)
        {
            return await _context.Books
                .OrderByDescending(b => b.Id) // ID'ye göre en son eklenenler
                .Take(count)
                .ToListAsync();
        }


        // Kitap güncelle
        public async Task<bool> UpdateBookAsync(Book model)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.ISBN == model.ISBN);
            if (book == null)
            {
                Console.WriteLine("Book not found with ISBN: " + model.ISBN); // Debugging
                return false;
            }

            // Kitap bilgilerini güncelle
            book.Title = model.Title;
            book.Author = model.Author;
            book.Description = model.Description;
            book.ImagePath = model.ImagePath;

            await _context.SaveChangesAsync();

            Console.WriteLine("Book updated with ISBN: " + model.ISBN); // Debugging
            return true;
        }


        public async Task<Book?> GetBookByISBNAsync(string isbn)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        // Kitap sil
        public async Task DeleteBookAsync(int id)
        {
            var book = await GetBookByIdAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
        // ISBN numarasına göre kitap sil
        public async Task<bool> DeleteBookByISBNAsync(string isbn)
        {

            var book = await _context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn);


            if (book == null)
            {
                Console.WriteLine("Book not found with ISBN: " + isbn); // Debugging
                return false;
            }


            _context.Books.Remove(book);


            await _context.SaveChangesAsync();

            Console.WriteLine("Book deleted with ISBN: " + isbn); // Debugging
            return true;
        }


        // Kiralanmış kitapları getir
        public async Task<List<Book>> GetRentedBooksAsync()
        {
            return await _context.Books.Where(b => b.IsRented).ToListAsync();
        }
        public async Task<List<Book>> GetRentedBooksByUserAsync(string userId)
        {
            // Kullanıcının kiraladığı kitapları getir
            return await _context.RentedBooks
                .Where(rb => rb.UserId == userId)
                .Select(rb => rb.Book)
                .ToListAsync();
        }

        public async Task<bool> RentBookAsync(Book book, string userId)
        {
            if (book == null || string.IsNullOrEmpty(userId))
                return false;

            var rental = new Rental
            {
                BookId = book.Id,
                UserId = int.Parse(userId), // Eğer UserId int ise
                RentalDate = DateTime.Now
            };

            book.IsRented = true;

            _context.Rentals.Add(rental);
            _context.Books.Update(book);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while renting book: {ex.Message}");
                return false;
            }
        }

    }
}
