﻿CREATE DATABASE IF NOT EXISTS StockDb;

USE StockDb;

CREATE TABLE IF NOT EXISTS Product (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100) NOT NULL,
    PartNumber VARCHAR(50) NOT NULL,
    AverageCostPrice DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS Inventory (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    ProductId INT NOT NULL,
    AvailableQuantity INT NOT NULL DEFAULT 0,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (ProductId) REFERENCES Product(Id) ON DELETE RESTRICT
);

CREATE TABLE IF NOT EXISTS InventoryMovement (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    ProductId INT NOT NULL,
    MovementType ENUM('I', 'O') NOT NULL,
    Quantity INT NOT NULL,
    AverageCost DECIMAL(10, 2) NOT NULL,
    MovementDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ProductId) REFERENCES Product(Id) ON DELETE RESTRICT
);

CREATE TABLE IF NOT EXISTS ErrorLog (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ErrorMessage TEXT,
    StackTrace TEXT,
    CommandType VARCHAR(255),
    CreatedAt DATETIME DEFAULT NOW()
);