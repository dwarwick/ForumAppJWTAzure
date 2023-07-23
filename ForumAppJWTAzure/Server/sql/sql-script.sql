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
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [DisplayName] nvarchar(max) NOT NULL,
        [Theme] nvarchar(max) NOT NULL,
        [ProfilePicture] nvarchar(max) NULL,
        [CreatedDate] datetime2 NULL,
        [ModifiedDate] datetime2 NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE TABLE [Forums] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(max) NULL,
        [CreatedById] nvarchar(450) NULL,
        [CreatedDate] datetime2 NOT NULL,
        [ModifiedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Forums] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Forums_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE TABLE [Tags] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [CreatedById] nvarchar(450) NULL,
        [CreatedDate] datetime2 NOT NULL,
        [ModifiedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Tags] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Tags_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE TABLE [Posts] (
        [Id] int NOT NULL IDENTITY,
        [Text] nvarchar(max) NULL,
        [ForumId] int NULL,
        [ReplyPostId] int NULL,
        [CreatedById] nvarchar(450) NULL,
        [CreatedDate] datetime2 NOT NULL,
        [ModifiedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Posts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Posts_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_Posts_Forums_ForumId] FOREIGN KEY ([ForumId]) REFERENCES [Forums] ([Id]),
        CONSTRAINT [FK_Posts_Posts_ReplyPostId] FOREIGN KEY ([ReplyPostId]) REFERENCES [Posts] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE TABLE [ForumTags] (
        [ForumId] int NOT NULL,
        [TagId] int NOT NULL,
        CONSTRAINT [PK_ForumTags] PRIMARY KEY ([ForumId], [TagId]),
        CONSTRAINT [FK_ForumTags_Forums_ForumId] FOREIGN KEY ([ForumId]) REFERENCES [Forums] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ForumTags_Tags_TagId] FOREIGN KEY ([TagId]) REFERENCES [Tags] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE TABLE [Votes] (
        [Id] int NOT NULL IDENTITY,
        [Value] int NOT NULL,
        [PostId] int NOT NULL,
        [CreatedById] nvarchar(450) NULL,
        [CreatedDate] datetime2 NOT NULL,
        [ModifiedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Votes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Votes_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_Votes_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [Posts] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] ON;
    EXEC(N'INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
    VALUES (N''8343074e-8623-4e1a-b0c1-84fb8678c8f3'', NULL, N''User'', N''USER''),
    (N''c7ac6cfe-1f10-4baf-b604-cde350db9554'', NULL, N''Administrator'', N''ADMINISTRATOR'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'CreatedDate', N'DisplayName', N'Email', N'EmailConfirmed', N'LockoutEnabled', N'LockoutEnd', N'ModifiedDate', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'ProfilePicture', N'SecurityStamp', N'Theme', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
        SET IDENTITY_INSERT [AspNetUsers] ON;
    EXEC(N'INSERT INTO [AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [CreatedDate], [DisplayName], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [ModifiedDate], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [ProfilePicture], [SecurityStamp], [Theme], [TwoFactorEnabled], [UserName])
    VALUES (N''30a24107-d279-4e37-96fd-01af5b38cb27'', 0, N''6395b5ad-b8bb-4a49-90d7-bda4bcddadb3'', ''2023-07-22T16:17:36.3277174Z'', N''User'', N''user@bookstore.com'', CAST(0 AS bit), CAST(0 AS bit), NULL, ''2023-07-22T16:17:36.2674357Z'', N''USER@BOOKSTORE.COM'', N''USER@BOOKSTORE.COM'', N''AQAAAAIAAYagAAAAEDFYg21GR7B5GL4cnrLpmp8tjijUHPr0sEQmkFOXtXnk8K9WKbbWc4yKwmUJRkrE5w=='', NULL, CAST(0 AS bit), NULL, N''69883287-6544-4a85-aff7-87330be96229'', N''Dark'', CAST(0 AS bit), N''user@bookstore.com''),
    (N''8e448afa-f008-446e-a52f-13c449803c2e'', 0, N''38d403e7-8800-472d-9aa7-75a6ca60348b'', ''2023-07-22T16:17:36.2674061Z'', N''Admin'', N''admin@bookstore.com'', CAST(0 AS bit), CAST(0 AS bit), NULL, ''2023-07-22T16:17:36.2080450Z'', N''ADMIN@BOOKSTORE.COM'', N''ADMIN@BOOKSTORE.COM'', N''AQAAAAIAAYagAAAAEHZ+5IArqJK5visisTOf29SvLI9wt0CHnJtpauG/Z5+ZbqaVvALxeLtFNfCQ4q62uA=='', NULL, CAST(0 AS bit), NULL, N''1a092eae-090d-4ea2-8cba-2a9b7cffde2e'', N''Dark'', CAST(0 AS bit), N''admin@bookstore.com'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'CreatedDate', N'DisplayName', N'Email', N'EmailConfirmed', N'LockoutEnabled', N'LockoutEnd', N'ModifiedDate', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'ProfilePicture', N'SecurityStamp', N'Theme', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
        SET IDENTITY_INSERT [AspNetUsers] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserRoles]'))
        SET IDENTITY_INSERT [AspNetUserRoles] ON;
    EXEC(N'INSERT INTO [AspNetUserRoles] ([RoleId], [UserId])
    VALUES (N''8343074e-8623-4e1a-b0c1-84fb8678c8f3'', N''30a24107-d279-4e37-96fd-01af5b38cb27''),
    (N''c7ac6cfe-1f10-4baf-b604-cde350db9554'', N''8e448afa-f008-446e-a52f-13c449803c2e'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserRoles]'))
        SET IDENTITY_INSERT [AspNetUserRoles] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE INDEX [IX_Forums_CreatedById] ON [Forums] ([CreatedById]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE INDEX [IX_ForumTags_TagId] ON [ForumTags] ([TagId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE INDEX [IX_Posts_CreatedById] ON [Posts] ([CreatedById]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE INDEX [IX_Posts_ForumId] ON [Posts] ([ForumId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE INDEX [IX_Posts_ReplyPostId] ON [Posts] ([ReplyPostId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE INDEX [IX_Tags_CreatedById] ON [Tags] ([CreatedById]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE INDEX [IX_Votes_CreatedById] ON [Votes] ([CreatedById]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    CREATE INDEX [IX_Votes_PostId] ON [Votes] ([PostId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230722161736_InitialMigration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230722161736_InitialMigration', N'7.0.9');
END;
GO

COMMIT;
GO

