using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using Microsoft.EntityFrameworkCore;


namespace LibraryManagementSystem.Services
{
    public class RentalService : IRentalService
    {
        private readonly ApplicationDbContext _context;

        public RentalService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Tüm kiralamaları getir
        public async Task<List<Rental>> GetAllRentalsAsync()
        {
            return await _context.Rentals.ToListAsync();
        }

        // ID'ye göre kiralama getir
        public async Task<Rental> GetRentalByIdAsync(int id)
        {
            return await _context.Rentals.FirstOrDefaultAsync(r => r.Id == id);
        }

        // Yeni kiralama ekle
        public async Task AddRentalAsync(Rental rental)
        {
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();
        }

        // Kiralamayı güncelle
        public async Task<bool> UpdateRentalAsync(Rental rental)
        {
            var existingRental = await _context.Rentals.FirstOrDefaultAsync(r => r.Id == rental.Id);
            if (existingRental == null)
            {
                return false;
            }

            // Kiralama bilgilerini güncelle
            existingRental.BookId = rental.BookId;
            existingRental.UserId = rental.UserId;
            existingRental.RentalDate = rental.RentalDate;
            existingRental.ReturnDate = rental.ReturnDate;
            existingRental.BookISBN = rental.BookISBN;
            existingRental.BookName = rental.BookName;
            existingRental.Username = rental.Username;

            await _context.SaveChangesAsync();
            return true;
        }

        // ID'ye göre kiralama sil
        public async Task DeleteRentalAsync(int id)
        {
            var rental = await GetRentalByIdAsync(id);
            if (rental != null)
            {
                _context.Rentals.Remove(rental);
                await _context.SaveChangesAsync();
            }
        }

        // Belirli bir kullanıcının kiralamalarını getir
        public async Task<List<Rental>> GetRentalsByUserIdAsync(string userId)
        {
            return await _context.Rentals
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
        public async Task<List<RentalViewModel>> GetRentalsWithDetailsByUserIdAsync(string userId)
        {
            return await (from rental in _context.Rentals
                          join book in _context.Books on rental.BookId equals book.Id
                          where rental.UserId == userId
                          select new RentalViewModel
                          {
                              RentalId = rental.Id,
                              RentalDate = rental.RentalDate,
                              BookISBN = rental.BookISBN,
                              BookName = rental.BookName,
                              Author = book.Author,
                              Description = book.Description,
                              ImagePath = book.ImagePath
                          }).ToListAsync();
        }

        public async Task<List<RentalViewModel>> GetAllRentalsWithDetailsAsync()
        {
            return await (from rental in _context.Rentals
                          join book in _context.Books on rental.BookId equals book.Id
                          join user in _context.Users on rental.UserId equals user.Id.ToString()
                          select new RentalViewModel
                          {
                              RentalId = rental.Id,
                              RentalDate = rental.RentalDate,
                              BookISBN = rental.BookISBN,
                              BookName = rental.BookName,
                              Author = book.Author,
                              Description = book.Description,
                              ImagePath = book.ImagePath,
                              Username = user.Username
                          }).ToListAsync();
        }

    }
}
