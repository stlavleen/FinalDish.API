# FinalDish.API

TODO: 
1. Check roles on different actions.
	For example: 
	- only admin can add new or modify existing ingredients, dishes categories and base dishes (preinstalled).
	- moderator can modify existing ingredients, dishes categories and base dishes (preinstalled).
2. Create UserDishes table and corresponding relations to Users and Dishes to separate preinstalled from user dishes.

Main tasks:
1. Users can create/modify/delete their own dishes.
2. Users can use preinstalled dishes (readonly) as templates to create their own.  

Guide:

After app install do the following:

Identity (see Resources/IdentityTestData.txt as example):
1. Add roles (Moderator, Administrator)
2. Add supervisors (Moderators, Administrators). 
	Note: Both Administrator and Moderator roles should be assigned to Admin user.
3. Assign roles to users

Dishes:
4. Add Dishes Types, Dishes, Ingredients, Dishes_Ingredients. (use script from Resources)
