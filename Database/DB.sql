CREATE SEQUENCE [SalesOrderSeq] START WITH 1 INCREMENT BY 1 NO CYCLE;
GO

CREATE TABLE [Customers] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWSEQUENTIALID()),
    [Name] nvarchar(100) NOT NULL,
    [Phone] nvarchar(30) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [Address] nvarchar(100) NOT NULL,
    [CreatedAt] datetime2(0) NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] datetime2(0) NULL DEFAULT (GETDATE()),
    [IsDeleted] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [UpdatedBy] uniqueidentifier NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Products] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWSEQUENTIALID()),
    [Name] nvarchar(150) NOT NULL,
    [SKU] nvarchar(20) NOT NULL,
    [Description] nvarchar(1000) NOT NULL,
    [IsActive] bit NOT NULL DEFAULT 1,
    [UnitPrice] decimal(18,2) NOT NULL,
    [UnitOfMeasure] int NOT NULL DEFAULT 1,
    [LastCost] decimal(18,4) NOT NULL DEFAULT 0.0,
    [StandardCost] decimal(18,4) NOT NULL DEFAULT 0.0,
    [CreatedAt] datetime2(0) NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] datetime2(0) NULL DEFAULT (GETDATE()),
    [IsDeleted] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [UpdatedBy] uniqueidentifier NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Roles] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWSEQUENTIALID()),
    [Name] nvarchar(50) NOT NULL,
    [CreatedAt] datetime2(0) NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] datetime2(0) NULL DEFAULT (GETDATE()),
    [IsDeleted] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [UpdatedBy] uniqueidentifier NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Users] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWSEQUENTIALID()),
    [Name] nvarchar(50) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL DEFAULT 1,
    [CreatedAt] datetime2(0) NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] datetime2(0) NULL DEFAULT (GETDATE()),
    [IsDeleted] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [UpdatedBy] uniqueidentifier NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Warehouses] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWSEQUENTIALID()),
    [Name] nvarchar(100) NOT NULL,
    [Location] nvarchar(200) NOT NULL,
    [IsActive] bit NOT NULL DEFAULT 1,
    [CreatedAt] datetime2(0) NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] datetime2(0) NULL DEFAULT (GETDATE()),
    [IsDeleted] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [UpdatedBy] uniqueidentifier NULL,
    CONSTRAINT [PK_Warehouses] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [UserRoles] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWSEQUENTIALID()),
    [UserId] uniqueidentifier NOT NULL,
    [RoleId] uniqueidentifier NOT NULL,
    [CreatedAt] datetime2(0) NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] datetime2(0) NULL DEFAULT (GETDATE()),
    [IsDeleted] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [UpdatedBy] uniqueidentifier NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [InventoryAdjustment] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWSEQUENTIALID()),
    [ProductId] uniqueidentifier NOT NULL,
    [WarehouseId] uniqueidentifier NOT NULL,
    [AdjustmentDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [QuantityBefore] decimal(18,2) NOT NULL,
    [QuantityAdjusted] decimal(18,2) NOT NULL,
    [QuantityAfter] decimal(18,2) NOT NULL,
    [CostImpact] decimal(18,4) NOT NULL,
    [Reason] nvarchar(500) NOT NULL,
    [AdjustedByUserId] uniqueidentifier NULL,
    [CreatedAt] datetime2(0) NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] datetime2(0) NULL DEFAULT (GETDATE()),
    [IsDeleted] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [UpdatedBy] uniqueidentifier NULL,
    CONSTRAINT [PK_InventoryAdjustment] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InventoryAdjustment_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]),
    CONSTRAINT [FK_InventoryAdjustment_Users_AdjustedByUserId] FOREIGN KEY ([AdjustedByUserId]) REFERENCES [Users] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_InventoryAdjustment_Warehouses_WarehouseId] FOREIGN KEY ([WarehouseId]) REFERENCES [Warehouses] ([Id])
);
GO

CREATE TABLE [SalesOrders] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWSEQUENTIALID()),
    [CustomerId] uniqueidentifier NOT NULL,
    [WarehouseId] uniqueidentifier NOT NULL,
    [Status] int NOT NULL,
    [OrderNumber] nvarchar(30) NOT NULL DEFAULT ('SO-' + RIGHT('000000' + CAST(NEXT VALUE FOR SalesOrderSeq AS varchar(6)), 6)),
    [OrderDate] datetime2 NOT NULL DEFAULT (GETDATE()),
    [Total] decimal(18,2) NOT NULL,
    [TaxAmount] decimal(18,2) NOT NULL,
    [Discount] decimal(18,2) NOT NULL,
    [TotalCost] decimal(18,2) NOT NULL,
    [TotalProfit] decimal(18,2) NOT NULL,
    [CreatedAt] datetime2(0) NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] datetime2(0) NULL DEFAULT (GETDATE()),
    [IsDeleted] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [UpdatedBy] uniqueidentifier NULL,
    CONSTRAINT [PK_SalesOrders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SalesOrders_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]),
    CONSTRAINT [FK_SalesOrders_Warehouses_WarehouseId] FOREIGN KEY ([WarehouseId]) REFERENCES [Warehouses] ([Id])
);
GO

CREATE TABLE [Stocks] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWSEQUENTIALID()),
    [ProductId] uniqueidentifier NOT NULL,
    [WarehouseId] uniqueidentifier NOT NULL,
    [Quantity] decimal(18,2) NOT NULL DEFAULT 0.0,
    [ReservedQuantity] decimal(18,2) NOT NULL DEFAULT 0.0,
    [MinQuantityLevel] decimal(18,2) NOT NULL,
    [AvgCost] decimal(18,4) NOT NULL DEFAULT 0.0,
    [CreatedAt] datetime2(0) NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] datetime2(0) NULL DEFAULT (GETDATE()),
    [IsDeleted] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [UpdatedBy] uniqueidentifier NULL,
    CONSTRAINT [PK_Stocks] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Stocks_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]),
    CONSTRAINT [FK_Stocks_Warehouses_WarehouseId] FOREIGN KEY ([WarehouseId]) REFERENCES [Warehouses] ([Id])
);
GO

CREATE TABLE [StockTransactions] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWSEQUENTIALID()),
    [ProductId] uniqueidentifier NOT NULL,
    [WarehouseId] uniqueidentifier NOT NULL,
    [Type] int NOT NULL,
    [Source] int NOT NULL,
    [Quantity] decimal(18,2) NOT NULL,
    [UnitCost] decimal(18,4) NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    [TransactionDate] datetime2 NOT NULL DEFAULT (GETDATE()),
    [BalanceAfter] decimal(18,2) NOT NULL,
    [UserId] uniqueidentifier NULL,
    [ReferenceId] uniqueidentifier NULL,
    [ReferenceType] nvarchar(50) NOT NULL,
    [CreatedAt] datetime2(0) NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] datetime2(0) NULL DEFAULT (GETDATE()),
    [IsDeleted] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [UpdatedBy] uniqueidentifier NULL,
    CONSTRAINT [PK_StockTransactions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_StockTransactions_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]),
    CONSTRAINT [FK_StockTransactions_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]),
    CONSTRAINT [FK_StockTransactions_Warehouses_WarehouseId] FOREIGN KEY ([WarehouseId]) REFERENCES [Warehouses] ([Id])
);
GO

CREATE TABLE [SalesOrderItems] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWSEQUENTIALID()),
    [SalesOrderId] uniqueidentifier NOT NULL,
    [ProductId] uniqueidentifier NOT NULL,
    [WarehouseId] uniqueidentifier NOT NULL,
    [Quantity] decimal(18,2) NOT NULL,
    [UnitPriceAtSale] decimal(18,2) NOT NULL,
    [UnitCostAtSale] decimal(18,2) NOT NULL,
    [CreatedAt] datetime2(0) NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] datetime2(0) NULL DEFAULT (GETDATE()),
    [IsDeleted] bit NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [UpdatedBy] uniqueidentifier NULL,
    CONSTRAINT [PK_SalesOrderItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SalesOrderItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]),
    CONSTRAINT [FK_SalesOrderItems_SalesOrders_SalesOrderId] FOREIGN KEY ([SalesOrderId]) REFERENCES [SalesOrders] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_SalesOrderItems_Warehouses_WarehouseId] FOREIGN KEY ([WarehouseId]) REFERENCES [Warehouses] ([Id])
);
GO

CREATE UNIQUE INDEX [IX_Customers_Email] ON [Customers] ([Email]);
CREATE UNIQUE INDEX [IX_Customers_Phone] ON [Customers] ([Phone]);
CREATE INDEX [IX_Customers_IsDeleted] ON [Customers] ([IsDeleted]);

CREATE UNIQUE INDEX [IX_Products_SKU] ON [Products] ([SKU]);
CREATE INDEX [IX_Products_IsDeleted] ON [Products] ([IsDeleted]);
CREATE INDEX [IX_Products_IsActive_Name] ON [Products] ([IsActive], [Name]);

CREATE UNIQUE INDEX [IX_Roles_Name] ON [Roles] ([Name]);
CREATE INDEX [IX_Roles_IsDeleted] ON [Roles] ([IsDeleted]);

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
CREATE INDEX [IX_Users_IsDeleted] ON [Users] ([IsDeleted]);

CREATE UNIQUE INDEX [IX_UserRoles_UserId_RoleId] ON [UserRoles] ([UserId], [RoleId]);
CREATE INDEX [IX_UserRoles_IsDeleted] ON [UserRoles] ([IsDeleted]);

CREATE INDEX [IX_Warehouses_Name] ON [Warehouses] ([Name]);
CREATE INDEX [IX_Warehouses_IsDeleted] ON [Warehouses] ([IsDeleted]);

CREATE UNIQUE INDEX [IX_SalesOrders_OrderNumber] ON [SalesOrders] ([OrderNumber]);
CREATE INDEX [IX_SalesOrders_OrderDate_Status] ON [SalesOrders] ([OrderDate], [Status]);
CREATE INDEX [IX_SalesOrders_IsDeleted] ON [SalesOrders] ([IsDeleted]);

CREATE UNIQUE INDEX [IX_SalesOrderItems_SalesOrderId_ProductId] ON [SalesOrderItems] ([SalesOrderId], [ProductId]);
CREATE INDEX [IX_SalesOrderItems_IsDeleted] ON [SalesOrderItems] ([IsDeleted]);

CREATE UNIQUE INDEX [IX_Stocks_ProductId_WarehouseId] ON [Stocks] ([ProductId], [WarehouseId]);
CREATE INDEX [IX_Stocks_IsDeleted] ON [Stocks] ([IsDeleted]);

CREATE INDEX [IX_InventoryAdjustment_ProductId_WarehouseId_AdjustmentDate] ON [InventoryAdjustment] ([ProductId], [WarehouseId], [AdjustmentDate]);
CREATE INDEX [IX_InventoryAdjustment_IsDeleted] ON [InventoryAdjustment] ([IsDeleted]);

CREATE INDEX [IX_StockTransactions_ProductId_WarehouseId_TransactionDate] ON [StockTransactions] ([ProductId], [WarehouseId], [TransactionDate]);
CREATE INDEX [IX_StockTransactions_IsDeleted] ON [StockTransactions] ([IsDeleted]);
GO