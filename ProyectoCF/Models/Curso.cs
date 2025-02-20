using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoCF.Models
{
    public class Curso
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        [Required]
        public int DocenteId { get; set; }
        public ICollection<Material> Materiales { get; set; }
    }
}
