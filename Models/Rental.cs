namespace LibraryManagementSystem.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public Book Book { get; set; }
        public User User { get; set; }
    }
}
