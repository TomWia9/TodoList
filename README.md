# TodoList
This repository conatins TodoList - a sample Blazor app

## Description
This project is a **.NET 5** implemented Blazor WebAssembly application for managing tasks.

**TodoList** enables communication with the **MSSQL database** consisting of sending and receiving data regarding users, lists and todos. Application can be used by users who may create account and login to the application.
## Screenshots
<details>
  <summary>Click to expand!</summary>
  
#### List of todos
![ListOfTodos](https://i.imgur.com/zwqBkva.png[/img] "List of todos")

#### Todo details
![Todo details](https://i.imgur.com/x6gS05a.png[/img] "Todo details")

#### Login/Register
![Login/Register](https://i.imgur.com/6ZYIx8I.png[/img] "Login/Register")

</details>

## Stack
It uses **Entity Framework Core** to communicate with a database, which contains required data tables like:
* Users - where informations about users are stored 
* ListsOfTodos - where informations about lists of todos are stored 
* Todos-  where informations about todos are stored.

Other tools used in project:
* **JwtBearer** - for authentication
* **Open API** - for API documentation
* **AutoMapper** - for mapping DTO-s and EntityModels data

## API Controllers and their operations:

### Users - Controller for users management
<details>
  <summary>Click to expand!</summary>
  
* **[POST] Create new user**  
 ```/api/users```
* **[POST] Authenticate the user**  
 ```/api/users/authenticate```
* **[GET] Get user by id**  
 ```/api/users/{userId}```
</details>

### Lists - Controller for todo lists management
<details>
  <summary>Click to expand!</summary>
  
* **[POST] Create a new list**  
 ```/api/lists```
* **[GET] Get a list of todo lists**  
 ```/api/lists```
* **[GET] Get todo list by id**  
 ```/api/lists/{listOfTodosId}```
* **[DELETE] Delete the todo list with given id**  
 ```/api/lists/{listOfTodosId}```
* **[PUT] Update (full update) todo list**  
 ```/api/lists/{listOfTodosId}```
</details>

### Todos - Controller for todos management
<details>
  <summary>Click to expand!</summary>
  
* **[POST] Create a new todo**  
 ```/api/lists/{listOfTodosId}/todos```
* **[GET] Get a list of todos from specified todo list**  
 ```/api/lists/{listOfTodosId}/todos```
* **[GET] Get todo from specified todo list**  
 ```/api/lists/{listOfTodosId}/todos/{todoId}```
* **[DELETE] Delete the todo with given id**  
 ```/api/lists/{listOfTodosId}/todos/{todoId}```
* **[PUT] Update (full update) todo**  
 ```/api/lists/{listOfTodosId}/todos/{todoId}```
* **[PATCH] Update (Partially update) todo**  
 ```/api/lists/{listOfTodosId}/todos/{todoId}```
</details>

For more documentation data, visit 
```/swagger/TodoListOpenAPISepcification/swagger.json```  
(or ```/index.html``` for documentation UI)

## Installation
Make sure you have the **.NET 5.0 SDK** installed on your machine. Then do:  
>`git clone https://github.com/TomWia9/TodoList.git`  
`cd TodoList\TodoList\Server`  
`dotnet run`

## Configuration
This will need to be perfored before running the application for the first time
1. You have to change ConnectionString in **appsettings.json** for ConnectionString that allow you to connect with database in your computer.
2. Issue the Entity Framework command to update the database  
`dotnet ef database update`
 
## License
[MIT](https://choosealicense.com/licenses/mit/)
