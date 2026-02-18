
CREATE TABLE [Users] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    
    [FirstName] NVARCHAR(100) NOT NULL,
    [LastName] NVARCHAR(100) NOT NULL,
    [Username] NVARCHAR(256) NOT NULL,
    [NormalizedUserName] NVARCHAR(256) NOT NULL,
    [Email] NVARCHAR(256) NOT NULL,
    [NormalizedEmail] NVARCHAR(256) NOT NULL,
    [PasswordHash] NVARCHAR(MAX) NOT NULL,
    [PasswordSalt] NVARCHAR(MAX) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Roles] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    
    [Name] NVARCHAR(256) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Customers] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    
    [Name] NVARCHAR(200) NOT NULL,
    [Phone] NVARCHAR(50) NULL,
    [Email] NVARCHAR(256) NULL,
    [Address] NVARCHAR(MAX) NULL,
    
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Warehouses] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    
    [Name] NVARCHAR(200) NOT NULL,
    [Location] NVARCHAR(MAX) NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    
    CONSTRAINT [PK_Warehouses] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Products] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    
    [Name] NVARCHAR(200) NOT NULL,
    [SKU] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [AVGUnitCost] DECIMAL(18, 4) NOT NULL DEFAULT 0,
    [UnitPrice] DECIMAL(18, 4) NOT NULL DEFAULT 0,
    [UnitOfMeasure] INT NOT NULL,
    
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [UserRoles] (
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [RoleId] UNIQUEIDENTIFIER NOT NULL,
    
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY ([RoleId]) REFERENCES [Roles]([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [RefreshTokens] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [TokenHash] NVARCHAR(MAX) NOT NULL,
    [CreatedOn] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ExpiresOn] DATETIME2 NOT NULL,
    [IsUsed] BIT NOT NULL DEFAULT 0,
    [IsRevoked] BIT NOT NULL DEFAULT 0,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    
    CONSTRAINT [PK_RefreshTokens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RefreshTokens_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Stocks] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    
    [ProductId] UNIQUEIDENTIFIER NOT NULL,
    [WarehouseId] UNIQUEIDENTIFIER NOT NULL,
    [Quantity] DECIMAL(18, 4) NOT NULL DEFAULT 0,
    [ReservedQuantity] DECIMAL(18, 4) NOT NULL DEFAULT 0,
    [AvgCost] DECIMAL(18, 4) NOT NULL DEFAULT 0,
    
    CONSTRAINT [PK_Stocks] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Stocks_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Stocks_Warehouses] FOREIGN KEY ([WarehouseId]) REFERENCES [Warehouses]([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [StockTransactions] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    
    [ProductId] UNIQUEIDENTIFIER NOT NULL,
    [WarehouseId] UNIQUEIDENTIFIER NOT NULL,
    [Type] INT NOT NULL,
    [Source] INT NOT NULL,
    [Quantity] DECIMAL(18, 4) NOT NULL,
    [UnitCost] DECIMAL(18, 4) NOT NULL,
    [TransactionDate] DATETIME2 NOT NULL,
    [BalanceAfter] DECIMAL(18, 4) NOT NULL,
    [UserId] UNIQUEIDENTIFIER NULL,
    [ReferenceId] UNIQUEIDENTIFIER NULL,
    [ReferenceType] NVARCHAR(100) NULL,
    
    CONSTRAINT [PK_StockTransactions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_StockTransactions_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]),
    CONSTRAINT [FK_StockTransactions_Warehouses] FOREIGN KEY ([WarehouseId]) REFERENCES [Warehouses]([Id])
);
GO

CREATE TABLE [InventoryAdjustments] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    
    [ProductId] UNIQUEIDENTIFIER NOT NULL,
    [WarehouseId] UNIQUEIDENTIFIER NOT NULL,
    [AdjustmentDate] DATETIME2 NOT NULL,
    [QuantityBefore] DECIMAL(18, 4) NOT NULL,
    [QuantityAdjusted] DECIMAL(18, 4) NOT NULL,
    [QuantityAfter] DECIMAL(18, 4) NOT NULL,
    [CostImpact] DECIMAL(18, 4) NOT NULL,
    [Reason] NVARCHAR(MAX) NULL,
    [AdjustedByUserId] UNIQUEIDENTIFIER NULL,
    
    CONSTRAINT [PK_InventoryAdjustments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InventoryAdjustments_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]),
    CONSTRAINT [FK_InventoryAdjustments_Warehouses] FOREIGN KEY ([WarehouseId]) REFERENCES [Warehouses]([Id])
);
GO

CREATE TABLE [SalesOrders] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    
    [CustomerId] UNIQUEIDENTIFIER NOT NULL,
    [Status] INT NOT NULL,
    [OrderNumber] NVARCHAR(50) NOT NULL,
    [OrderDate] DATETIME2 NOT NULL,
    [TotalPrice] DECIMAL(18, 4) NOT NULL DEFAULT 0,
    [TotalCost] DECIMAL(18, 4) NOT NULL DEFAULT 0,
    
    CONSTRAINT [PK_SalesOrders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SalesOrders_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [Customers]([Id])
);
GO

-- Table: SalesOrderItems
CREATE TABLE [SalesOrderItems] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    
    [ProductId] UNIQUEIDENTIFIER NOT NULL,
    [SalesOrderId] UNIQUEIDENTIFIER NOT NULL,
    [Quantity] DECIMAL(18, 4) NOT NULL,
    [UnitPriceAtSale] DECIMAL(18, 4) NOT NULL,
    [UnitCostAtSale] DECIMAL(18, 4) NOT NULL,
    
    CONSTRAINT [PK_SalesOrderItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SalesOrderItems_SalesOrders] FOREIGN KEY ([SalesOrderId]) REFERENCES [SalesOrders]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_SalesOrderItems_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id])
);
GO

CREATE TABLE [ReservationRequests] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    
    [OrderId] UNIQUEIDENTIFIER NOT NULL,
    [ProductId] UNIQUEIDENTIFIER NOT NULL,
    [WarehouseId] UNIQUEIDENTIFIER NOT NULL,
    [Quantity] DECIMAL(18, 4) NOT NULL,
    [Status] INT NOT NULL,
    
    CONSTRAINT [PK_ReservationRequests] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ReservationRequests_SalesOrders] FOREIGN KEY ([OrderId]) REFERENCES [SalesOrders]([Id]) ON DELETE CASCADE, -- Assuming request dies with order
    CONSTRAINT [FK_ReservationRequests_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]),
    CONSTRAINT [FK_ReservationRequests_Warehouses] FOREIGN KEY ([WarehouseId]) REFERENCES [Warehouses]([Id])
);
GO

-- Table: ReturnedItems
CREATE TABLE [ReturnedItems] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    -- No BaseEntity fields here per class definition
    [ProductId] UNIQUEIDENTIFIER NOT NULL,
    [Quantity] DECIMAL(18, 4) NOT NULL,
    [Reason] NVARCHAR(MAX) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [Source] NVARCHAR(100) NULL,
    
    CONSTRAINT [PK_ReturnedItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ReturnedItems_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id])
);
GO


CREATE UNIQUE INDEX [IX_Users_Username] ON [Users]([NormalizedUserName]) WHERE [IsDeleted] = 0;
CREATE UNIQUE INDEX [IX_Users_Email] ON [Users]([NormalizedEmail]) WHERE [IsDeleted] = 0;

CREATE UNIQUE INDEX [IX_Products_SKU] ON [Products]([SKU]) WHERE [IsDeleted] = 0;

CREATE UNIQUE INDEX [IX_Stocks_Product_Warehouse] ON [Stocks]([ProductId], [WarehouseId]) WHERE [IsDeleted] = 0;

CREATE UNIQUE INDEX [IX_SalesOrders_OrderNumber] ON [SalesOrders]([OrderNumber]) WHERE [IsDeleted] = 0;

CREATE INDEX [IX_SalesOrderItems_SalesOrderId] ON [SalesOrderItems]([SalesOrderId]);
CREATE INDEX [IX_SalesOrderItems_ProductId] ON [SalesOrderItems]([ProductId]);
CREATE INDEX [IX_StockTransactions_ProductId] ON [StockTransactions]([ProductId]);
CREATE INDEX [IX_StockTransactions_WarehouseId] ON [StockTransactions]([WarehouseId]);
CREATE INDEX [IX_StockTransactions_TransactionDate] ON [StockTransactions]([TransactionDate]);
GO