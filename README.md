This is simple API application, demonstrating way of coding REST API. 
API is simple and consist of few functions for:
* retrieving list of products
* retrieving one product by its ID
* updating of one product description
* updated version of retrieving products list, supporting pagination, held as function update in higher API version 

You can see here:
* maintaining of two wersions of API
* integration of browser accessible documentation (Swagger)
* implementation of unit tests

Libraries used
==============
* **.Net Core**
* **Swashbuckle/Swagger** for in-browser API documentation
* **AutoMapper**
* **Entity Framework Core** for accessing data

Unit testing
------------
* **NUnit** as unit testing framework
* **Moq** as mocking framework
* **EntityFrameworkCore.InMemory** - in-memory database for mocking DB

Requirements
============
* MS SQL server
* Hosting environment - IIS instance, or run it from Visual Studio

Setting up
==========
For runing application you need to prepare and configure SQL database
1. Create new database in your MS SQL server
2. Set connection string to your database in configuration - appsettings.json or appsettings.Development.json or user secrets (depending on purpose of app instance you are preparing)
	The connection string must be named "ProductApiExampleData" (see sample in appsettins.json)
3. Build database structure by Entity Framework command update-database.

Database will be created with few seed sample product records.