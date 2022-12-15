using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Api.Uno.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : Controller
    {
        private readonly IConfiguration _config;
        public PersonaController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Persona>>> GetPersonas()
        {
            using var conexion = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var personas = await conexion.QueryAsync<Persona>("SELECT * FROM Persona");
            return Ok(personas);
        }

        [HttpGet("PersonaId")]
        public async Task<ActionResult<Persona>> GetPersona(int personaId)
        {
            using var conexion = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var persona = await conexion.QueryFirstAsync<Persona>("SELECT * FROM Persona WHERE id = @Id",
                new {Id = personaId});
            return Ok(persona);
        }

        [HttpPost("Agregar")]

        public async Task<ActionResult<Persona>> CreatePersona(Persona persona)
        {
            using var conexion = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await conexion.ExecuteAsync("INSERT INTO Persona (Nombre,Apellido) VALUES (@Nombre,@Apellido)", persona);
            return Ok(await SelectAllPersonas(conexion));
        }

        private static async Task<IEnumerable<Persona>> SelectAllPersonas(SqlConnection connection)
        {
            return await connection.QueryAsync<Persona>("SELECT * FROM Persona");
        }

    }
}
