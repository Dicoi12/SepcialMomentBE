using Microsoft.EntityFrameworkCore;
using SepcialMomentBE.Data;
using SepcialMomentBE.Models;

namespace SepcialMomentBE.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events
                .Include(e => e.User)
                .Include(e => e.EventForms)
                .Include(e => e.WeddingExpenses)
                .Include(e => e.WeddingGuests)
                .Include(e => e.Tables)
                .Include(e => e.InvitationTemplates)
                .Include(e => e.ServiceProviders)
                .ToListAsync();
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await _context.Events
                .Include(e => e.User)
                .Include(e => e.EventForms)
                .Include(e => e.WeddingExpenses)
                .Include(e => e.WeddingGuests)
                .Include(e => e.Tables)
                .Include(e => e.InvitationTemplates)
                .Include(e => e.ServiceProviders)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Event> CreateEventAsync(Event event_)
        {
            _context.Events.Add(event_);
            await _context.SaveChangesAsync();
            return event_;
        }

        public async Task<bool> UpdateEventAsync(Event event_)
        {
            var existingEvent = await _context.Events.FindAsync(event_.Id);
            if (existingEvent == null)
            {
                return false;
            }

            _context.Entry(existingEvent).CurrentValues.SetValues(event_);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            var event_ = await _context.Events.FindAsync(id);
            if (event_ == null)
            {
                return false;
            }

            _context.Events.Remove(event_);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EventFormTemplate>> GetEventFormTemplatesByEventTypeAsync(string eventType)
        {
            return await _context.EventFormTemplates
                .Where(template => template.EventType.ToLower() == eventType.ToLower())
                .OrderBy(template => template.DisplayOrder)
                .ToListAsync();
        }

        public async Task<Event> CreateEventWithFormAsync(Event event_, List<EventForm> eventForms)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // Adaug evenimentul
                _context.Events.Add(event_);
                await _context.SaveChangesAsync();
                
                // Adaug formularele asociate evenimentului
                foreach (var form in eventForms)
                {
                    form.EventId = event_.Id;
                    form.CreatedAt = DateTime.UtcNow;
                }
                
                _context.EventForms.AddRange(eventForms);
                await _context.SaveChangesAsync();
                
                await transaction.CommitAsync();
                
                // Returnez evenimentul cu formularele incluse
                return await GetEventByIdAsync(event_.Id) ?? event_;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
} 