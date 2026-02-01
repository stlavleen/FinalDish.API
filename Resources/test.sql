
USE FinalDish;

SELECT 
	di.DishId, 
	d.Name AS DishName, 
	di.IngredientId, 
	i.Name AS IngredientName 
FROM dbo.Dishes AS d
LEFT JOIN dbo.Dishes_Ingredients AS di ON d.Id = di.DishId
LEFT JOIN dbo.Ingredients AS i ON di.IngredientId = i.Id;
