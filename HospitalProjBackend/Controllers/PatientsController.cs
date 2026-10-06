using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalProjBackend.Data;

namespace HospitalProjBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public PatientsController(AppDbContext db) { _db = db; }

        [HttpGet] public async Task<IActionResult> Get() => Ok(await _db.Patients.ToListAsync());
    }
 
 }
