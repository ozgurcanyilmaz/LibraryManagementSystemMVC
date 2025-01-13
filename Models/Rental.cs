namespace LibraryManagementSystem.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string UserId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string BookISBN { get; set; }
        public string BookName { get; set; }
        public string Username { get; set; }


    }

}
