namespace LibraryManagementSystem.Models
{
  public class PageViewModel
    {
        public List<Book> LastAddedBooks { get; set; } // Last added books
        public List<Book> LastRentedBooks { get; set; } // Last rented books
   
    }
}
