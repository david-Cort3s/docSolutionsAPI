using docSolutionAPI.Models;

namespace docSolutionAPI.Contracts
{
    public class CreateUserRequest
    {
        public string? Tenant { get; set; }

        public string Nombre { get; set; }

        public string Apellido_Paterno { get; set; }

        public string Apellido_Materno { get; set; }

        public string Email { get; set; }

        public string Telefono { get; set; }

        public string Usuario { get; set; }

        public string Password { get; set; }

        public string? Metadata { get; set; }

        public int Roles { get; set; }
    }
}
