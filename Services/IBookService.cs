using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task AddBookAsync(BookViewModel model);
        Task<List<Book>> GetLastAddedBooksAsync(int count);
        Task<bool> UpdateBookAsync(Book model);
        Task<Book?> GetBookByISBNAsync(string isbn);
        Task DeleteBookAsync(int id);
        Task<bool> DeleteBookByISBNAsync(string isbn);
        Task<List<Book>> GetRentedBooksAsync();
    }
}
