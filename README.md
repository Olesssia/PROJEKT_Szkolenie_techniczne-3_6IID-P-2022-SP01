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
GET /api/readers

POST /api/readers

PUT /api/readers/{id}

DELETE /api/readers/{id}

### BookRentalService
#### Books Controller

GET /api/books

POST /api/books

PUT /api/books/{id}

DELETE /api/books/{id}

#### Rentals Controller

GET /api/rentals/{readerid}

POST /api/rentals

PUT /api/rentals/return/{id}

### LoyaltyService
GET /api/loyalty/{readerId}

POST /api/loyalty/add

PUT /api/loyalty/{readerId}

DELETE /api/loyalty/{readerId}
