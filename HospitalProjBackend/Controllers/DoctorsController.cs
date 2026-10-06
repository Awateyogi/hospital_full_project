using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalProjBackend.Data;

namespace HospitalProjBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public DoctorsController(AppDbContext db) { _db = db; }

        [HttpGet] public async Task<IActionResult> Get() => Ok(await _db.Doctors.ToListAsync());
    }
}

