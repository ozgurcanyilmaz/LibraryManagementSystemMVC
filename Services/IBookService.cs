using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public interface IBookService
    {
        // Tüm kitapları getir
        Task<List<Book>> GetAllBooksAsync();

        // Kitap ID'sine göre kitap getir
        Task<Book> GetBookByIdAsync(int id);

        // Yeni kitap ekle
        Task AddBookAsync(BookViewModel model);

        // Son eklenen kitapları getir
        Task<List<Book>> GetLastAddedBooksAsync(int count);
        Task<List<Book>> GetLastRentedBooksAsync(int userId, int count);

        // Kitap güncelle
        Task<bool> UpdateBookAsync(Book model);

        // ISBN ile kitap getir
        Task<Book?> GetBookByISBNAsync(string isbn);

        // Kitap ID'si ile kitap sil
        Task DeleteBookAsync(int id);

        // ISBN ile kitap sil
        Task<bool> DeleteBookByISBNAsync(string isbn);

        // Tüm kiralanmış kitapları getir (Admin için)
        Task<List<Book>> GetRentedBooksAsync();

        // Kullanıcının kiraladığı kitapları getir
        Task<List<Rental>> GetRentalsWithDetailsAsync();
        Task<List<Book>> GetRentedBooksByUserAsync(string userId);

        // Kitap kirala
        Task<bool> RentBookAsync(Book book, string userId);
    }
}