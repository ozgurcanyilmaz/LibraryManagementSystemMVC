namespace LibraryManagementSystem.Models;
public class Book
{
    public int Id { get; set; }
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public bool IsRented { get; set; }
    public DateTime CreatedAt { get; set; }
}
