/* Items: This table stores information about each item in the pantry, including the item name, description, 
category, quantity, and unit of measurement. The ItemID column is set as the primary key and is set to auto-increment. 
There is also a foreign key constraint to link the items to their respective categories.

Categories: This table stores information about the categories of items in the pantry, including 
the category name. The CategoryID column is set as the primary key and is set to auto-increment.

Purchases: This table stores information about each purchase of an item in the pantry, including 
the purchase date, location, quantity, and item ID. The PurchaseID column is set as the primary 
key and is set to auto-increment. There is also a foreign key constraint to link the purchases 
to the items they correspond to.

Consumptions: This table stores information about each consumption of an item in the 
pantry, including the consumption date, quantity, and item ID. The ConsumptionID column is 
set as the primary key and is set to auto-increment. There is also a foreign key constraint 
to link the consumptions to the items they correspond to.

Users: This table stores information about each user of the pantry inventory app, including the user 
name, email and password. The UserID column is set as the primary key and is set to auto-increment.

User_Items: This table stores a many-to-many relationship between users and items. There are two columns 
in the table that are foreign keys to the Users and Items tables.
*/

-- Create Categories table
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName VARCHAR(255) NOT NULL
);

-- Create Items table
CREATE TABLE Items (
    ItemID INT PRIMARY KEY IDENTITY(1,1),
    ItemName VARCHAR(255) NOT NULL,
    ItemDescription VARCHAR(255) NOT NULL,
    CategoryID INT NOT NULL,
    Quantity INT NOT NULL,
    Unit VARCHAR(255) NOT NULL,
    CONSTRAINT FK_Items_Categories FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- Create Purchases table
CREATE TABLE Purchases (
    PurchaseID INT PRIMARY KEY IDENTITY(1,1),
    PurchaseDate DATE NOT NULL,
    PurchaseLocation VARCHAR(255) NOT NULL,
    Quantity INT NOT NULL,
    ItemID INT NOT NULL,
    CONSTRAINT FK_Purchases_Items FOREIGN KEY (ItemID) REFERENCES Items(ItemID)
);

-- Create Consumptions table
CREATE TABLE Consumptions (
    ConsumptionID INT PRIMARY KEY IDENTITY(1,1),
    ConsumptionDate DATE NOT NULL,
    Quantity INT NOT NULL,
    ItemID INT NOT NULL,
    CONSTRAINT FK_Consumptions_Items FOREIGN KEY (ItemID) REFERENCES Items(ItemID)
);

-- Create Users table
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    UserName VARCHAR(255) NOT NULL,
    UserEmail VARCHAR(255) NOT NULL UNIQUE,
    UserPassword VARCHAR(255) NOT NULL
);

-- Create User_Items table
CREATE TABLE User_Items (
    UserID INT NOT NULL,
    ItemID INT NOT NULL,
    PRIMARY KEY (UserID, ItemID),
    CONSTRAINT FK_User_Items_Users FOREIGN KEY (UserID) REFERENCES Users(UserID),
    CONSTRAINT FK_User_Items_Items FOREIGN KEY (ItemID) REFERENCES Items(ItemID)
);
-- Create ExpirationDates table
CREATE TABLE ExpirationDates (
    ExpirationID INT PRIMARY KEY IDENTITY(1,1),
    ExpirationDate DATE NOT NULL,
    ItemID INT NOT NULL,
    CONSTRAINT FK_ExpirationDates_Items FOREIGN KEY (ItemID) REFERENCES Items(ItemID)
);
