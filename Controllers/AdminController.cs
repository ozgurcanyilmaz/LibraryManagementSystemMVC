using LibraryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    private readonly IBookService _bookService;

    public AdminController(IBookService bookService)
    {
        _bookService = bookService;
    }

    public async Task<IActionResult> Portal()
    {
        // Son eklenen 5 kitabı al
        var lastAddedBooks = await _bookService.GetLastAddedBooksAsync(5);

        // ViewBag ile kitapları gönder
        ViewBag.Books = lastAddedBooks.Select(book => new
        {
            Title = book.Title,
            Author = book.Author,
            ISBN = book.ISBN,
            Image = book.ImagePath
        }).ToList();

        return View();
    }
}
