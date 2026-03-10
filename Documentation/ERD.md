# Entity Relationship Diagram – SkillInternationalDB

## Tables

### Logins
Stores system user credentials for application authentication.

| Column    | Type          | Constraints                     |
|-----------|---------------|---------------------------------|
| Id        | INT IDENTITY  | PRIMARY KEY                     |
| username  | NVARCHAR(100) | NOT NULL, UNIQUE                |
| password  | NVARCHAR(256) | NOT NULL                        |
| CreatedAt | DATETIME      | NOT NULL, DEFAULT GETDATE()     |

### Registration
Stores student registration records.

| Column      | Type          | Constraints                     |
|-------------|---------------|---------------------------------|
| regNo       | INT IDENTITY  | PRIMARY KEY                     |
| firstName   | NVARCHAR(100) | NOT NULL                        |
| lastName    | NVARCHAR(100) |                                 |
| dateOfBirth | DATE          |                                 |
| gender      | NVARCHAR(10)  |                                 |
| address     | NVARCHAR(255) |                                 |
| mobilePhone | NVARCHAR(20)  |                                 |
| email       | NVARCHAR(100) |                                 |
| homePhone   | NVARCHAR(20)  |                                 |
| parentName  | NVARCHAR(100) |                                 |
| nic         | NVARCHAR(20)  | UNIQUE                          |
| contactNo   | NVARCHAR(20)  |                                 |
| CreatedAt   | DATETIME      | NOT NULL, DEFAULT GETDATE()     |
| UpdatedAt   | DATETIME      | NULL (set on every UPDATE)      |

---

## Relationships

These two tables are intentionally **independent**:

- `Logins` manages only system authentication.
- `Registration` manages student records.
- Any authenticated user may manage any student record.
- No foreign key between them is required for this scope.

```
+----------------------------+        +------------------------------+
|         Logins             |        |        Registration          |
+----------------------------+        +------------------------------+
| PK  Id            INT      |        | PK  regNo        INT         |
|     username (UQ) NVAR100  |        |     firstName    NVAR100     |
|     password      NVAR256  |        |     lastName     NVAR100     |
|     CreatedAt     DATETIME |        |     dateOfBirth  DATE        |
+----------------------------+        |     gender       NVAR10      |
                                      |     address      NVAR255     |
                                      |     mobilePhone  NVAR20      |
                                      |     email        NVAR100     |
                                      |     homePhone    NVAR20      |
                                      |     parentName   NVAR100     |
                                      |     nic (UQ)     NVAR20      |
                                      |     contactNo    NVAR20      |
                                      |     CreatedAt    DATETIME    |
                                      |     UpdatedAt    DATETIME    |
                                      +------------------------------+

No FK between the two tables (independent domains).
```

---

## Normalization Analysis

### 1NF ✔
- All columns are atomic and single-valued.
- Each table has a unique primary key (`Id`, `regNo`).

### 2NF ✔
- Both primary keys are single columns, so partial dependencies cannot exist.

### 3NF ✔
- **Logins**: `username`, `password`, `CreatedAt` all depend only on `Id`. No transitive dependency.
- **Registration**: every non-key column depends only on `regNo`. No column depends on another non-key column.

---

## Design Notes

| Decision | Rationale |
|----------|-----------|
| Phone columns use `NVARCHAR(20)` | Preserves leading zeros (e.g. `0712345678`). Original schema used `INT` which silently truncated them. |
| `UpdatedAt` on Registration | Enables audit trail without a separate history table. |
| Plain-text passwords | Acceptable for a school management MVP. Upgrade path: store a SHA-256 / bcrypt hash; see `MigrationPlan.md`. |
| No FK between tables | Login is authentication-only; no per-user data ownership is required at this stage. |
