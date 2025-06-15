using SepcialMomentBE.Models;

namespace SepcialMomentBE.Services
{
    public interface IWeddingService
    {
        // Wedding Expenses
        Task<IEnumerable<WeddingExpense>> GetExpensesAsync(int eventId);
        Task<WeddingExpense> CreateExpenseAsync(WeddingExpense expense);
        Task<bool> UpdateExpenseAsync(WeddingExpense expense);
        Task<bool> DeleteExpenseAsync(int id);

        // Wedding Guests
        Task<IEnumerable<WeddingGuest>> GetGuestsAsync(int eventId);
        Task<WeddingGuest> CreateGuestAsync(WeddingGuest guest);
        Task<bool> UpdateGuestAsync(WeddingGuest guest);
        Task<bool> DeleteGuestAsync(int id);

        // Tables
        Task<IEnumerable<Table>> GetTablesAsync(int eventId);
        Task<Table> CreateTableAsync(Table table);
        Task<bool> UpdateTableAsync(Table table);
        Task<bool> DeleteTableAsync(int id);

        // Service Providers
        Task<IEnumerable<WeddingServiceProvider>> GetServiceProvidersAsync(int eventId);
        Task<WeddingServiceProvider> CreateServiceProviderAsync(WeddingServiceProvider provider);
        Task<bool> UpdateServiceProviderAsync(WeddingServiceProvider provider);
        Task<bool> DeleteServiceProviderAsync(int id);
    }
} 