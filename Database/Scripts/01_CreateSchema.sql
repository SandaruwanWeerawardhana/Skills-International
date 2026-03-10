
USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'Student')
BEGIN
    CREATE DATABASE [Student];
    PRINT 'Database Student created.';
END
ELSE
    PRINT 'Database Student already exists – schema will be updated if needed.';
GO

USE [Student];
GO

-- -------------------------------------------------------------
-- Table: Logins
-- Stores system user credentials for authentication.
-- -------------------------------------------------------------
IF NOT EXISTS (
    SELECT * FROM sys.objects
    WHERE object_id = OBJECT_ID(N'[dbo].[Logins]') AND type = N'U')
BEGIN
    CREATE TABLE [dbo].[Logins] (
        [Id]        INT            IDENTITY(1,1)  NOT NULL,
        [username]  VARCHAR(20)  NOT NULL,
        [password]  VARCHAR(20)  NOT NULL,

        CONSTRAINT [PK_Logins]          PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [UQ_Logins_Username] UNIQUE ([username])
    );
    PRINT 'Table [Logins] created.';
END
ELSE
    PRINT 'Table [Logins] already exists.';
GO

-- -------------------------------------------------------------
-- Table: Registration
-- Stores student registration records.
-- Phone / contact numbers use NVARCHAR(20) to preserve
-- leading zeros (e.g. 0712345678).
-- -------------------------------------------------------------
IF NOT EXISTS (
    SELECT * FROM sys.objects
    WHERE object_id = OBJECT_ID(N'[dbo].[Registration]') AND type = N'U')
BEGIN
    CREATE TABLE [dbo].[Registration] (
        [regNo]       INT            IDENTITY(1,1)  NOT NULL,
        [firstName]   VARCHAR(20)  NOT NULL,
        [lastName]    VARCHAR(20)  NULL,
        [dateOfBirth] DATE           NULL,
        [gender]      VARCHAR(6)   NULL,
        [address]     VARCHAR(50)  NULL,
        [mobilePhone] INT   NULL,
        [email]       VARCHAR(30)  NULL,
        [homePhone]   INT   NULL,
        [parentName]  VARCHAR(50)  NULL,
        [nic]         VARCHAR(20)   NULL,
        [contactNo]   INT   NULL,
        [CreatedAt]   DATETIME       NOT NULL
            CONSTRAINT [DF_Registration_CreatedAt] DEFAULT GETDATE(),
        [UpdatedAt]   DATETIME       NULL,

        CONSTRAINT [PK_Registration]     PRIMARY KEY CLUSTERED ([regNo] ASC),
        CONSTRAINT [UQ_Registration_NIC] UNIQUE ([nic])
    );
    PRINT 'Table [Registration] created.';
END
ELSE
    PRINT 'Table [Registration] already exists.';
GO

PRINT 'Schema provisioning complete.';
GO
