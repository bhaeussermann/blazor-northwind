# Northwind

This is a simple CRUD application for managing the Employees in a standard Northwind database.

The application interfaces with a [REST service hosted via Express in Node.js](https://github.com/bhaeussermann/northwind-api).

It connects to where the API is hosted [here](https://northwind-express-api.herokuapp.com/swagger/) by default, but this can be changed in *Northwind.Server/appesttings.json* if you're hosting the API yourself.

## Project setup

### Compiles the code
```
dotnet build
```

### Compiles and hot-reloads for development
```
dotnet watch --project Northwind.Client run --project Northwind.Server
```
