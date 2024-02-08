using Postgrest.Attributes;
using Postgrest.Models;

namespace docSolutionAPI.Models
{
    [Table("users")]
	public class User : BaseModel
	{
        [PrimaryKey("id")]
		public int Id { get; set; }

        [Column("Nombre")]
		public string Nombre { get; set; }

        [Column("Apellido_Paterno")]
        public string Apellido_Paterno { get; set; }

        [Column("Apellido_Materno")]
        public string Apellido_Materno { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Telefono")]
        public string Telefono { get; set; }

        [Column("Usuario")]
        public string Usuario { get; set; }

        [Column("Password")]
        public string Password { get; set; }

        [Column("Tenant")]
        public string? Tenant { get; set; }

        [Column("Metadata")]
        public string? Metadata { get; set; }

        [Column("Roles")]
        public int Roles { get; set; }
    }
}
