# FinalDish.API

Features:
1. Users can create/modify/delete their own dishes.
2. API provides pre-installed dishes for everyone without authorization requirement.
3. Moderators and Administrators have extended rights.  

Guide:

After API install (placed release build in some directory) do the following:

1. Add secret data to appsettings.json (use Resources/additional_appsettings_pattern.json and replace "Val" by your own values).
2. Add roles: Moderator, Administrator (see Resources/IdentityTestData.txt as example).
3. Add supervisors (Moderators, Administrators users). Note: Both Administrator and Moderator roles should be assigned to Admin user.
4. Assign roles to users
5. Add Dishes Types, Dishes, Ingredients, Dishes_Ingredients (use script Resources/FinalDish.sql. 
6. Host API in IIS or Nginx
