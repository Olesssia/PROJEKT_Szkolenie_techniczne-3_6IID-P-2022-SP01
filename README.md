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

### 1. Klonowanie repozytorium

```bash
git clone https://github.com/Olesssia/PROJEKT_Szkolenie_techniczne-3_6IID-P-2022-SP01
```

### 2. Wykonanie migracji
Otwórz konsolę Package Manager Console i wykonaj:

```bash
Add-Migration InitialCreate -Context ReaderDbContext
Update-Database -Context ReaderDbContext

Add-Migration InitialCreate -Context BookRentalsDbContext
Update-Database -Context BookRentalsDbContext

Add-Migration InitialCreate -Context LoyaltyDbContext
Update-Database -Context LoyaltyDbContext
