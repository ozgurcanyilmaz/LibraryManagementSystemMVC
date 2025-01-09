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

        // Kitap listesini doğrudan modele bağlayarak döndür
        return View(lastAddedBooks);
    }
}
