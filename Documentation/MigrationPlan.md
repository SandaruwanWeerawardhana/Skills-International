# Migration Plan – SkillInternationalDB

## 1. Current State (before this migration)

| Area | Old approach | Problem |
|------|-------------|---------|
| Connection string | Hard-coded in each form (`DESKTOP-T8RBHGC\SQLEXPRESS`) | Breaks on any other machine; two copies to maintain |
| SQL code | Inline ADO.NET in every button click handler | No separation of concerns; untestable |
| Phone columns | Stored as `INT` via `int.TryParse` | Silently drops leading zeros; rejects non-numeric input |
| Database name | `Student` | Generic; conflicts with default SQL Server demos |
| Tests | None | Regressions invisible until manual smoke-test |

---

## 2. Target State (this migration)

| Area | New approach |
|------|-------------|
| Connection string | Single entry `SkillInternationalDB` in `App.config` |
| SQL code | Centralised in `Database\DbHelper.cs` (ADO.NET DAL) |
| Models | `Database\Models\StudentRecord.cs`, `LoginRecord.cs` |
| Phone columns | `NVARCHAR(20)` — leading zeros preserved |
| Database name | `SkillInternationalDB` (provisioned by SQL scripts) |
| Tests | MSTest integration test project (`Tests\`) |
| CI | GitHub Actions workflow (`.github\workflows\ci.yml`) |

---

## 3. Step-by-Step Migration Instructions

### 3.1 Provision a fresh database (recommended for new machines)

```powershell
# From solution root – requires sqlcmd in PATH
.\Database\Scripts\Provision-LocalDb.ps1
```

Default login seeded: **admin / admin123**

### 3.2 Migrate an existing `Student` database

If you have existing data in the old `Student` database, run this once:

```sql
-- Run against your old SQL Express instance
USE [Student];

-- Export students to new schema (adjust server/linked-server as needed)
INSERT INTO [SkillInternationalDB].[dbo].[Registration]
    ([firstName],[lastName],[dateOfBirth],[gender],[address],
     [mobilePhone],[email],[homePhone],[parentName],[nic],[contactNo])
SELECT
    [firstName],
    [lastName],
    [dateOfBirth],
    [gender],
    [address],
    -- Cast INT phone columns back to string, re-adding leading zeros is manual
    CAST([mobilePhone] AS NVARCHAR(20)),
    [email],
    CAST([homePhone]  AS NVARCHAR(20)),
    [parentName],
    [nic],
    CAST([contactNo]  AS NVARCHAR(20))
FROM [dbo].[Registration];

INSERT INTO [SkillInternationalDB].[dbo].[Logins] ([username],[password])
SELECT [username],[password] FROM [dbo].[Logins];
```

> **Note:** Phone numbers stored as INT have lost leading zeros. You must correct
> them manually after migration (e.g. `UPDATE Registration SET mobilePhone = '0' + mobilePhone WHERE LEN(mobilePhone) = 9`).

### 3.3 Update `App.config` for your environment

Open `App.config` and adjust `Data Source` to match your SQL instance:

| Scenario | Data Source value |
|----------|------------------|
| LocalDB (default, dev) | `(localdb)\MSSQLLocalDB` |
| SQL Server Express (named instance) | `DESKTOP-T8RBHGC\SQLEXPRESS` |
| Full SQL Server | `.\SQLEXPRESS` or `servername` |

### 3.4 Build and run

1. Open solution in Visual Studio 2022.
2. Build → Rebuild Solution (`Ctrl+Shift+B`).
3. Run (`F5`) – the Login form appears; use `admin / admin123`.

### 3.5 Run integration tests

```powershell
# Provision DB first (see 3.1), then:
dotnet test Tests\SkillInternationalTests.csproj --logger "console;verbosity=normal"
```

---

## 4. EF Core vs ADO.NET DAL – Pros and Cons

### Option A – Keep ADO.NET DAL (chosen for this migration)

| Pros | Cons |
|------|------|
| Zero new NuGet dependencies | More boilerplate SQL per operation |
| Full control over every query | No change-tracking or migrations |
| No runtime overhead of ORM | Developer must write SQL manually |
| Compatible with .NET Framework 4.7.2 out of the box | |
| Easiest to explain and maintain in a student project | |

### Option B – Migrate to EF Core 6 / 8

| Pros | Cons |
|------|------|
| Strongly-typed LINQ queries | Requires .NET 6+ (project retarget needed) |
| Automatic migrations (`dotnet ef migrations add`) | Adds ~5 NuGet packages |
| Change-tracking simplifies update logic | EF Core 6+ does not support .NET Framework |
| Scaffolding generates model classes from DB | Learning curve for EF Core conventions |

**Recommendation:** Stay with the ADO.NET DAL for this project. Migrate to EF Core
only if/when the project is re-targeted to .NET 8+ and the team is comfortable with
the EF Core migration workflow.

### Safe EF Core migration path (future reference)

1. Retarget project to `net8.0-windows` in `.csproj`.
2. Add packages: `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Tools`.
3. Scaffold: `dotnet ef dbcontext scaffold "..." Microsoft.EntityFrameworkCore.SqlServer --output-dir Data`.
4. Replace `DbHelper` calls with `DbContext` calls.
5. Add `dotnet ef migrations add InitialCreate` and `dotnet ef database update`.
6. Update CI to run `dotnet ef database update` instead of `sqlcmd`.

---

## 5. Rollback Plan

If the migration causes issues, revert by:

1. Restoring the original form files from Git: `git checkout HEAD~1 -- "RegistrationForm.cs" "Login - Skills International.cs"`.
2. Changing `App.config` `Data Source` back to `DESKTOP-T8RBHGC\SQLEXPRESS` and `Initial Catalog` back to `Student`.
3. The old `Student` database is untouched by this migration.
