using Microsoft.EntityFrameworkCore;
using SepcialMomentBE.Data;
using SepcialMomentBE.Models;

namespace SepcialMomentBE.Services
{
    public class WeddingService : IWeddingService
    {
        private readonly ApplicationDbContext _context;

        public WeddingService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Wedding Expenses
        public async Task<IEnumerable<WeddingExpense>> GetExpensesAsync(int eventId)
        {
            return await _context.WeddingExpenses
                .Where(we => we.EventId == eventId)
                .ToListAsync();
        }

        public async Task<WeddingExpense> CreateExpenseAsync(WeddingExpense expense)
        {
            _context.WeddingExpenses.Add(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<bool> UpdateExpenseAsync(WeddingExpense expense)
        {
            var existingExpense = await _context.WeddingExpenses.FindAsync(expense.Id);
            if (existingExpense == null)
            {
                return false;
            }

            _context.Entry(existingExpense).CurrentValues.SetValues(expense);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var expense = await _context.WeddingExpenses.FindAsync(id);
            if (expense == null)
            {
                return false;
            }

            _context.WeddingExpenses.Remove(expense);
            await _context.SaveChangesAsync();
            return true;
        }

        // Wedding Guests
        public async Task<IEnumerable<WeddingGuest>> GetGuestsAsync(int eventId)
        {
            return await _context.WeddingGuests
                .Where(wg => wg.EventId == eventId)
                .ToListAsync();
        }

        public async Task<WeddingGuest> CreateGuestAsync(WeddingGuest guest)
        {
            _context.WeddingGuests.Add(guest);
            await _context.SaveChangesAsync();
            return guest;
        }

        public async Task<bool> UpdateGuestAsync(WeddingGuest guest)
        {
            var existingGuest = await _context.WeddingGuests.FindAsync(guest.Id);
            if (existingGuest == null)
            {
                return false;
            }

            _context.Entry(existingGuest).CurrentValues.SetValues(guest);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteGuestAsync(int id)
        {
            var guest = await _context.WeddingGuests.FindAsync(id);
            if (guest == null)
            {
                return false;
            }

            _context.WeddingGuests.Remove(guest);
            await _context.SaveChangesAsync();
            return true;
        }

        // Tables
        public async Task<IEnumerable<Table>> GetTablesAsync(int eventId)
        {
            return await _context.Tables
                .Where(t => t.EventId == eventId)
                .ToListAsync();
        }

        public async Task<Table> CreateTableAsync(Table table)
        {
            _context.Tables.Add(table);
            await _context.SaveChangesAsync();
            return table;
        }

        public async Task<bool> UpdateTableAsync(Table table)
        {
            var existingTable = await _context.Tables.FindAsync(table.Id);
            if (existingTable == null)
            {
                return false;
            }

            _context.Entry(existingTable).CurrentValues.SetValues(table);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTableAsync(int id)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table == null)
            {
                return false;
            }

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
            return true;
        }

        // Service Providers
        public async Task<IEnumerable<WeddingServiceProvider>> GetServiceProvidersAsync(int eventId)
        {
            return await _context.ServiceProviders
                .Where(sp => sp.EventId == eventId)
                .ToListAsync();
        }

        public async Task<WeddingServiceProvider> CreateServiceProviderAsync(WeddingServiceProvider provider)
        {
            _context.ServiceProviders.Add(provider);
            await _context.SaveChangesAsync();
            return provider;
        }

        public async Task<bool> UpdateServiceProviderAsync(WeddingServiceProvider provider)
        {
            var existingProvider = await _context.ServiceProviders.FindAsync(provider.Id);
            if (existingProvider == null)
            {
                return false;
            }

            _context.Entry(existingProvider).CurrentValues.SetValues(provider);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteServiceProviderAsync(int id)
        {
            var provider = await _context.ServiceProviders.FindAsync(id);
            if (provider == null)
            {
                return false;
            }

            _context.ServiceProviders.Remove(provider);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 