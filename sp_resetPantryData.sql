CREATE PROCEDURE [db_owner].[sp_resetPantryData]
AS
BEGIN
    DELETE FROM Items;
    DELETE FROM Categories;


    DBCC CHECKIDENT ('Items', RESEED, 0);
    DBCC CHECKIDENT ('Categories', RESEED, 0);

    INSERT INTO Categories (CategoryName)
    VALUES ('Canned Foods'), ('Bakery'), ('Dry Goods'), ('Beverages');

    INSERT INTO Items(ItemName, ItemDescription, CategoryID, Quantity, Unit, Expiration)
    VALUES 
    ('Canned Tomatoes', 'Whole peeled tomatoes', 1, 400, 'grams', '2020-01-01'),
    ('Bread', 'Whole wheat bread', 2, 500, 'grams', '2020-01-01'),
    ('Pasta', 'Spaghetti noodles', 3, 1000, 'grams', '2020-01-01'),
    ('Soda', 'Cola', 4, 355, 'milliliters', '2020-01-01'),
    ('Canned Peaches', 'Peaches in syrup', 1, 720, 'grams', '2020-01-01'),
    ('Bagels', 'Plain bagels', 2, 140, 'grams', '2020-01-01'),
    ('Rice', 'White rice', 3, 1000, 'grams', '2020-01-01'),
    ('Juice', 'Orange juice', 4, 1000, 'milliliters', '2020-01-01'),
    ('Canned Beans', 'Kidney beans', 1, 400, 'grams', '2020-01-01'),
    ('Muffins', 'Blueberry muffins', 2, 125, 'grams', '2020-01-01'),
    ('Cereal', 'Cornflakes', 3, 500, 'grams', '2020-01-01'),
    ('Milk', 'Whole milk', 4, 1000, 'milliliters', '2020-01-01'),
    ('Canned Corn', 'Sweet corn', 1, 340, 'grams','2020-01-01'),
    ('Cakes', 'Chocolate cakes', 2, 500, 'grams', '2020-01-01'),
    ('Noodles', 'Ramen noodles', 3, 500, 'grams', '2020-01-01'),
    ('Water', 'Mineral water', 4, 500, 'milliliters', '2020-01-01'),
    ('Peanut Butter', 'Smooth peanut butter', 3, 340, 'grams', '2020-01-01'),
    ('Crackers', 'Saltine crackers', 3, 250, 'grams', '2020-01-01'),
    ('Sugar', 'White granulated sugar', 3, 500, 'grams', '2020-01-01'),
    ('Honey', 'Wildflower honey', 3, 340, 'grams', '2020-01-01'),
    ('Tea', 'Black tea bags', 4, 100, 'grams', '2020-01-01');

END
