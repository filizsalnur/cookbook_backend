# Cookbook Backend

This backend project was developed for the BLM_4519 course at Ankara University. It serves as the .NET backend for a recipe management system. The associated Flutter project for the mobile application can be found [here](https://github.com/filizsalnur/cookbook_mobile).

## Recipes Page
![Recipes Page](https://github.com/filizsalnur/cookbook_backend/assets/92436947/e28a692f-7109-4d3c-ad83-a313e396986e)

## API Endpoints

### Create a User
**Endpoint:** `POST http://localhost:5003/api/User`

**Example JSON:**
```json
{
  "UserName": "filiz",
  "Email": "filiz@example.com",
  "Password": "123",
  "Recipes": []
}
```
### Create a User
**Endpoint:**  `POST http://localhost:5003/api/Recipe`

**Example JSON:**
```json
{
  "Title": "Grilled Chicken Kebabs",
  "Description": "Savor the flavor of perfectly grilled chicken kebabs with aromatic spices.",
  "UserId": "6582f9bb5b1cd75150cb4175",
  "UserName": "first_user",
  "MealType": "Kebab - Skewer"
}
```
### Get All Users
**Endpoint:** `GET http://localhost:5003/api/User`

### Get All Recipes
**Endpoint:**  `GET http://localhost:5003/api/Recipe`
