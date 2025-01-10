using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class RentedBook
    {
        public int Id { get; set; } // Primary key

        [Required]
        public string UserId { get; set; } // Kiralayan kullanıcı

        [Required]
        public int BookId { get; set; } // Kiralanan kitabın ID'si

        public DateTime RentDate { get; set; } = DateTime.Now; // Kiralama tarihi

        // Navigation property
        public Book Book { get; set; }
    }
}
