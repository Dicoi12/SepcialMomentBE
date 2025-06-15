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