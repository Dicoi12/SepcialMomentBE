# Special Moment

Aplicație pentru gestionarea evenimentelor speciale, cum ar fi nunți, botezuri și alte evenimente importante.

## Structura Proiectului

Proiectul este împărțit în două părți principale:

### Backend (.NET Core)

- API REST pentru gestionarea evenimentelor
- Autentificare și autorizare
- Gestionarea bazei de date PostgreSQL
- Servicii pentru gestionarea evenimentelor, invitaților, cheltuielilor etc.

### Frontend (în dezvoltare)

Directorul `frontend` va conține interfața utilizator, dezvoltată cu tehnologii moderne web.

## Cerințe

- .NET 8.0 SDK
- PostgreSQL
- Node.js (pentru frontend)

## Instalare

### Backend

1. Clonați repository-ul
2. Configurați connection string-ul în `appsettings.json`
3. Rulați migrările bazei de date:
   ```bash
   dotnet ef database update
   ```
4. Porniți aplicația:
   ```bash
   dotnet run
   ```

### Frontend (în dezvoltare)

Instrucțiunile pentru frontend vor fi adăugate când acesta va fi implementat.

## Contribuție

1. Fork repository-ul
2. Creați un branch nou pentru feature (`git checkout -b feature/AmazingFeature`)
3. Commit schimbările (`git commit -m 'Add some AmazingFeature'`)
4. Push la branch (`git push origin feature/AmazingFeature`)
5. Deschideți un Pull Request

## Licență

Acest proiect este licențiat sub MIT License - vedeți fișierul [LICENSE](LICENSE) pentru detalii.

## Funcționalitate nouă: Template-uri pentru evenimente

### Descriere
Sistemul permite crearea de template-uri predefinite pentru diferite tipuri de evenimente. Când un utilizator alege un tip de eveniment, se încarcă automat câmpurile predefinite pentru acel tip.

### Template-uri pentru nuntă
Sistemul include template-uri predefinite pentru evenimentele de tip "nunta" cu următoarele câmpuri:

1. **Denumirea nunții** (text, obligatoriu)
2. **Numele miresei** (text, obligatoriu)
3. **Numele mirelui** (text, obligatoriu)
4. **Părinții miresei** (text, obligatoriu)
5. **Părinții mirelui** (text, obligatoriu)
6. **Numele nașilor** (text, obligatoriu)
7. **Denumirea bisericii** (text, obligatoriu)
8. **Adresa bisericii** (text, obligatoriu)
9. **Data ceremoniei** (date, obligatoriu)
10. **Ora ceremoniei** (time, obligatoriu)
11. **Denumirea sălii de recepție** (text, obligatoriu)
12. **Adresa sălii de recepție** (text, obligatoriu)
13. **Data recepției** (date, obligatoriu)
14. **Ora recepției** (time, obligatoriu)
15. **Numărul de invitați** (number, obligatoriu)
16. **Stilul nunții** (select, opțional)
17. **Schema de culori** (text, opțional)
18. **Bugetul total** (number, opțional)
19. **Fotograf** (text, opțional)
20. **Serviciu catering** (text, opțional)
21. **Tipul de muzică** (select, opțional)
22. **Florar** (text, opțional)
23. **Serviciu transport** (text, opțional)
24. **Cerințe speciale** (textarea, opțional)

### API Endpoints

#### 1. Inițializare template-uri pentru nuntă
```http
POST /api/EventFormTemplate/initialize-wedding-templates
```
Creează template-urile predefinite pentru evenimentele de tip "wedding".

#### 2. Obținere template-uri după tipul de eveniment
```http
GET /api/EventFormTemplate/by-event-type/{eventType}
```
Returnează toate template-urile pentru un tip specific de eveniment.

**Exemplu:**
```http
GET /api/EventFormTemplate/by-event-type/nunta
```

#### 3. Creare eveniment cu formular completat
```http
POST /api/Event/with-form
```

**Body request:**
```json
{
  "title": "Nunta Maria și Ion",
  "description": "Nunta noastră specială",
  "date": "2024-06-15T18:00:00Z",
  "location": "București",
  "eventType": "nunta",
  "formData": [
    {
      "formTemplateId": 1,
      "fieldValue": "Nunta Maria și Ion"
    },
    {
      "formTemplateId": 2,
      "fieldValue": "Maria Popescu"
    },
    {
      "formTemplateId": 3,
      "fieldValue": "Ion Ionescu"
    },
    {
      "formTemplateId": 4,
      "fieldValue": "Ioan și Maria Popescu"
    },
    {
      "formTemplateId": 5,
      "fieldValue": "Gheorghe și Ana Ionescu"
    },
    {
      "formTemplateId": 6,
      "fieldValue": "Vasile și Elena Dumitrescu"
    },
    {
      "formTemplateId": 7,
      "fieldValue": "Biserica Ortodoxă Sfântul Gheorghe"
    },
    {
      "formTemplateId": 8,
      "fieldValue": "Strada Bisericii nr. 15, București"
    }
  ]
}
```

### Flux de utilizare

1. **Inițializare template-uri** (o singură dată):
   - Apelează `POST /api/EventFormTemplate/initialize-wedding-templates` pentru a crea template-urile pentru nuntă

2. **Obținere câmpuri pentru tipul de eveniment**:
   - Apelează `GET /api/EventFormTemplate/by-event-type/nunta` pentru a obține toate câmpurile pentru nuntă

3. **Creare eveniment cu formular completat**:
   - Folosește `POST /api/Event/with-form` pentru a crea evenimentul cu datele completate

### Structura datelor

#### EventFormTemplate
- `Id`: ID-ul template-ului
- `EventType`: Tipul de eveniment (ex: "wedding")
- `FieldName`: Numele câmpului în sistem
- `FieldType`: Tipul câmpului (text, number, date, select, textarea)
- `FieldLabel`: Eticheta afișată utilizatorului
- `Description`: Descrierea câmpului
- `IsRequired`: Dacă câmpul este obligatoriu
- `DisplayOrder`: Ordinea de afișare
- `Options`: Opțiuni pentru câmpurile de tip select (JSON)
- `HelpText`: Text de ajutor pentru utilizator

#### EventForm
- `Id`: ID-ul formularului
- `EventId`: ID-ul evenimentului
- `FormTemplateId`: ID-ul template-ului
- `FieldValue`: Valoarea completată de utilizator

### Exemple de utilizare

#### Obținere template-uri pentru nuntă
```javascript
const response = await fetch('/api/EventFormTemplate/by-event-type/nunta', {
  headers: {
    'Authorization': `Bearer ${token}`
  }
});
const templates = await response.json();
```

#### Creare eveniment cu formular
```javascript
const eventData = {
  title: "Nunta noastră",
  description: "O zi specială",
  date: "2024-06-15T18:00:00Z",
  location: "București",
  eventType: "nunta",
  formData: [
    { formTemplateId: 1, fieldValue: "Nunta Maria și Ion" },
    { formTemplateId: 2, fieldValue: "Maria Popescu" },
    { formTemplateId: 3, fieldValue: "Ion Ionescu" },
    { formTemplateId: 7, fieldValue: "Biserica Ortodoxă Sfântul Gheorghe" },
    { formTemplateId: 11, fieldValue: "Sala de recepții Grand Hotel" }
  ]
};

const response = await fetch('/api/Event/with-form', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  },
  body: JSON.stringify(eventData)
});
``` 