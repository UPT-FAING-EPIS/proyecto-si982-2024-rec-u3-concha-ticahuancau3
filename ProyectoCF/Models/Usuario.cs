using System.ComponentModel.DataAnnotations;

namespace ProyectoCF.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Nombre { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Apellido { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public required string Email { get; set; }

        [Required]
        public required string Contrasena { get; set; }

        [Required]
        public required string Rol { get; set; }
    }

}
