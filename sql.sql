BEGIN TRANSACTION;
GO

CREATE TABLE [MealPlans] (
    [Id] bigint NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [Notes] nvarchar(max) NULL,
    [Created] datetime2 NOT NULL,
    CONSTRAINT [PK_MealPlans] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MealPlans_MUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [MUser] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [PlanDays] (
    [Id] bigint NOT NULL IDENTITY,
    [Day] int NOT NULL,
    [MealPlanId] bigint NOT NULL,
    [Created] datetime2 NOT NULL,
    CONSTRAINT [PK_PlanDays] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PlanDays_MealPlans_MealPlanId] FOREIGN KEY ([MealPlanId]) REFERENCES [MealPlans] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [PlanDayMenus] (
    [Id] bigint NOT NULL IDENTITY,
    [MealType] int NOT NULL,
    [PlanDayId] bigint NOT NULL,
    [Created] datetime2 NOT NULL,
    CONSTRAINT [PK_PlanDayMenus] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PlanDayMenus_PlanDays_PlanDayId] FOREIGN KEY ([PlanDayId]) REFERENCES [PlanDays] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [PlanDayMenuMeals] (
    [Id] bigint NOT NULL IDENTITY,
    [MealId] bigint NOT NULL,
    [PlanDayMenuId] bigint NOT NULL,
    [Created] datetime2 NOT NULL,
    CONSTRAINT [PK_PlanDayMenuMeals] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PlanDayMenuMeals_Meals_MealId] FOREIGN KEY ([MealId]) REFERENCES [Meals] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PlanDayMenuMeals_PlanDayMenus_PlanDayMenuId] FOREIGN KEY ([PlanDayMenuId]) REFERENCES [PlanDayMenus] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_MealPlans_UserId] ON [MealPlans] ([UserId]);
GO

CREATE INDEX [IX_PlanDayMenuMeals_MealId] ON [PlanDayMenuMeals] ([MealId]);
GO

CREATE INDEX [IX_PlanDayMenuMeals_PlanDayMenuId] ON [PlanDayMenuMeals] ([PlanDayMenuId]);
GO

CREATE INDEX [IX_PlanDayMenus_PlanDayId] ON [PlanDayMenus] ([PlanDayId]);
GO

CREATE INDEX [IX_PlanDays_MealPlanId] ON [PlanDays] ([MealPlanId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220728194631_Add Meal Plans', N'5.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [PlanDayMenus] ADD [Status] int NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220728205632_Add Meal Plan Day menu Status', N'5.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MUserRoles]') AND [c].[name] = N'CreatedBy');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [MUserRoles] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [MUserRoles] DROP COLUMN [CreatedBy];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MUserRoles]') AND [c].[name] = N'Description');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [MUserRoles] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [MUserRoles] DROP COLUMN [Description];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MUserRoles]') AND [c].[name] = N'IsActive');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [MUserRoles] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [MUserRoles] DROP COLUMN [IsActive];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MUserRoles]') AND [c].[name] = N'UpdatedBy');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [MUserRoles] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [MUserRoles] DROP COLUMN [UpdatedBy];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MUserRoles]') AND [c].[name] = N'UpdatedOn');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [MUserRoles] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [MUserRoles] DROP COLUMN [UpdatedOn];
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MRoles]') AND [c].[name] = N'CreatedBy');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [MRoles] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [MRoles] DROP COLUMN [CreatedBy];
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MRoles]') AND [c].[name] = N'Description');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [MRoles] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [MRoles] DROP COLUMN [Description];
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MRoles]') AND [c].[name] = N'IsActive');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [MRoles] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [MRoles] DROP COLUMN [IsActive];
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MRoles]') AND [c].[name] = N'UpdatedBy');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [MRoles] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [MRoles] DROP COLUMN [UpdatedBy];
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MRoles]') AND [c].[name] = N'UpdatedOn');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [MRoles] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [MRoles] DROP COLUMN [UpdatedOn];
GO

EXEC sp_rename N'[MUserRoles].[CreatedOn]', N'Created', N'COLUMN';
GO

EXEC sp_rename N'[MRoles].[CreatedOn]', N'Created', N'COLUMN';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220729143747_Fix Some Roles Issues', N'5.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [MealPlans] DROP CONSTRAINT [FK_MealPlans_MUser_UserId];
GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MealPlans]') AND [c].[name] = N'UserId');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [MealPlans] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [MealPlans] ALTER COLUMN [UserId] int NULL;
GO

ALTER TABLE [MealPlans] ADD [IsTemplate] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

ALTER TABLE [MealPlans] ADD [TemplateName] nvarchar(max) NULL;
GO

ALTER TABLE [MealPlans] ADD CONSTRAINT [FK_MealPlans_MUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [MUser] ([Id]) ON DELETE NO ACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220729165009_Support Plan Template', N'5.0.11');
GO

COMMIT;
GO

