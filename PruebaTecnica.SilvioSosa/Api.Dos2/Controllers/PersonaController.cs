using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace Api.Dos2.Controllers
{
    [ApiController]
    [Route("Persona2")]
    public class PersonaController : Controller
    {
        [HttpGet("Personas")]
        public async Task<ActionResult<List<Persona>>> PersonasLista()
        {
            var respuesta = new List<Persona>();
            var urlApiExterna = "https://localhost:7244/api/Persona";
            using var client = new HttpClient();

            try
            {
                var response = await client.GetAsync(urlApiExterna);
                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(respuesta);
                }

                string jsonSerializado = await response.Content.ReadAsStringAsync();   
                respuesta = JsonConvert.DeserializeObject<List<Persona>>(jsonSerializado)!;
                //respuesta.Add(JsonConvert.DeserializeObject<Persona>(jsonSerializado)!);
            }
            catch(Exception e)
            {
                
            }
            return Ok(respuesta);
        }

        private readonly IConfiguration _config;
        public PersonaController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("PersonaId")]
        public async Task<ActionResult<Persona>> GetPersona(int personaId)
        {
            using var conexion = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var persona = await conexion.QueryFirstAsync<Persona>("SELECT * FROM Persona WHERE id = @Id",
                new { Id = personaId });
            return Ok(persona);
        }
    }
}
