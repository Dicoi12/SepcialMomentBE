# Wizard pentru Adăugarea Evenimentelor

## Descriere

Wizard-ul pentru adăugarea evenimentelor este un sistem în 3 etape care permite utilizatorilor să creeze evenimente cu informații personalizate bazate pe tipul de eveniment selectat.

## Funcționalități

### Etapa 1: Informații de Bază
- Titlu eveniment
- Tip eveniment (Nuntă, Botez, Aniversare)
- Data și ora evenimentului
- Locație
- Descriere

### Etapa 2: Configurare Specifică
- Fetch automat al template-ului de formular pentru tipul de eveniment selectat
- Câmpuri dinamice bazate pe template
- Validare pentru câmpurile obligatorii
- Suport pentru diferite tipuri de câmpuri:
  - Text
  - Textarea
  - Number
  - Dropdown
  - Checkbox
  - Date

### Etapa 3: Revizuire și Salvare
- Afișare sumară a tuturor informațiilor
- Confirmare înainte de salvare
- Trimitere către API cu toate datele

## API Endpoints Necesare

### 1. GET /api/EventForm/GetTemplate/{eventType}
Returnează template-ul de formular pentru tipul de eveniment specificat.

**Response:**
```json
{
  "id": 1,
  "eventType": "Wedding",
  "name": "Template Nuntă",
  "description": "Template pentru evenimente de tip nuntă",
  "fields": [
    {
      "name": "numarInvitati",
      "label": "Numărul de invitați",
      "type": "number",
      "required": true,
      "placeholder": "Ex: 120",
      "defaultValue": 0,
      "validation": {
        "min": 1,
        "max": 1000
      }
    },
    {
      "name": "tipReceptie",
      "label": "Tip recepție",
      "type": "dropdown",
      "required": true,
      "options": [
        { "label": "Restaurant", "value": "restaurant" },
        { "label": "Sala de evenimente", "value": "sala" },
        { "label": "Acasă", "value": "casa" }
      ]
    },
    {
      "name": "dressCode",
      "label": "Dress code",
      "type": "text",
      "required": false,
      "placeholder": "Ex: Casual elegant"
    },
    {
      "name": "areFotograf",
      "label": "Are fotograf",
      "type": "checkbox",
      "required": false,
      "defaultValue": false
    }
  ],
  "isActive": true,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### 2. POST /api/Event
Creează un eveniment nou cu toate datele din wizard.

**Request Body:**
```json
{
  "title": "Nunta Maria și Ion",
  "eventType": "Wedding",
  "date": "2024-07-15T18:00:00Z",
  "location": "Restaurantul La Strada",
  "description": "Cea mai frumoasă zi din viața noastră",
  "userId": 1,
  "formData": {
    "numarInvitati": 120,
    "tipReceptie": "restaurant",
    "dressCode": "Casual elegant",
    "areFotograf": true
  }
}
```

## Structura Backend Recomandată

### EventFormController
```csharp
[ApiController]
[Route("api/[controller]")]
public class EventFormController : ControllerBase
{
    [HttpGet("GetTemplate/{eventType}")]
    public async Task<ActionResult<EventFormTemplate>> GetTemplate(string eventType)
    {
        // Returnează template-ul pentru tipul de eveniment
    }
}
```

### EventController
```csharp
[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Event>> CreateEvent([FromBody] CreateEventRequest request)
    {
        // Creează evenimentul cu datele de bază și formData
    }
}
```

## Tipuri de Câmpuri Suportate

### Text
```json
{
  "name": "dressCode",
  "label": "Dress code",
  "type": "text",
  "required": false,
  "placeholder": "Ex: Casual elegant"
}
```

### Textarea
```json
{
  "name": "observatii",
  "label": "Observații",
  "type": "textarea",
  "required": false,
  "placeholder": "Observații suplimentare...",
  "rows": 4
}
```

### Number
```json
{
  "name": "numarInvitati",
  "label": "Numărul de invitați",
  "type": "number",
  "required": true,
  "placeholder": "Ex: 120",
  "validation": {
    "min": 1,
    "max": 1000
  }
}
```

### Dropdown
```json
{
  "name": "tipReceptie",
  "label": "Tip recepție",
  "type": "dropdown",
  "required": true,
  "options": [
    { "label": "Restaurant", "value": "restaurant" },
    { "label": "Sala de evenimente", "value": "sala" },
    { "label": "Acasă", "value": "casa" }
  ]
}
```

### Checkbox
```json
{
  "name": "areFotograf",
  "label": "Are fotograf",
  "type": "checkbox",
  "required": false,
  "defaultValue": false
}
```

### Date
```json
{
  "name": "dataContractare",
  "label": "Data contractării",
  "type": "date",
  "required": false,
  "placeholder": "Selectează data"
}
```

## Fluxul de Utilizare

1. **Utilizatorul apasă "Eveniment Nou"**
2. **Etapa 1**: Completează informațiile de bază
3. **La trecerea la Etapa 2**: Se face fetch automat al template-ului pentru tipul de eveniment selectat
4. **Etapa 2**: Utilizatorul completează câmpurile specifice din template
5. **Etapa 3**: Revizuiește toate informațiile
6. **Salvare**: Se trimite toate datele către API

## Avantaje

- **Experiență utilizator îmbunătățită**: Wizard-ul ghidează utilizatorul pas cu pas
- **Flexibilitate**: Template-urile permit personalizarea pentru fiecare tip de eveniment
- **Validare**: Validare în timp real pentru toate câmpurile
- **Responsive**: Funcționează pe toate dispozitivele
- **Extensibilitate**: Ușor de adăugat noi tipuri de câmpuri și template-uri

## Implementare Backend

Pentru a implementa backend-ul, va trebui să:

1. Creați tabelele pentru `EventFormTemplates` și `EventFormFields`
2. Implementați `EventFormController` cu endpoint-ul `GetTemplate/{eventType}`
3. Modificați `EventController` pentru a accepta `formData` în request
4. Creați template-uri predefinite pentru fiecare tip de eveniment
5. Implementați validarea pentru câmpurile din template 