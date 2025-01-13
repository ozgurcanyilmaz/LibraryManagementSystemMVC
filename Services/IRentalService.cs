using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;


namespace LibraryManagementSystem.Services
{
    public interface IRentalService
    {
        Task<List<Rental>> GetAllRentalsAsync();
        Task<Rental> GetRentalByIdAsync(int id);
        Task AddRentalAsync(Rental rental);
        Task<bool> UpdateRentalAsync(Rental rental);
        Task<List<Rental>> GetRentalsByUserIdAsync(string userId);
        Task DeleteRentalAsync(int rentalId);
        Task<List<RentalViewModel>> GetRentalsWithDetailsByUserIdAsync(string userId);
    }
}
