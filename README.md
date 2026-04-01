# Finance Tracker API

A RESTful Web API for managing personal finances, built with ASP.NET Core.  
The application allows users to register, authenticate, and manage transactions and categories securely.

---

## Features

- User authentication (register & login)
- Transaction management (CRUD)
- Category management (CRUD)
- Data validation and integrity checks
- Filtering transactions by category
- Interactive API documentation with Swagger
- Clean architecture (Application / Infrastructure / API layers)

---

## Tech Stack

- ASP.NET Core Web API  
- Entity Framework Core  
- ASP.NET Identity  
- SQL Server  
- Swagger (Swashbuckle)

---

## Project Structure


FinanceTracker
│
├── FinanceTracker.API # Controllers & entry point
├── FinanceTracker.Application # DTOs & interfaces
├── FinanceTracker.Infrastructure # Services & database logic
├── FinanceTracker.Domain # Entities & enums


---

## Setup Instructions

### 1. Clone the repository


git clone https://github.com/your-username/finance-tracker.git

cd finance-tracker


### 2. Configure database

Update connection string in:


appsettings.json


Example:


"ConnectionStrings": {
"DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=FinanceTrackerDb;Trusted_Connection=True;"
}


### 3. Apply migrations


dotnet ef database update


### 4. Run the application


dotnet run


### 5. Open Swagger


https://localhost:{port}/swagger


---

## Authentication

1. Register a new user:


POST /api/auth/register


2. Login:


POST /api/auth/login


3. Copy the returned JWT token

4. In Swagger, click "Authorize" and enter:


Bearer YOUR_TOKEN


---

## API Endpoints

### Auth


POST /api/auth/register
POST /api/auth/login


### Transactions


GET /api/transactions
GET /api/transactions/{id}
POST /api/transactions
PUT /api/transactions/{id}
DELETE /api/transactions/{id}


Optional filtering:


GET /api/transactions?categoryId=1


### Categories


GET /api/categories
GET /api/categories/{id}
POST /api/categories
PUT /api/categories/{id}
DELETE /api/categories/{id}


---

## Key Concepts

- DTO pattern for clean data transfer  
- Dependency Injection for loose coupling  
- Entity relationships (Transactions ↔ Categories)  
- Validation and error handling  
- Authorization using JWT tokens  

---

## Possible Improvements

- Transaction summaries (income vs expenses)
- Pagination
- Advanced filtering (date range, type)
- Frontend integration (React / Angular)

---

## License

This project is for educational purposes.

---

## Author

Created as a backend learning project using ASP.NET Core.