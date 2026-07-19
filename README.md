# 🚆 Train Ticket Reservation System

A modern train ticket reservation and management system designed to simplify booking management, schedule organization, reporting, and travel planning. The application provides a complete workflow for creating, updating, managing, and analyzing train reservations through an intuitive web interface.

The system follows an enterprise application architecture using a React frontend, ASP.NET Core backend, Entity Framework Core, and SQLite database storage.

---

## ✨ Features

### 🎫 Booking Management
- Create new train ticket bookings.
- View existing reservations.
- Update booking details.
- Delete bookings.
- Search and filter bookings.
- Manage one-off and recurring bookings.
- Associate special requests with bookings.

### 🚉 Schedule Management
- Create and manage train schedules.
- Maintain travel dates, departure times, and arrival times.
- Link schedules with bookings.

### 📝 Special Request Management
- Add additional travel requirements.
- Manage requests such as:
  - Window seat preference.
  - Wheelchair assistance.
  - Extra luggage.
  - Custom passenger requirements.

### 📊 Reporting Dashboard
- Generate booking analytics.
- Filter reports by:
  - Date range.
  - Route.
  - Booking type.
- View:
  - Total bookings.
  - Total ticket expenditure.
  - Popular routes.
  - Special request statistics.
- Visualize route booking frequency.

### 📄 Report Export
- Export reports as:
  - CSV files.
  - PDF documents.

### 🤖 Prediction Support
- Analyze historical booking patterns.
- Provide availability and pricing trend predictions based on previous records.

---

# 🏗️ System Architecture

The application follows a layered enterprise architecture.

```
Frontend (React + TypeScript)
          |
          |
REST API
          |
          |
ASP.NET Core Web API
          |
          |
Service Layer
          |
          |
Repository Layer
          |
          |
Entity Framework Core
          |
          |
SQLite Database
```

---

# 🛠️ Technologies Used

## Frontend

- React
- TypeScript
- Tailwind CSS
- Axios
- Recharts
- html2canvas
- jsPDF

## Backend

- ASP.NET Core Web API
- C#
- Entity Framework Core
- Repository Pattern
- LINQ

## Database

- SQLite
- Entity Framework Core Migrations
- Database Seeding

## Development Tools

- Visual Studio Code
- Git
- Swagger API Documentation

---

# 📂 Project Structure

```
Train-Ticket-Reservation-System

│
├── frontend
│   ├── src
│   │   ├── components
│   │   ├── pages
│   │   ├── services
│   │   └── types
│   │
│   └── package.json
│
│
├── backend
│   └── TrainTicket.API
│       │
│       ├── Controllers
│       ├── Models
│       ├── Services
│       ├── Repositories
│       ├── Data
│       ├── DTOs
│       ├── Migrations
│       └── Program.cs
│
└── README.md
```

---

# 🗄️ Data Storage Architecture

The application uses **SQLite with Entity Framework Core** for persistent data storage.

This implementation uses a production-oriented persistence architecture to provide:

- Persistent data storage.
- Database integrity.
- Relationship management.
- Migration support.
- Scalability towards enterprise databases.

Entity Framework Core manages relationships between:

- Bookings.
- Routes.
- Schedules.
- Special requests.

---

# 🌱 Database Seeding

The project includes automatic database seeding.

During the first application startup:

- Sample routes are created.
- Example bookings are inserted.
- Schedule information is generated.
- Special requests are added.

The seed process runs only when the database is empty, preventing duplicate data creation.

This provides:

- Consistent testing data.
- Easier project demonstration.
- Reproducible development environments.

---

# 🚀 Running the Application

## Backend Setup

Navigate to:

```
backend/TrainTicket.API
```

Install dependencies:

```
dotnet restore
```

Apply database migrations:

```
dotnet ef database update
```

Run the API:

```
dotnet run
```

The API will start on:

```
https://localhost:xxxx
```

Swagger documentation is available at:

```
https://localhost:xxxx/swagger
```

---

## Frontend Setup

Navigate to:

```
frontend
```

Install dependencies:

```
npm install
```

Run the development server:

```
npm run dev
```

The frontend will be available at:

```
http://localhost:5173
```

---

# 🔄 Database Migration Commands

Create a new migration:

```
dotnet ef migrations add MigrationName
```

Apply migrations:

```
dotnet ef database update
```

Remove the latest migration:

```
dotnet ef migrations remove
```

---

# 📈 Design Principles Followed

The system follows:

- Object-oriented programming principles.
- Separation of concerns.
- Repository pattern.
- Service-oriented design.
- RESTful API principles.
- Entity relationship modelling.
- Component-based frontend architecture.

---

# 🔒 Data Integrity and Security

The system maintains data integrity through:

- Entity Framework Core relationships.
- Required field validation.
- Service-layer validation.
- Foreign key constraints.
- Parameterized database queries.

---

# 👨‍💻 Author

**Onaliy Vinukiy Jayawardana**

