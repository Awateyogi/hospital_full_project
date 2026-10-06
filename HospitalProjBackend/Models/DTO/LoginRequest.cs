namespace HospitalProjBackend.Models.DTO
{
    public class LoginRequest
    {
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
    }
}
