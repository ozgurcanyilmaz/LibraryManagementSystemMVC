using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ISBN is required.")]
        [StringLength(13, ErrorMessage = "ISBN must be 13 characters.")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        public string Author { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public bool IsRented { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
