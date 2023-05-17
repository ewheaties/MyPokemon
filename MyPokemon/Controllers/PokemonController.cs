using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using MySqlConnector;
using PokeApiNet;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MyPokemon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly PokeApiClient _pokeApiClient;
        private readonly string _connectionString;
        private string _jsonString; 

        public PokemonController(IConfiguration configuration)
        {
            _pokeApiClient = new PokeApiClient();
            _connectionString = "Server=localhost;Database=sys;Uid=wheaton;Pwd=pokeapi411*;";
            _jsonString = "";
        }
        
        [HttpGet("{name}")]
        public async Task<IActionResult> GetPokemonAsync(string name)
        {
            try
            {
                // make api call for PokemonSpecies object and serialize response if successful 
                PokemonSpecies pokemon = await _pokeApiClient.GetResourceAsync<PokemonSpecies>(name);
                _jsonString = JsonConvert.SerializeObject(pokemon);
            }
            catch (Exception ex)
            {
                // serialize error message if incorrect input is given 
                _jsonString = JsonConvert.SerializeObject("{error: " + ex.Message + " }");
            }
            
            try
            {
                // store the json response in the MySQL database
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var command = connection.CreateCommand();

                    //given database has an int auto-increment primary key RequestId
                    command.CommandText = "INSERT INTO PokemonSpeciesRequest (RequestName, RequestJson) " +
                    "VALUES (@RequestName, @RequestJson)";

                    command.Parameters.AddWithValue("@RequestName", name);
                    command.Parameters.AddWithValue("@RequestJson", _jsonString ?? "");

                    await command.ExecuteNonQueryAsync();

                    connection.Close();
                }

                return Ok(_jsonString);

            }
            catch (Exception e)
            {
                //prints message if database connection is unsuccessful
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }
    }
}


