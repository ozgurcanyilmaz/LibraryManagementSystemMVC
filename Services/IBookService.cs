using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetAllBooksAsync();  // Tüm kitapları getirme
        Task<Book> GetBookByIdAsync(int id);  // ID'ye göre kitap getirme
        Task AddBookAsync(BookViewModel model);  // Kitap ekleme
        Task<List<Book>> GetLastAddedBooksAsync(int count);
        Task UpdateBookAsync(BookViewModel model);  // Kitap güncelleme
        Task DeleteBookAsync(int id);  // Kitap silme
        Task<bool> DeleteBookByISBNAsync(string isbn);
        Task<List<Book>> GetRentedBooksAsync();  // Kiralanmış kitapları listeleme
    }
}
