namespace LibraryManagementSystem.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public string BookISBN { get; set; } // New field
        public string BookName { get; set; } // New field
        public string Username { get; set; } // New field

        public Book Book { get; set; }
        public User User { get; set; }
    }
}
