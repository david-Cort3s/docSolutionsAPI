using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using docSolutionAPI.Contracts;
using docSolutionAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace docSolutionAPI.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly Supabase.Client _db;

        public LoginController(IConfiguration config, Supabase.Client client)
        {
            _config = config;
            _db = client;
        }

        [HttpPost]
        [Route("authentication")]
        public async Task<IActionResult> Post([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Usuario) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Nombre de usuario y contraseña son obligatorios.");
            }

            try
            {
                var response = await _db
                                    .From<User>()
                                    .Where(user => user.Usuario == loginRequest.Usuario)
                                    .Get();
                var user = response.Models.First();

                if (user.Password != loginRequest.Password)
                {
                    return BadRequest("Contraseña incorrecta");
                }
            }
            catch
            {
                return NotFound($"Usuario {loginRequest.Usuario} no encontrado");
            }
            

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return Ok(token);
        }
    }
}