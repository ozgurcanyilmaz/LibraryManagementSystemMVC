using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;  // DbContext, veritabanı işlemleri için

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
                CreatedAt = System.DateTime.Now,
                IsRented = false
            };

            if (model.Image != null)
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", model.Image.FileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }
                book.ImagePath = imagePath;
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
        public async Task UpdateBookAsync(BookViewModel model)
        {
            var book = await GetBookByIdAsync(model.Id);
            if (book != null)
            {
                book.Title = model.Title;
                book.Author = model.Author;
                book.Description = model.Description;

                if (model.Image != null)
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", model.Image.FileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }
                    book.ImagePath = imagePath;
                }

                _context.Books.Update(book);
                await _context.SaveChangesAsync();
            }
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

        // Kiralanmış kitapları getir
        public async Task<List<Book>> GetRentedBooksAsync()
        {
            return await _context.Books.Where(b => b.IsRented).ToListAsync();
        }
    }
}
