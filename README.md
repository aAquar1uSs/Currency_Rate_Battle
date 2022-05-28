# Currency Rate Battle

#Docker-Compose Run

Server configuration:

Update ConnectionDb in appsettings file for the server:

`"ConnectionDb": "Host=db;Port=5432;Database=CRBdb;Username=postgres;Password=111111"

Client configuration:

`"BaseURL": "http://localhost:5003",

Execute such commands in the  root folder:

`docker-compose build

`docker-compose up

In order to check the status of the services:

`docker-compose ps