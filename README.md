# AutoRepair ERP Consumer

A separate ASP.NET Core MVC application that consumes the **AutoRepair ERP System** entirely through its REST API — no direct database connection, no duplicated data, no separate database setup required.

This project demonstrates that an external application can fully integrate with the ERP's core modules purely over HTTP, using the same shared backend and live data as the main ERP system.

🔗 Main ERP Repository: https://github.com/<you>/AutoRepair-ERP-System
🌐 Live ERP Deployment: http://autorepairerp.runasp.net/
📘 API / Swagger Docs: http://autorepairerp.runasp.net/swagger

---

## Purpose

Instead of standing up a separate database and duplicating business logic, this application proves that any external client can plug directly into the ERP's REST API and read/write real, live data — making the ERP genuinely reusable across multiple frontends, not just its own MVC views.

---

## Architecture

- Consumes the ERP's REST API via `HttpClient` (`IHttpClientFactory`)
- No `DbContext`, no connection string, no direct SQL Server access
- Dedicated service layer wraps each API call and maps API DTOs to this app's own view models
- Registered and injected through the standard ASP.NET Core DI container

**Flow:**
`View → Controller → API Service (HttpClient) → ERP REST API → SQL Server`
`(response DTO flows back up the same chain into the view)`

---

## Modules Consumed

| Module     | ERP API Endpoint     |
|------------|-----------------------|
| Customers  | `api/customers`      |
| Vehicles   | `api/vehicles`       |
| Parts      | `api/parts`          |
| Services   | `api/services`       |
| Categories | `api/categories`     |

---

## Tech Stack

- ASP.NET Core MVC
- C#
- HttpClient / IHttpClientFactory
- Bootstrap (or whatever you used)

---

## Getting Started

1. Clone the repo
```bash
   git clone https://github.com/<you>/AutoRepair-ERP-Consumer.git
```
2. Set the ERP API base URL in `appsettings.json`:
```json
   {
     "ErpApi": {
       "BaseUrl": "http://autorepairerp.runasp.net/"
     }
   }
```
3. Run the app:
```bash
   dotnet run
```

---

## Related Projects

This is one half of a two-part system:
- **AutoRepair ERP System** (provider) — https://github.com/<you>/AutoRepair-ERP-System
- **AutoRepair ERP Consumer** (this repo) — API-driven client