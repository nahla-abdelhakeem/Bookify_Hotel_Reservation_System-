# Raha Bookify Hotel Reservation System 🏨

A comprehensive hotel booking system built with ASP.NET Core MVC, featuring room management, online payments via Stripe, and an admin dashboard.

## 📋 Table of Contents
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Installation & Setup](#installation--setup)
- [Configuration](#configuration)
- [Database Setup](#database-setup)
- [Running the Application](#running-the-application)
- [Project Structure](#project-structure)
- [User Roles](#user-roles)
- [Screenshots](#screenshots)
- [Contributors](#contributors)

## ✨ Features

### Guest Features
- Browse available rooms with detailed information
- View room types and pricing
- Check room availability by date range
- Add multiple rooms to shopping cart
- Modify booking dates in cart
- Secure online payment via Stripe
- User registration and authentication
- User profile with booking history

### Admin Features
- Admin Dashboard with statistics
- User management
- Role management
- Room type management
- Room management
- View all bookings
- Separate admin layout

### Payment Integration
- Stripe payment gateway integration
- Secure checkout process
- Payment success/cancel pages
- Session-based cart management

## 🛠 Technologies Used

- **Backend:** ASP.NET Core MVC (.NET 8.0)
- **Database:** SQL Server with Entity Framework Core
- **Authentication:** ASP.NET Identity
- **Payment:** Stripe.net
- **Frontend:** 
  - HTML5, CSS3, JavaScript
  - Bootstrap 5
  - Font Awesome Icons
  - jQuery
- **Design Patterns:** 
  - Repository Pattern
  - Unit of Work Pattern
- **Session Management:** IHttpContextAccessor

## 📦 Prerequisites

Before running this project, ensure you have:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or Visual Studio Code
- [Stripe Account](https://stripe.com/) (for payment integration)

## 🚀 Installation & Setup

### 1. Clone the Repository

```bash
git clone https://github.com/nahla-abdelhakeem/Bookify_Hotel_Reservation_System-.git
cd BookifyHotelSystem
```

### 2. Restore NuGet Packages

```bash
dotnet restore
```

### 3. Update Database Connection String

Open `appsettings.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=BookifyHotelDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Replace `YOUR_SERVER_NAME` with your SQL Server instance name.

## ⚙️ Configuration

### Stripe Configuration

#### Option 1: Using appsettings.json (Development Only)

Add the following to your `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=BookifyHotelDB;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Stripe": {
    "PublishableKey": "pk_test_51SbThlJmpuKpbU5bJp9c2WrWqjUmv3UFzZOE8XggcdzHHOgQ0Ej8yd3jwkexrvAg67NT65iSyPsknTR2aKCboYmN00E3GGhkRQ",
    "SecretKey": "sk_test_51SbThlJmpuKpbU5bJp9c2WrWqjUmv3UFzZOE8XggcdzHHOgQ0Ej8yd3jwkexrvAg67NT65iSyPsknTR2aKCboYmN00E3GGhkRQ"
  }
}
```


## 🗄 Database Setup

### 1. Apply Migrations

```bash
dotnet ef database update
```

This will create the database and apply all migrations.

## ▶️ Running the Application

### Using Visual Studio
1. Open `BookifyHotelSystem.sln`
2. Press `F5` or click "Run"

## 📁 Project Structure

```
BookifyHotelSystem/
├── Controllers/
│   ├── AccountController.cs      # Authentication & Profile
│   ├── AdminController.cs        # Admin Dashboard
│   ├── BookingController.cs      # Booking Management
│   ├── HomeController.cs         # Home & Room Browsing
│   ├── RoleController.cs         # Role Management
│   ├── RoomController.cs         # Room Management
│   ├── RoomTypeController.cs     # Room Type Management
│   └── UserController.cs         # User Management
├── Models/
│   ├── ApplicationUser.cs        # Custom User Model
│   ├── Booking.cs                # Booking Entity
│   ├── Room.cs                   # Room Entity
│   ├── RoomBooking.cs           # Junction Table
│   └── RoomType.cs              # Room Type Entity
├── View Model/
│   ├── LoginViewModel.cs
│   ├── RegiterViewModel.cs
│   ├── ProfileViewModel.cs
│   ├── RoomViewModel.cs
│   └── BookingHistoryViewModel.cs
├── DatabaseServices/
│   ├── AppDbContext.cs
│   ├── BaseRepositories/        # Generic Repository
│   ├── Repositories/            # Specific Repositories
│   └── Unit_Of_Work/            # UoW Pattern
├── Views/
│   ├── Account/                 # Login, Register, Profile
│   ├── Admin/                   # Admin Dashboard
│   ├── Booking/                 # Checkout, Success, Cancel
│   ├── Home/                    # Index, Room Details
│   ├── Role/                    # Role Management
│   ├── Room/                    # Room CRUD
│   ├── RoomType/                # Room Type CRUD
│   ├── User/                    # User Management
│   └── Shared/
│       ├── _Layout.cshtml       # Main Layout
│       └── _AdminLayout.cshtml  # Admin Layout
├── wwwroot/
│   ├── css/
│   │   ├── admin-dashboard.css
│   │   ├── cart.css
│   │   ├── checkout.css
│   │   ├── profile.css
│   │   └── room-details.css
│   ├── js/
│   │   ├── admin-dashboard.js
│   │   ├── cart.js
│   │   └── room_details.js
│   └── Images/                  # Room & User Images
├── Utilities/
│   └── SessionExtentions.cs     # Session Helpers
└── Migrations/                  # EF Core Migrations
```

## 👥 User Roles

### Admin
- Access to admin dashboard (`/Admin/Dashboard`)
- Full CRUD on Users, Roles, Rooms, Room Types
- View all bookings
- Redirect to dashboard after login

### Regular User
- Browse and book rooms
- Manage profile
- View booking history
- Redirect to home after login

## 🙏 Acknowledgments

- ASP.NET Core Documentation
- Stripe API Documentation
- Bootstrap Framework
- Font Awesome Icons

---

**Happy Booking! 🎉**
