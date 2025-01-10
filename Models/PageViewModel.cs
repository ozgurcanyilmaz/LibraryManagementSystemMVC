namespace LibraryManagementSystem.Models
{
    public class PageViewModel
    {
        public List<Book> LastAddedBooks { get; set; }
        public List<Rental> RentedBooks { get; set; }
    }
}
