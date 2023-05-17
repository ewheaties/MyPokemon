MyPokemon API 

# Description
An API .NET Core project that sends a request to PokeNetApi and gets a json response
S/F for a pokemon species by name or id in the url 

# Installation
Project was created utilizing MySQL and MySQL Workbench

1) Create database table with the following query:

CREATE TABLE PokemonSpeciesRequest (
    RequestId INT AUTO_INCREMENT PRIMARY KEY,
    RequestName VARCHAR(255) NOT NULL DEFAULT '',
    RequestJson JSON NULL
);

2) Modify _connectionString credentials in PokemonController.cs for database name,
username and passwordas needed and add permissions necessary for local database access

3) Install following nuget packages:

Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Tools
Microsoft.Extensions.Configuration
Microsoft.Extensions.Configuration.EnvironmentVariables
Microsoft.Extensions.Configuration.Json
Pomelo.EntityFrameworkCore.MySql 
(not all are necessary if you want to migrate PokemonSpecies model into table)

# Usage
Run project in visual studio and add name or id into the url in the generated webpage

ex: https://localhost:7188/api/pokemon/pikachu
    https://localhost:7188/api/pokemon/25

webpage displays json result and stores into local database










