# Skills International School Management System

A Windows Forms (.NET Framework) application for managing student registrations at **Skills International** with a simple username/password based login.

The system persists data in a SQL Server database (`SkillInternationalDB`) using a small ADO.NET data-access layer, and is structured to be easy to understand and extend for learning or coursework.

---

## Features

- **User authentication**
  - Login form backed by the `Logins` table.
  - Centralised login validation in `DbHelper.ValidateLogin`.
- **Student registration management**
  - Registration form for creating, viewing, updating, and deleting student records.
  - Strongly-typed `StudentRecord` model mapped to the `Registration` table.
- **Database-backed storage**
  - SQL Server database `SkillInternationalDB` with `Logins` and `Registration` tables.
  - SQL scripts for schema creation and seed data.
- **Separation of concerns**
  - Forms focus on UI and validation.
  - All database access is centralised in `Database/DbHelper.cs`.
- **Documentation-first**
  - ERD and migration plan included under `Documentation/`.

---

## Tech Stack

- **Language:** C#
- **Framework:** .NET Framework 4.7.2
- **UI:** Windows Forms
- **Database:** Microsoft SQL Server (LocalDB / Express / full SQL Server)
- **Data access:** ADO.NET (`System.Data.SqlClient`) via a central `DbHelper` class
- **Testing:** Placeholder integration tests in `Tests/` (MSTest-style pattern)
- **Build tooling:** MSBuild (via the `.csproj` project file)
- **Optional CI:** GitHub Actions workflow (`.github/workflows/ci.yml`) if used in a GitHub repo

---

## Project Structure

```text
Skills International School Management System.slnx
Skills International School Management System.csproj
App.config
Program.cs
Login - Skills International.cs
Login - Skills International.Designer.cs
Login - Skills International.resx
RegistrationForm.cs
RegistrationForm.Designer.cs
RegistrationForm.resx

Asset/
  logo.png                # Application logo used by the WinForms UI

Database/
  DbHelper.cs             # Central ADO.NET data-access layer
  Models/
    LoginRecord.cs        # (If used) model for rows in [Logins]
    StudentRecord.cs      # Model for rows in [Registration]
  Scripts/
    01_CreateSchema.sql   # Creates SkillInternationalDB schema
    02_SeedData.sql       # Inserts initial data (e.g. default login)

Documentation/
  ERD.md                  # Detailed entity relationship diagram and notes
  MigrationPlan.md        # Migration and setup guidance, design decisions

Properties/
  AssemblyInfo.cs
  Resources.resx
  Resources.Designer.cs
  Settings.settings
  Settings.Designer.cs

Tests/
  DbHelperIntegrationTests.cs  # Placeholder for DbHelper integration tests

bin/
  Debug/
  Release/                # Build outputs

obj/
  ...                     # MSBuild intermediate files
```

> Note: Some build- and IDE-generated files (for example, under `obj/` and `bin/`) are omitted or abbreviated here.

---

## Database Design

The application targets a SQL Server database named **`SkillInternationalDB`**.

### Core Tables

- **Logins**
  - Stores system user credentials.
  - Columns (simplified): `Id`, `username`, `password`, `CreatedAt`.
  - `username` is unique.
- **Registration**
  - Stores student registration records.
  - Columns (simplified): `regNo`, `firstName`, `lastName`, `dateOfBirth`, `gender`, `address`, `mobilePhone`, `email`, `homePhone`, `parentName`, `nic`, `contactNo`, `CreatedAt`, `UpdatedAt`.
  - `regNo` is the primary key; `nic` is unique.

Additional notes on normalization, relationships, and design tradeoffs are documented in:

- `Documentation/ERD.md`
- `Documentation/MigrationPlan.md`

---

## Getting Started

### Prerequisites

- **Operating System:** Windows
- **IDE:** Visual Studio 2019/2022 (recommended) with **.NET desktop development** workload
- **Runtime:** .NET Framework 4.7.2 (developer pack)
- **Database:**
  - SQL Server Express or LocalDB (recommended for local development), **or**
  - A full SQL Server instance you can connect to.

### 1. Clone / open the project

1. Place the project folder on your machine (e.g. `d:\Project\Skills International`).
2. Open `Skills International School Management System.slnx` in Visual Studio.

### 2. Provision the database

You have two main options:

1. **Fresh local database (recommended for a clean setup)**
   - Create a database called `SkillInternationalDB` on your SQL Server / LocalDB instance.
   - Run `Database/Scripts/01_CreateSchema.sql` against that database.
   - Run `Database/Scripts/02_SeedData.sql` to insert initial data (including a default admin user if provided by the script).

2. **Migrate from an existing `Student` database**
   - If you are upgrading from an older version of the project, follow the detailed instructions in `Documentation/MigrationPlan.md` to migrate data from the legacy `Student` database to `SkillInternationalDB`.

### 3. Configure the connection string

1. Open `App.config`.
2. Locate the `<connectionStrings>` section and ensure there is an entry for the database.
3. Set the `Data Source` and other parts of the connection string so they match your local SQL Server / LocalDB environment.

`DbHelper.ConnectionString` reads the connection string from the application configuration (the deployed `.exe.config`), so no changes in code are required once the connection string is correct.

### 4. Build and run

1. In Visual Studio, select **Build → Rebuild Solution**.
2. Start the application with **F5** or **Debug → Start Debugging**.
3. The **Login** form should appear.
4. Sign in using a user created by your seed data (for example, a default admin account defined in `02_SeedData.sql`).

Once logged in, you should be able to open the **Registration** form and perform CRUD operations for student registrations.

---

## Testing

- The `Tests/DbHelperIntegrationTests.cs` file is a placeholder for ADO.NET / database integration tests around `DbHelper`.
- To fully enable automated tests:
  - Create a dedicated MSTest (or other framework) test project.
  - Link or reference `Database/DbHelper.cs` and the model classes.
  - Add your test methods there and configure them to point at a test database instance.

(At present, the repository structure is prepared for tests but does not ship with a fully configured test project or test runner configuration.)

---

## How It Works (High-Level)

- **Program entry point**
  - `Program.cs` configures WinForms and starts `Application.Run(new Login());`.
- **Login flow**
  - The login form calls `DbHelper.ValidateLogin(username, password)`.
  - ADO.NET executes a parameterised query against the `Logins` table.
- **Student registration flow**
  - The registration form binds UI controls to a `StudentRecord` instance.
  - Create/Update/Delete actions forward to `DbHelper.InsertStudent`, `DbHelper.UpdateStudent`, and `DbHelper.DeleteStudent`.
  - Reads use `DbHelper.GetByRegNo` and `DbHelper.GetAllRegNos`.
- **Data access**
  - `DbHelper` encapsulates all direct SQL calls.
  - Uses `SqlConnection`, `SqlCommand`, and `SqlDataReader` with parameterised queries to avoid SQL injection and keep SQL centralised.

---

## Extending the Project

Here are some ideas for future improvements:

- Replace plain-text password storage with a secure hashing algorithm (e.g., SHA-256 or bcrypt) and update `DbHelper.ValidateLogin` accordingly.
- Add search and filtering features to the registration UI.
- Introduce role-based access control (e.g., admin vs. staff).
- Add reporting (e.g., export to CSV, basic statistics).
- Split tests into a dedicated test project and wire them into CI.

---

## License / Usage

This project is intended primarily as a learning and demonstration application. Use, modify, and extend it according to your own course or organisational requirements.
