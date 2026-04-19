IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260227022712_InitialCommit'
)
BEGIN
    IF SCHEMA_ID(N'Customer') IS NULL EXEC(N'CREATE SCHEMA [Customer];');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260227022712_InitialCommit'
)
BEGIN
    CREATE TABLE [Customer].[Customer] (
        [CustomerId] uniqueidentifier NOT NULL,
        [Name] varchar(80) NOT NULL,
        [LastName] varchar(80) NOT NULL,
        [DocumentNumber] varchar(20) NOT NULL,
        [DocumentType] varchar(12) NOT NULL,
        [Email] varchar(100) NULL,
        [PhoneNumber] varchar(50) NULL,
        [IsMailVerified] bit NOT NULL,
        [IsPhoneVerified] bit NOT NULL,
        [BirthDate] date NULL,
        [Gender] varchar(10) NULL,
        [Status] varchar(20) NOT NULL,
        [LastModifiedBy] varchar(255) NULL,
        [LastModifiedAt] datetime NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedBy] varchar(255) NULL,
        [DeletedAt] datetime NULL,
        CONSTRAINT [PK_Customer] PRIMARY KEY ([CustomerId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260227022712_InitialCommit'
)
BEGIN
    CREATE TABLE [Customer].[CustomerAddress] (
        [CustomerAddressId] uniqueidentifier NOT NULL,
        [CustomerId] uniqueidentifier NOT NULL,
        [Label] varchar(30) NOT NULL,
        [Street] varchar(100) NOT NULL,
        [District] varchar(50) NOT NULL,
        [Province] varchar(50) NOT NULL,
        [Departament] varchar(20) NOT NULL,
        [Reference] varchar(50) NULL,
        [IsDefault] bit NOT NULL,
        [LastModifiedBy] varchar(255) NULL,
        [LastModifiedAt] datetime NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedBy] varchar(255) NULL,
        [DeletedAt] datetime NULL,
        CONSTRAINT [PK_CustomerAddress] PRIMARY KEY ([CustomerAddressId]),
        CONSTRAINT [FK_CustomerAddress_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer].[Customer] ([CustomerId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260227022712_InitialCommit'
)
BEGIN
    CREATE INDEX [IX_CustomerAddress_CustomerId] ON [Customer].[CustomerAddress] ([CustomerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260227022712_InitialCommit'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260227022712_InitialCommit', N'10.0.3');
END;

COMMIT;
GO

