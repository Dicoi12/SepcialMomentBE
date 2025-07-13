using SepcialMomentBE.Models;

namespace SepcialMomentBE.Services
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event?> GetEventByIdAsync(int id);
        Task<Event> CreateEventAsync(Event event_);
        Task<bool> UpdateEventAsync(Event event_);
        Task<bool> DeleteEventAsync(int id);
        Task<IEnumerable<EventFormTemplate>> GetEventFormTemplatesByEventTypeAsync(int? eventTypeId);
        Task<IEnumerable<EventType>> GetAllEventTypesAsync();
        Task<Event> CreateEventWithFormAsync(Event event_, List<EventForm> eventForms);
    }
} 