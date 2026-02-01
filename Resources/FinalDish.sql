
USE FinalDish;
GO

BEGIN DISTRIBUTED TRANSACTION; 

DELETE FROM dbo.Dishes_Ingredients;
DELETE FROM dbo.Ingredients;
DELETE FROM dbo.Dishes;
DELETE FROM dbo.DishTypes;

SET IDENTITY_INSERT dbo.Ingredients ON;

INSERT INTO dbo.Ingredients(Id, Name) VALUES
(1, 'Potato'),
(2, 'Dough'),
(3, 'Tomato'),
(4, 'Cucumber'),
(5, 'Ham'),
(6, 'Chicken'),
(7, 'Mozarella'),
(8, 'Chedder'),
(9, 'Parsley'),
(10, 'Onion'),
(11, 'Garlic'),
(12, 'Olive oil'),
(13, 'Oregano'),
(14, 'Chocolate'),
(15, 'Cream'),
(16, 'Milk'),
(17, 'Eggs'),
(18, 'Mushrooms'),
(19, 'Salt'),
(20, 'Pepper');

SET IDENTITY_INSERT dbo.Ingredients OFF;

SET IDENTITY_INSERT dbo.DishTypes ON;

INSERT INTO dbo.DishTypes(Id, Name) VALUES
(1, 'Hot dish'),
(2, 'Cold dish'),
(3, 'Soup'),
(4, 'Dessert'),
(5, 'Sauce');

SET IDENTITY_INSERT dbo.DishTypes OFF;

SET IDENTITY_INSERT dbo.Dishes ON;

INSERT INTO dbo.Dishes(Id, Name, DishTypeId) VALUES
(1, 'French fries', 1),
(2, 'Pizza Margarita', 1),
(3, 'Pizza with ham', 1),
(4, 'Salad Cesar', 2),
(5, 'Mushroom cream soup', 3),
(6, 'Choco Ice-cream', 4),
(7, 'Cheese sauce', 5),
(8, 'Sauce BBQ', 5),
(9, 'Ketchup', 5);

SET IDENTITY_INSERT dbo.Dishes OFF;

INSERT INTO dbo.Dishes_Ingredients(DishId, IngredientId) VALUES
(1, 1),
(1, 19),
(2, 2),
(2, 3),
(2, 8),
(3, 2),
(3, 5),
(3, 7),
(3, 13),
(4, 6),
(4, 7),
(4, 12),
(5, 11),
(5, 12),
(5, 13),
(5, 15),
(5, 18),
(6, 14),
(6, 15),
(6, 16),
(7, 8),
(7, 15),
(7, 16),
(7, 17),
(8, 3),
(8, 5),
(8, 6),
(8, 15),
(8, 16),
(8, 17),
(9, 3),
(9, 15),
(9, 16),
(9, 17);

COMMIT TRANSACTION;
GO
