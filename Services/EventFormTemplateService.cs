
using SepcialMomentBE.Models;
using SepcialMomentBE.Data; 
using Microsoft.EntityFrameworkCore;

namespace SepcialMomentBE.Services;

public class EventFormTemplateService
{

    private readonly ApplicationDbContext _context;

    public EventFormTemplateService(ApplicationDbContext context)
    {
        _context = context;
        
    }

    public async Task<List<EventFormTemplate>> GetTemplatesByEventTypeAsync(EventType eventType)
    {
        string eventTypeString = eventType.ToString();
        
        var templates = await _context.EventFormTemplates
            .Where( t => t.EventType == eventTypeString )
            .OrderBy(t => t.DisplayOrder)
            .ToListAsync();
        
        return templates;   
    }
    
    
    

}