BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Meals]') AND [c].[name] = N'Ingredients');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Meals] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Meals] DROP COLUMN [Ingredients];
GO

CREATE TABLE [MealIngredientEntity] (
    [Id] bigint NOT NULL IDENTITY,
    [Quantity] real NOT NULL,
    [UnitType] int NOT NULL,
    [IngredientName] nvarchar(max) NULL,
    [MealId] bigint NOT NULL,
    [Created] datetime2 NOT NULL,
    CONSTRAINT [PK_MealIngredientEntity] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MealIngredientEntity_Meals_MealId] FOREIGN KEY ([MealId]) REFERENCES [Meals] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_MealIngredientEntity_MealId] ON [MealIngredientEntity] ([MealId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220801130240_Support Ingredient', N'5.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [IngredientLookups] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Created] datetime2 NOT NULL,
    CONSTRAINT [PK_IngredientLookups] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ShoppingCarts] (
    [Id] bigint NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [Created] datetime2 NOT NULL,
    CONSTRAINT [PK_ShoppingCarts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ShoppingCarts_MUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [MUser] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [ShoppingCartItems] (
    [Id] bigint NOT NULL IDENTITY,
    [Quantity] real NOT NULL,
    [ItemName] nvarchar(max) NULL,
    [UnitType] int NOT NULL,
    [IsBought] bit NOT NULL,
    [ShoppingCartId] bigint NOT NULL,
    [Created] datetime2 NOT NULL,
    CONSTRAINT [PK_ShoppingCartItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ShoppingCartItems_ShoppingCarts_ShoppingCartId] FOREIGN KEY ([ShoppingCartId]) REFERENCES [ShoppingCarts] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_ShoppingCartItems_ShoppingCartId] ON [ShoppingCartItems] ([ShoppingCartId]);
GO

CREATE INDEX [IX_ShoppingCarts_UserId] ON [ShoppingCarts] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220801160047_Support Shopping Cart', N'5.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[IngredientLookups]') AND [c].[name] = N'Name');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [IngredientLookups] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [IngredientLookups] ALTER COLUMN [Name] nvarchar(450) NULL;
GO

CREATE UNIQUE INDEX [IX_IngredientLookups_Name] ON [IngredientLookups] ([Name]) WHERE [Name] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220801171102_Support Ingredient add Index', N'5.0.11');
GO

COMMIT;
GO

