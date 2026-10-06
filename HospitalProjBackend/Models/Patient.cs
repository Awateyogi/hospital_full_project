using System.ComponentModel.DataAnnotations;

namespace HospitalProjBackend.Models
{
    public class Patient
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; } = "";
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
