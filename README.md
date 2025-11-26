# Auction System -- ASP.NET Core Web API

A clean and modular **Online Auction Platform** built with **ASP.NET
Core Web API**, **Entity Framework Core**, **JWT Authentication**, and
**SQLite**.\
The project is designed as a production-ready backend for real auction
workflows: creating auctions, placing bids, user authentication,
countdown logic, and more.

This repository is part of my Full-Stack learning journey and is
structured professionally for potential employers and remote junior
positions.

------------------------------------------------------------------------

## 🚀 Features

### 🔐 Authentication & Security

-   User Registration & Login (JWT)
-   Password hashing (ASP.NET Identity)
-   Protected endpoints
-   Role-based restrictions (Admin / User)

### 🛒 Auction System

-   Create new auctions\
-   Set starting price, start/end time\
-   Real-time bid validation\
-   Prevent bidding after auction expiration\
-   Auto-select highest bid

### 💸 Bid System

-   Users can place bids on active auctions\
-   Minimum-increment validation\
-   Returns bid history per auction

### 🗄 Database (EF Core + SQLite)

-   Code-first migrations\
-   Seed data (optional)\
-   Clean DbContext structure

### ⚙️ Clean Architecture

-   Models (Auction, Bid, User)
-   DTOs for input/output
-   Services (AuctionService, BidService)
-   Controllers with validation
-   Repository-like structure using EF Core

------------------------------------------------------------------------

## 📂 Project Structure

    AuctionSystem/
    │
    ├── AuctionSystem.sln
    ├── AuctionSystem/                # Backend project (Web API)
    │   ├── Controllers/
    │   ├── Models/
    │   ├── DTOs/
    │   ├── Services/
    │   ├── Data/ (DbContext + Migrations)
    │   ├── appsettings.json
    │   └── Program.cs
    │
    └── AuctionSystem.Client/         # Frontend (optional/placeholder)

------------------------------------------------------------------------

## 🛠 Technologies

-   **C# / .NET 8**
-   **ASP.NET Core Web API**
-   **Entity Framework Core**
-   **JWT Authentication**
-   **SQLite database**
-   **Swagger / OpenAPI**
-   (Optional) Blazor / React client

------------------------------------------------------------------------

## ▶️ How to Run Locally

### 1️⃣ Clone the repository

``` bash
git clone https://github.com/SabaTchanturia/AuctionSystem.git
cd AuctionSystem/AuctionSystem
```

### 2️⃣ Restore packages

``` bash
dotnet restore
```

### 3️⃣ Run migrations (if needed)

``` bash
dotnet ef database update
```

### 4️⃣ Run the API

``` bash
dotnet run
```

API will start on:

    http://localhost:5000
    http://localhost:5000/swagger

------------------------------------------------------------------------

## 📸 Screenshots (Optional)

Add them in a `/screenshots` folder and reference here:

    ![Swagger UI](screenshots/swagger.png)
    ![Auctions Page](screenshots/auctions.png)

------------------------------------------------------------------------

## 🌍 Why This Project Is Valuable for Remote Work

-   Shows **real backend logic** (not just CRUD).
-   Contains **authentication, services, validation**, and **business
    rules**.
-   Demonstrates knowledge of:
    -   Clean code
    -   API design
    -   Database modeling
    -   Real-world features (bidding rules, auction timers)
-   Perfect for **Junior / Beginner remote positions**.

------------------------------------------------------------------------



## 📜 License

This project is MIT-licensed. You may use, modify, and share it freely.
