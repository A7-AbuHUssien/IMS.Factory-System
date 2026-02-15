# Inventory Management System (IMS)

## Overview
IMS is a backend system that simulates how real companies manage inventory across multiple warehouses.  
It handles products, stock, sales orders, movements, adjustments, and reporting while enforcing real business rules and data integrity.

---

## Core Idea
Stock = Current State  
Transactions = History  

- Stock table stores current quantities (fast reads)
- Transaction table stores every movement (full audit trail)

---

## Key Business Logic
- Stock exists per warehouse
- Stock never changes without a transaction record
- Draft orders do not affect stock
- Confirmed orders reduce stock
- Stock cannot be negative
- Soft delete only
- Automatic average cost calculation

---

## Tech Stack
- ASP.NET Core Web API
- C# (.NET)
- Entity Framework Core
- AutoMapper
- Onion Architecture (API / Application / Domain / Infrastructure)
- Repository + Unit of Work
- DTO contracts + validation layer

---

## Architecture Highlights
- Controllers contain no business logic
- UseCases + DomainServices handle rules (simple CRUD operations intentionally implemented without them to avoid unnecessary complexity)
- Domain layer is framework-independent
- Snapshot reads + transaction history model
- Designed for scalability and traceability

---

## Security
- HTTPS enforced
- Authentication required
- Role + Ownership authorization
- Rate limiting protection

---

## Result
This project demonstrates real-world backend engineering practices:
- business rule modeling
- system correctness
- auditability
- scalable architecture
