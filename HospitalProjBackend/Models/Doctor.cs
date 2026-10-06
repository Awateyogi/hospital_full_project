using System.ComponentModel.DataAnnotations;

namespace HospitalProjBackend.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; } = "";
        public string? Specialty { get; set; }
    }
}
