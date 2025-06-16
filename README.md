Aplikacja webowa do zarządzania biblioteką, zawierająca trzy główne moduły:

- 📚 ReaderService — zarządzanie czytelnikami.
- 📖 BookRentalService — wypożyczanie książek.
- 🎖️ LoyaltyService — system lojalnościowy z punktami.

## 📦 Technologie

- ASP.NET Core Web API  
- Entity Framework Core  
- PostgreSQL  
- Swagger (OpenAPI)  
- C#

## 🏁 Szybki start

### Klonowanie repozytorium

```bash
git clone https://github.com/Olesssia/PROJEKT_Szkolenie_techniczne-3_6IID-P-2022-SP01
```

### Wykonanie migracji
Otwórz konsolę Package Manager Console i wykonaj:

```bash
Add-Migration InitialCreate -Context ReaderDbContext
Update-Database -Context ReaderDbContext

Add-Migration InitialCreate -Context BookRentalsDbContext
Update-Database -Context BookRentalsDbContext

Add-Migration InitialCreate -Context LoyaltyDbContext
Update-Database -Context LoyaltyDbContext
```

## 🚀 Główne endpointy
### ReaderService
GET /api/Readers

POST /api/Readers

PUT /api/Readers/{id}

DELETE /api/Readers/{id}

### BookRentalService
#### Books Controller

GET /api/Books

POST /api/Books

PUT /api/Books/{id}

DELETE /api/Books/{id}

#### Rentals Controller

GET /api/Rentals/{readerid}

POST /api/Rentals

PUT /api/Rentals/return/{id}

### LoyaltyService
GET /api/Loyalty/{readerId}

POST /api/Loyalty/add

PUT /api/Loyalty/{readerId}

DELETE /api/Loyalty/{readerId}
