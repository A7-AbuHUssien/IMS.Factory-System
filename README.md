# IMS Factory System

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet" />
  <img src="https://img.shields.io/badge/Architecture-Clean%20%2F%20Onion-blue?style=flat-square" />
  <img src="https://img.shields.io/badge/Database-SQL%20Server-CC2927?style=flat-square&logo=microsoftsqlserver" />
  <img src="https://img.shields.io/badge/Auth-JWT%20%2B%20Refresh-orange?style=flat-square" />
</p>

A backend system for managing inventory across multiple warehouses, built with Clean Architecture. Handles products, stock movements, sales orders, reservations, and adjustments with full audit trail.

---

## Core Design Principle

```
Stock Table     = Current State   (fast reads, quantity snapshot per warehouse)
Transaction Log = Full History    (every movement recorded immutably)
```

Draft orders do not touch stock. Confirmed orders reserve stock. Completed orders consume it.

---

## Architecture

```
IMS.API             → Controllers, Middleware, Swagger
IMS.Application     → Use Cases, Services, DTOs, Validators, Profiles
IMS.Domain          → Entities, Domain Services, Enums, Exceptions
IMS.Infrastructure  → EF Core, Repositories, UnitOfWork, JWT, Hashing
```

**Pattern:** Clean / Onion Architecture with Use Cases + Domain Services.  
Controllers contain no business logic. Use Cases own each operation end-to-end.

---

## Modules

### Auth
| Endpoint | Description |
|----------|-------------|
| `POST /api/auth/auth/register` | Register new user (assigned User role by default) |
| `POST /api/auth/auth/login` | Login, returns JWT + Refresh Token |
| `POST /api/auth/auth/refresh` | Rotate refresh token |
| `POST /api/auth/auth/forgot-password` | Send OTP to email |
| `POST /api/auth/auth/verify-otp` | Verify OTP, returns reset token |
| `POST /api/auth/auth/reset-password` | Set new password via reset token |
| `POST /api/auth/auth/change-password` | Change password (authenticated) |

### Inventory
| Endpoint | Description |
|----------|-------------|
| `GET /api/inventory/products/all` | List products (paginated, filterable) |
| `GET /api/inventory/products/{id}` | Get product by ID |
| `POST /api/inventory/products/create` | Create product |
| `PUT /api/inventory/products/update` | Update product |
| `GET /api/inventory/warehouses` | List warehouses |
| `POST /api/inventory/warehouses` | Create warehouse |
| `PUT /api/inventory/warehouses` | Update warehouse |
| `DELETE /api/inventory/warehouses/{id}` | Delete warehouse |
| `GET /api/inventory/stocks` | List stock levels (paginated) |
| `POST /api/inventory/stocks/receive` | Receive stock into warehouse |
| `POST /api/inventory/stocks/transfer` | Transfer stock between warehouses |
| `POST /api/inventory/stocks/adjust` | Manual inventory adjustment |

### Sales
| Endpoint | Description |
|----------|-------------|
| `POST /api/sales/orders/create-draft-order` | Create draft order for customer |
| `POST /api/sales/orders/add-item` | Add product to draft/pending order |
| `POST /api/sales/orders/remove-item/{orderId}/{itemId}` | Remove item from order |
| `PATCH /api/sales/orders/update-item-quantity/{orderId}/{itemId}/{qty}` | Update item quantity |
| `POST /api/sales/orders/submit/{orderId}` | Submit draft → pending |
| `POST /api/sales/orders/confirm/{orderId}` | Confirm order, reserve stock |
| `POST /api/sales/orders/complete/{orderId}` | Complete order, deduct stock |
| `POST /api/sales/orders/cancel/{orderId}` | Cancel pending order |
| `POST /api/sales/orders/return` | Register returned item |
| `GET /api/sales/orders/{orderId}` | Order details |
| `GET /api/sales/orders/get-orders` | List orders (paginated, filterable) |

---

## Order State Machine

```
Draft ──submit──► Pending ──confirm──► Confirmed ──complete──► Delivered
  │                  │
  └──cancel──────────┘
```

- **Draft:** Order under construction. No stock impact.
- **Pending:** Submitted for admin review. No stock impact.
- **Confirmed:** Stock reserved across warehouses.
- **Delivered:** Reservations consumed, stock deducted, transactions logged.

---

## Roles

| Role | Description |
|------|-------------|
| `ADMIN` | Full access to all modules |
| `MANAGER` | Read access to orders and inventory |
| `WAREHOUSE_MANAGER` | Stock operations (receive, transfer, adjust) |
| `USER` | Create and manage own orders |

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | ASP.NET Core Web API |
| ORM | Entity Framework Core + SQL Server |
| Authentication | JWT Bearer + Refresh Tokens |
| Password Hashing | PBKDF2-SHA512 (100k iterations) |
| Mapping | AutoMapper |
| Validation | FluentValidation |
| Pattern | Repository + Unit of Work |

---

## Setup

### Prerequisites
- .NET 8 SDK
- SQL Server (local or remote)

### Steps

1. **Clone the repository**
   ```bash
   git clone <repo-url>
   cd IMS.Factory-System
   ```

2. **Configure environment**

   > ⚠️ Do **not** commit real credentials. Use environment variables or `dotnet user-secrets`.

   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.;Database=IMS;User ID=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
   dotnet user-secrets set "Jwt:Key" "YOUR_RANDOM_SECRET_KEY_32_CHARS_MIN"
   ```

   Or set environment variables:
   ```
   ConnectionStrings__DefaultConnection=...
   Jwt__Key=...
   ```

3. **Create the database**
   ```bash
   # Run the provided SQL script
   sqlcmd -S localhost -d master -i Database/DB.sql
   ```

4. **Run the API**
   ```bash
   cd IMS.API
   dotnet run
   ```

5. **Open Swagger**
   ```
   https://localhost:{port}/swagger
   ```

---

## Database Schema Overview

```
Users ──────────┬── UserRoles ── Roles
                ├── RefreshTokens
                └── OTPs / ResetTokens

Products ───────┬── Stocks (per Warehouse)
                ├── StockTransactions
                ├── SalesOrderItems
                └── InventoryAdjustments

SalesOrders ────┬── SalesOrderItems ── Products
                └── ReservationRequests ── Stocks
```

Full SQL schema is in `Database/DB.sql`.

---

## Key Business Rules

- Stock quantity can never go negative
- Reservations span multiple warehouses automatically (greedy allocation)
- Average cost recalculated on every stock receipt (weighted average)
- All deletes are soft deletes — records are never physically removed
- Every stock movement generates an immutable transaction record
- Order totals (price, cost, profit) recalculate automatically on item change

---

## Project Structure

```
IMS.Factory-System/
├── IMS.API/
│   ├── Controllers/
│   │   ├── Auth/          # AuthController, UserController, RolesController
│   │   ├── Inventory/     # ProductsController, StocksController, WarehousesController
│   │   └── Sales/         # OrdersController, CustomersController
│   ├── Middlewares/       # Global exception handling
│   └── Program.cs
├── IMS.Application/
│   └── Modules/
│       ├── Auth/          # UseCases, Services, DTOs, Validators
│       ├── Inventory/     # UseCases, Services, DTOs, Validators
│       ├── Sales/         # UseCases, Services, DTOs, Validators
│       └── Reporting/     # Dashboard stats
├── IMS.Domain/
│   ├── Entities/          # Core domain models
│   ├── DomainServices/    # StockGuard, StockCalculator, ReservationDomainService
│   ├── Enums/
│   └── Exceptions/
├── IMS.Infrastructure/
│   ├── Auth/              # JwtProvider, PasswordHasher, CurrentUser
│   ├── Persistence/       # DbContext, Repository, UnitOfWork, EF Configs
│   └── Services/          # EmailService, OtpService
└── Database/
    └── DB.sql
```

---

## License

This project is for educational/demonstration purposes.
