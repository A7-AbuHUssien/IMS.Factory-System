# Inventory Management System (IMS)

Backend system that simulates real-world inventory operations across multiple warehouses with real business rules, audit history, and scalable architecture.

---

## What it does
- Manages products, stock, warehouses, orders, customers
- Tracks every stock movement
- Supports adjustments and reporting
- Prevents invalid operations (negative stock, direct edits)

---

## Core Design
Stock = Current State  
Transactions = History  

Fast reads → Snapshot table  
Full traceability → Transaction table  

---

## Key Rules Implemented
- Stock tied to warehouse
- No stock change without transaction
- Orders affect stock only after confirmation
- Soft delete only
- Automatic cost calculation

---

## Tech
ASP.NET Core • C# • EF Core • Clean Architecture • DTOs • Validation • Repository/UoW

---

## Architecture
- 4-layer Clean Architecture
- Controllers = transport only
- Business logic in UseCases & DomainServices
- Framework-independent domain
- Built for scale + auditability

---

## Security
HTTPS • Authentication • Role + Ownership authorization • Rate limiting

---

## Why this project matters
Demonstrates real backend engineering skills:
- domain modeling
- data integrity design
- production architecture thinking
- scalable system design
