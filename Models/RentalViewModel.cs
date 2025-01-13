namespace LibraryManagementSystem.ViewModels
{
    public class RentalViewModel
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime RentalDate { get; set; }
        public string BookISBN { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }

}
