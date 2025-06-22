using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SepcialMomentBE.Models;
using SepcialMomentBE.Services;

namespace SepcialMomentBE.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventFormTemplateController : ControllerBase
    {
        private readonly GenericCrudService<EventFormTemplate> _service;
        private readonly IEventService _eventService;
        
        public EventFormTemplateController(GenericCrudService<EventFormTemplate> service, IEventService eventService)
        {
            _service = service;
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResult<List<EventFormTemplate>>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResult<EventFormTemplate>>> GetById(Guid id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpGet("search")]
        public async Task<ActionResult<ServiceResult<List<EventFormTemplate>>>> GetByProperty([FromQuery] string propertyName, [FromQuery] string value)
            => Ok(await _service.GetByPropertyAsync(propertyName, value));

        [HttpGet("by-event-type/{eventType}")]
        public async Task<ActionResult<IEnumerable<EventFormTemplate>>> GetByEventType(string eventType)
        {
            var templates = await _eventService.GetEventFormTemplatesByEventTypeAsync(eventType);
            return Ok(templates);
        }

        [HttpPost("initialize-wedding-templates")]
        public async Task<ActionResult<ServiceResult<bool>>> InitializeWeddingTemplates()
        {
            try
            {
                var weddingTemplates = new List<EventFormTemplate>
                {
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "denumire_nunta",
                        FieldType = "text",
                        FieldLabel = "Denumirea nunții",
                        Description = "Denumirea oficială a nunții (ex: Nunta Maria și Ion)",
                        IsRequired = true,
                        DisplayOrder = 1,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Introduceți denumirea oficială a nunții"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "nume_mireasa",
                        FieldType = "text",
                        FieldLabel = "Numele miresei",
                        Description = "Numele complet al miresei",
                        IsRequired = true,
                        DisplayOrder = 2,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Introduceți numele complet al miresei"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "nume_mire",
                        FieldType = "text",
                        FieldLabel = "Numele mirelui",
                        Description = "Numele complet al mirelui",
                        IsRequired = true,
                        DisplayOrder = 3,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Introduceți numele complet al mirelui"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "parinti_mireasa",
                        FieldType = "text",
                        FieldLabel = "Părinții miresei",
                        Description = "Numele părinților miresei",
                        IsRequired = true,
                        DisplayOrder = 4,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Introduceți numele părinților miresei"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "parinti_mire",
                        FieldType = "text",
                        FieldLabel = "Părinții mirelui",
                        Description = "Numele părinților mirelui",
                        IsRequired = true,
                        DisplayOrder = 5,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Introduceți numele părinților mirelui"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "nume_nasi",
                        FieldType = "text",
                        FieldLabel = "Numele nașilor",
                        Description = "Numele nașilor de nuntă",
                        IsRequired = true,
                        DisplayOrder = 6,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Introduceți numele nașilor de nuntă"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "denumire_biserica",
                        FieldType = "text",
                        FieldLabel = "Denumirea bisericii",
                        Description = "Numele bisericii unde va avea loc ceremonia",
                        IsRequired = true,
                        DisplayOrder = 7,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Introduceți numele bisericii (ex: Biserica Ortodoxă Sfântul Gheorghe)"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "adresa_biserica",
                        FieldType = "text",
                        FieldLabel = "Adresa bisericii",
                        Description = "Adresa completă a bisericii",
                        IsRequired = true,
                        DisplayOrder = 8,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Introduceți adresa completă a bisericii"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "data_ceremonie",
                        FieldType = "date",
                        FieldLabel = "Data ceremoniei",
                        Description = "Data când va avea loc ceremonia religioasă",
                        IsRequired = true,
                        DisplayOrder = 9,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Selectați data ceremoniei"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "ora_ceremonie",
                        FieldType = "time",
                        FieldLabel = "Ora ceremoniei",
                        Description = "Ora când începe ceremonia religioasă",
                        IsRequired = true,
                        DisplayOrder = 10,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Selectați ora ceremoniei"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "denumire_sala",
                        FieldType = "text",
                        FieldLabel = "Denumirea sălii de recepție",
                        Description = "Numele sălii sau restaurantului pentru recepție",
                        IsRequired = true,
                        DisplayOrder = 11,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Introduceți numele sălii de recepție"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "adresa_sala",
                        FieldType = "text",
                        FieldLabel = "Adresa sălii de recepție",
                        Description = "Adresa completă a sălii de recepție",
                        IsRequired = true,
                        DisplayOrder = 12,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Introduceți adresa completă a sălii de recepție"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "data_receptie",
                        FieldType = "date",
                        FieldLabel = "Data recepției",
                        Description = "Data când va avea loc recepția",
                        IsRequired = true,
                        DisplayOrder = 13,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Selectați data recepției"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "ora_receptie",
                        FieldType = "time",
                        FieldLabel = "Ora recepției",
                        Description = "Ora când începe recepția",
                        IsRequired = true,
                        DisplayOrder = 14,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Selectați ora recepției"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "numar_invitati",
                        FieldType = "number",
                        FieldLabel = "Numărul de invitați",
                        Description = "Numărul total de invitați la eveniment",
                        IsRequired = true,
                        DisplayOrder = 15,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Introduceți numărul total de invitați"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "stil_nunta",
                        FieldType = "select",
                        FieldLabel = "Stilul nunții",
                        Description = "Stilul tematic al nunții",
                        IsRequired = false,
                        DisplayOrder = 16,
                        Options = "[\"Clasic\", \"Modern\", \"Rustic\", \"Elegant\", \"Bohemian\", \"Vintage\", \"Minimalist\", \"Romantic\", \"Tradițional românesc\"]",
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Selectați stilul dorit pentru nunta voastră"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "schema_culori",
                        FieldType = "text",
                        FieldLabel = "Schema de culori",
                        Description = "Culorile principale ale nunții",
                        IsRequired = false,
                        DisplayOrder = 17,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Ex: Alb și auriu, Roșu și negru, Albastru și alb, etc."
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "buget_total",
                        FieldType = "number",
                        FieldLabel = "Bugetul total",
                        Description = "Bugetul total alocat pentru nuntă (în lei)",
                        IsRequired = false,
                        DisplayOrder = 18,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Introduceți bugetul total în lei"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "fotograf",
                        FieldType = "text",
                        FieldLabel = "Fotograf",
                        Description = "Numele fotografului ales",
                        IsRequired = false,
                        DisplayOrder = 19,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Introduceți numele fotografului"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "catering",
                        FieldType = "text",
                        FieldLabel = "Serviciu catering",
                        Description = "Numele companiei de catering",
                        IsRequired = false,
                        DisplayOrder = 20,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Introduceți numele companiei de catering"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "tip_muzica",
                        FieldType = "select",
                        FieldLabel = "Tipul de muzică",
                        Description = "Tipul de muzică dorit pentru recepție",
                        IsRequired = false,
                        DisplayOrder = 21,
                        Options = "[\"DJ\", \"Trupă live\", \"Orchestră\", \"DJ + Trupă\", \"Muzică înregistrată\", \"Muzică tradițională românească\"]",
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Selectați tipul de muzică dorit"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "florar",
                        FieldType = "text",
                        FieldLabel = "Florar",
                        Description = "Numele florarului pentru decorul floral",
                        IsRequired = false,
                        DisplayOrder = 22,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Introduceți numele florarului"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "transport",
                        FieldType = "text",
                        FieldLabel = "Serviciu transport",
                        Description = "Serviciul de transport pentru invitați",
                        IsRequired = false,
                        DisplayOrder = 23,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Introduceți numele companiei de transport"
                    },
                    new EventFormTemplate
                    {
                        EventType = "nunta",
                        FieldName = "cerinte_speciale",
                        FieldType = "textarea",
                        FieldLabel = "Cerințe speciale",
                        Description = "Cerințe speciale sau note adiționale",
                        IsRequired = false,
                        DisplayOrder = 24,
                        CreatedAt = DateTime.UtcNow,
                        HelpText = "Introduceți orice cerințe speciale sau note"
                    }
                };

                foreach (var template in weddingTemplates)
                {
                    await _service.CreateAsync(template);
                }

                return Ok(new ServiceResult<bool> { IsSuccessful = true, Result = true });
            }
            catch (Exception ex)
            {
                var result = new ServiceResult<bool> { IsSuccessful = false, Result = false };
                result.AddValidationMessage($"Eroare la crearea template-urilor: {ex.Message}");
                return BadRequest(result);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResult<EventFormTemplate>>> Create([FromBody] EventFormTemplate entity)
            => Ok(await _service.CreateAsync(entity));

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResult<EventFormTemplate>>> Update(Guid id, [FromBody] EventFormTemplate entity)
            => Ok(await _service.UpdateAsync(id, entity));

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResult<bool>>> Delete(Guid id)
            => Ok(await _service.DeleteAsync(id));
    }
} 