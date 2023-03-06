
-- Create the database
CREATE DATABASE FoodPantryDB;
GO
-- Use the database
USE FoodPantryDB;

-- Create the Categories table
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName VARCHAR(255) NOT NULL
);

-- Create the Items table
CREATE TABLE Items (
    ItemID INT PRIMARY KEY IDENTITY(1,1),
    ItemName VARCHAR(255) NOT NULL,
    ItemDescription VARCHAR(255) NOT NULL,
    CategoryID INT NOT NULL,
    Quantity DECIMAL NOT NULL,
    Unit VARCHAR(255) NOT NULL,
    Expiration DATE NOT NULL,
    CONSTRAINT FK_Items_Categories FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- Create the Purchases table
CREATE TABLE Purchases (
    PurchaseID INT PRIMARY KEY IDENTITY(1,1),
    PurchaseDate DATE NOT NULL,
    Quantity DECIMAL NOT NULL,
    ItemID INT NOT NULL,
    CONSTRAINT FK_Purchases_Items FOREIGN KEY (ItemID) REFERENCES Items(ItemID)
);

-- Create the Consumptions table
CREATE TABLE Consumptions (
    ConsumptionID INT PRIMARY KEY IDENTITY(1,1),
    ConsumptionDate DATE NOT NULL,
    Quantity DECIMAL NOT NULL,
    ItemID INT NOT NULL,
    CONSTRAINT FK_Consumptions_Items FOREIGN KEY (ItemID) REFERENCES Items(ItemID)
);

-- Create the Users table
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    UserName VARCHAR(255) NOT NULL,
    UserEmail VARCHAR(255) NOT NULL UNIQUE,
    UserPassword VARCHAR(255) NOT NULL
);

-- Create the User_Items table
CREATE TABLE User_Items (
    UserID INT NOT NULL,
    ItemID INT NOT NULL,
    PRIMARY KEY (UserID, ItemID),
    CONSTRAINT FK_User_Items_Users FOREIGN KEY (UserID) REFERENCES Users(UserID),
    CONSTRAINT FK_User_Items_Items FOREIGN KEY (ItemID) REFERENCES Items(ItemID)
);
GO
USE FoodPantryDB;
GO
CREATE USER FoodPantry FOR LOGIN FoodPantry WITH PASSWORD=N'PantryFood', DEFAULT_DATABASE=[FoodPantryDB], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF;
GO
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Categories TO FoodPantry;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Items TO FoodPantry;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Purchases TO FoodPantry;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Consumptions TO FoodPantry;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Users TO FoodPantry;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.User_Items TO FoodPantry;
GO
USE FoodPantryDB;
GO

IF OBJECT_ID('dbo.sp_resetPantryData', 'P') IS NOT NULL
BEGIN
    DROP PROCEDURE dbo.sp_resetPantryData;
END;
GO

CREATE PROCEDURE dbo.sp_resetPantryData

WITH EXECUTE AS OWNER
AS
BEGIN
    SET NOCOUNT ON;

    -- Delete all data from Items and Categories tables
    DELETE FROM Items;
    DELETE FROM Categories;

    -- Reset identity columns for Items and Categories tables
    DBCC CHECKIDENT ('Items', RESEED, 0);
    DBCC CHECKIDENT ('Categories', RESEED, 0);

    -- Populate Categories table
    INSERT INTO Categories (CategoryName)
    VALUES ('Canned Foods'), ('Bakery'), ('Dry Goods'), ('Beverages');

    -- Populate Items table
    INSERT INTO Items(ItemName, ItemDescription, CategoryID, Quantity, Unit, Expiration)
    VALUES 
    ('Canned Tomatoes', 'Whole peeled tomatoes', 1, 400, 'grams', '2021-03-01'),
    ('Bread', 'Whole wheat bread', 2, 500, 'grams', '2024-01-11'),
    ('Pasta', 'Spaghetti noodles', 3, 1000, 'grams', '2020-01-21'),
    ('Soda', 'Cola', 4, 355, 'milliliters', '2025-03-27'),
    ('Canned Peaches', 'Peaches in syrup', 1, 720, 'grams', '2023-11-07'),
    ('Bagels', 'Plain bagels', 2, 140, 'grams', '2022-04-08'),
    ('Rice', 'White rice', 3, 1000, 'grams', '2026-11-21'),
    ('Juice', 'Orange juice', 4, 1000, 'milliliters', '2021-09-07'),
    ('Canned Beans', 'Kidney beans', 1, 400, 'grams', '2030-03-04'),
    ('Muffins', 'Blueberry muffins', 2, 125, 'grams', '2023-05-01'),
    ('Cereal', 'Cornflakes', 3, 500, 'grams', '2026-03-11'),
    ('Milk', 'Whole milk', 4, 1000, 'milliliters', '2023-03-01'),
    ('Canned Corn', 'Sweet corn', 1, 340, 'grams','2025-04-05'),
    ('Cakes', 'Chocolate cakes', 2, 500, 'grams', '2024-01-06'),
    ('Noodles', 'Ramen noodles', 3, 500, 'grams', '2028-10-21'),
    ('Water', 'Mineral water', 4, 500, 'milliliters', '2027-04-11'),
    ('Peanut Butter', 'Smooth peanut butter', 3, 340, 'grams', '2026-03-02'),
    ('Crackers', 'Saltine crackers', 3, 250, 'grams', '2022-10-01'),
    ('Sugar', 'White granulated sugar', 3, 500, 'grams', '2032-07-01'),
    ('Tea', 'Black tea bags', 4, 100, 'grams', '2025-11-12');

END
GO
use FoodPantryDB;
GRANT EXECUTE ON dbo.sp_resetPantryData TO FoodPantry;
GO
