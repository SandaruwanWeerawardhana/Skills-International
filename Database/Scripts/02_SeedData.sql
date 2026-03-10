-- =============================================================
-- 02_SeedData.sql
-- Inserts default admin login and two sample student records.
-- Run AFTER 01_CreateSchema.sql.
-- WARNING: Passwords are plain-text for development only.
--          Hash passwords before deploying to production.
-- =============================================================

USE [Student];
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Logins] WHERE [username] = N'admin')
BEGIN
    INSERT INTO [dbo].[Logins] ([username], [password])
    VALUES (N'Admin', N'Skills@123');
    PRINT 'Admin user seeded.';
END
ELSE
    PRINT 'Admin user already exists – skipping.';
GO

-- Sample student 1
IF NOT EXISTS (SELECT 1 FROM [dbo].[Registration] WHERE [nic] = N'200012345678')
BEGIN
    INSERT INTO [dbo].[Registration]
        ([firstName],[lastName],[dateOfBirth],[gender],[address],
         [mobilePhone],[email],[homePhone],[parentName],[nic],[contactNo])
    VALUES
        (N'Kamal', N'Perera', '2000-03-15', N'Male',
         N'123 Galle Road, Colombo 03',
         N'0712345678', N'kamal.perera@email.com', N'0112345678',
         N'Sunil Perera', N'200012345678', N'0712345678');
    PRINT 'Sample student Kamal Perera seeded.';
END
ELSE
    PRINT 'Sample student 1 already exists – skipping.';
GO

-- Sample student 2
IF NOT EXISTS (SELECT 1 FROM [dbo].[Registration] WHERE [nic] = N'200198765432')
BEGIN
    INSERT INTO [dbo].[Registration]
        ([firstName],[lastName],[dateOfBirth],[gender],[address],
         [mobilePhone],[email],[homePhone],[parentName],[nic],[contactNo])
    VALUES
        (N'Nimali', N'Silva', '2001-07-22', N'Female',
         N'45 Kandy Road, Nugegoda',
         N'0778765432', N'nimali.silva@email.com', N'0118765432',
         N'Rohini Silva', N'200198765432', N'0778765432');
    PRINT 'Sample student Nimali Silva seeded.';
END
ELSE
    PRINT 'Sample student 2 already exists – skipping.';
GO

PRINT 'Seed data complete.';
GO
