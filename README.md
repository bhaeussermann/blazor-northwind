# Northwind

This is a simple CRUD application for managing the Employees in a standard Northwind database. It is hosted [here](https://blazor-northwind.azurewebsites.net) as a quick preview.

The application interfaces with a [REST service hosted via Express in Node.js](https://github.com/bhaeussermann/northwind-api).

It connects to where the API is hosted [here](https://northwind-express-api.herokuapp.com/swagger/) by default, but this can be changed in *Northwind.Server/appsettings.json* if you're hosting the API yourself.

## Project setup

### Compile the code
```
dotnet build
```

### Compile and hot-reload for development
```
dotnet watch --project Northwind.Client run --project Northwind.Server
```
