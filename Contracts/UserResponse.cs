using docSolutionAPI.Models;

namespace docSolutionAPI.Contracts
{
	public class UserResponse
	{
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Apellido_Paterno { get; set; }

        public string Apellido_Materno { get; set; }

        public string Email { get; set; }

        public string Telefono { get; set; }

        public string Usuario { get; set; }

        public string? Password { get; set; }

        public string Tenant { get; set; }

        public string Metadata { get; set; }

        public List<Role> Roles { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

