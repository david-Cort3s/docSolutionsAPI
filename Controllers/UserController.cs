using Microsoft.AspNetCore.Mvc;
using docSolutionAPI.Models;
using docSolutionAPI.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace docSolutionAPI.Controllers
{
    [Authorize]
    [ApiController]
	[Route("api/user")]
	public class UserController : ControllerBase
	{
		private readonly Supabase.Client _db;

		public UserController(Supabase.Client client)
		{
			_db = client;
		}

		[HttpPost]
		[Route("GetUsers")]
		public async Task<IActionResult> GetUsers([FromBody] GetUsersRequest getUsersRequest)
		{
			if(string.IsNullOrEmpty(getUsersRequest.SearchText))
			{
				return BadRequest("El campo de búsqueda no puede estar vacío");
			}

			try
			{
				var searchTextLower = getUsersRequest.SearchText.ToLower();

                var response = await _db
					.From<User>()
					.Where(user => user.Nombre.Contains(searchTextLower))
					.Get();

				var users = response.Models;

				if (!users.Any())
				{
					return NotFound("No se encontraron usuarios que conicidan con la búsqueda");
				}

				var usersResponse = users.Select(user => new UserResponse
				{
					Id = user.Id,
					Nombre = user.Nombre,
					Apellido_Paterno = user.Apellido_Paterno,
					Apellido_Materno = user.Apellido_Paterno,
					Email = user.Email,
					Telefono = user.Telefono,
					Usuario = user.Usuario,
					Tenant = user.Tenant,
                    Roles = new List<Role>
                    {
                        new Role {
                            Id = user.Roles,
                            Name = GetUserRoleName(user.Roles)
                        }
                    },
                    Metadata = user.Metadata,
				});

				return Ok(usersResponse);
			}
			catch(Exception ex)
			{
				return StatusCode(500, $"Error al obtener usuarios: {ex.Message}");
			}
		}

        [HttpPost]
        [Route("RegisterUserRole")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                var newUser = new User
                {
                    Tenant = request.Tenant,
                    Nombre = request.Nombre.ToLower(),
                    Apellido_Paterno = request.Apellido_Paterno.ToLower(),
                    Apellido_Materno = request.Apellido_Materno.ToLower(),
                    Email = request.Email,
                    Telefono = request.Telefono,
                    Usuario = request.Usuario,
                    Password = request.Password,
                    Roles = request.Roles,
                    Metadata = request.Metadata
                };

                var responses = await _db
                .From<User>()
                .Insert(newUser);

                var usersResponse = new UserResponse
                {
                    Id = newUser.Id,
                    Nombre = newUser.Nombre,
                    Apellido_Paterno = newUser.Apellido_Paterno,
                    Apellido_Materno = newUser.Apellido_Paterno,
                    Email = newUser.Email,
                    Telefono = newUser.Telefono,
                    Usuario = newUser.Usuario,
                    Tenant = newUser.Tenant,
                    Roles = new List<Role>
                    {
                        new Role {
                            Id = newUser.Roles,
                            Name = GetUserRoleName(newUser.Roles)
                        }
                    },
                    Metadata = newUser.Metadata,
                };

                return Ok(usersResponse);


            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear usuario: {ex.Message}");
            }

        }

        private string GetUserRoleName(int roleId)
        {
            switch (roleId)
            {
                case 1:
                    return "admin";
                case 2:
                    return "user";
                default:
                    return "unknown";
            }
        }

    }
}
