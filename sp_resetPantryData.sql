CREATE PROCEDURE [dbo].[sp_resetPantryData]
AS
BEGIN
    DELETE FROM Items;
    DELETE FROM Categories;

    DBCC CHECKIDENT ('Items', RESEED, 0);
    DBCC CHECKIDENT ('Categories', RESEED, 0);

    INSERT INTO Categories (CategoryName)
    VALUES ('Canned Foods'), ('Bakery'), ('Dry Goods'), ('Beverages');

    INSERT INTO Items(ItemName, ItemDescription, CategoryID, Quantity, Unit)
    VALUES 
    ('Canned Tomatoes', 'Whole peeled tomatoes', 1, 400, 'grams'),
    ('Bread', 'Whole wheat bread', 2, 500, 'grams'),
    ('Pasta', 'Spaghetti noodles', 3, 1000, 'grams'),
    ('Soda', 'Cola', 4, 355, 'milliliters'),
    ('Canned Peaches', 'Peaches in syrup', 1, 720, 'grams'),
    ('Bagels', 'Plain bagels', 2, 140, 'grams'),
    ('Rice', 'White rice', 3, 1000, 'grams'),
    ('Juice', 'Orange juice', 4, 1000, 'milliliters'),
    ('Canned Beans', 'Kidney beans', 1, 400, 'grams'),
    ('Muffins', 'Blueberry muffins', 2, 125, 'grams'),
    ('Cereal', 'Cornflakes', 3, 500, 'grams'),
    ('Milk', 'Whole milk', 4, 1000, 'milliliters'),
    ('Canned Corn', 'Sweet corn', 1, 340, 'grams'),
    ('Cakes', 'Chocolate cakes', 2, 500, 'grams'),
    ('Noodles', 'Ramen noodles', 3, 500, 'grams'),
    ('Water', 'Mineral water', 4, 500, 'milliliters'),
    ('Peanut Butter', 'Smooth peanut butter', 3, 340, 'grams'),
    ('Crackers', 'Saltine crackers', 3, 250, 'grams'),
    ('Sugar', 'White granulated sugar', 3, 500, 'grams'),
    ('Honey', 'Wildflower honey', 3, 340, 'grams'),
    ('Tea', 'Black tea bags', 4, 100, 'grams');
END
