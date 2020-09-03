# asp-core-crud-example
A sample .NET Core project with a MongoDB database with replicaset and transactions enabled.

## Goal
The goal of this project is to give a solution to the subject below (used for a job interview) :
```
Please create a sample .net/C# project that includes the following elements:
- Basic CRUD functionality for a data structure of your choice.
- RESTful APIs to expose that functionality with your framework of choice (ASP.NET Web APIs, ASP.NET Core, ServiceStack (preferred), …etc).
- Uses MongoDB using C# driver.
- Please demonstrate your usage of one of MongoDB advanced features (aggregation, transactions, indexes, …etc) using C# driver.
- Please push your code to a Git repository that you can share with us.
```

## Project Architecture
This is a asp core 3.1 web API project with a n-tiers architecture :
- model layer
- business logic layer
- data access layer
- presentation layer (here, a REST API)

Each layer has its own abstraction layer in order to avoid dependencies. 
I'm using the stock .net core IOC and DI engine.

## Tests
I didn't make any **Unit Tests** because there is no business logic. However, i've made a sample of an **Integration Test** using the ***Microsoft WebHost library***.
You can run the Integration Tests through visual studio or command line, but don't forget to run the ***docker-compose*** first (or else you won't have any database to request).

## Database
The implementation of the DAL is using the MongoDB C# driver. 
By running the docker-compose attached to the solution, you will get an instance of a Mongo server with a **replicaset enabled** (in order to support transactions).

### Transactions & (kind of) unit of work
As a demonstration of the usage of one of MongoDB advanced features, I've choosen transactions. So the ***IClientSessionHandle*** is injected in all repositories. 
I've also managed to make a middleware called **TransactionFilter** that creates a transaction for the whole HTTP request.
This way, we can make multi-document operations, on different repositories, that may rollback if a problem happens, without dealing with multi-document transactions manually. 

**Example :** 
- Http request arrives
- Transaction opens
- Insert in XXX collection
- Update in YYY collection with the Id taken from XXX created previously
- The Update fails
- The transaction will abort and rollback the insertion of XXX

It's working as it is but it would need some more further load testing. 
This may not be production ready.

## How to run ?
-  Open the solution in Visual Studio
-  Target the docker-compose project in the **00 - Solution Items folder**
-  Run with F5
-  **Wait for a few seconds (time for the replicaset to setup)**
-  Go to the swagger UI to make some requests : https://localhost:5000/swagger
-  Watch the db in Mongo Express interface if needed : http://localhost:8081

#### Known bugs :
Using ***docker-compose up*** in commandline doesn't seem to work with the API at the moment. 
I didn't have much time left so I just kept using VS to compose.