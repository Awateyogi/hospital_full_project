using HospitalProjBackend.Data;
using HospitalProjBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Sockets;


namespace HospitalProjBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AdminController(AppDbContext db) { _db = db; }

        // Doctors CRUD
        [HttpGet("doctors")] public async Task<IActionResult> GetDoctors() => Ok(await _db.Doctors.ToListAsync());
        [HttpGet("doctors/{id}")] public async Task<IActionResult> GetDoctor(int id) { var d = await _db.Doctors.FindAsync(id); if (d == null) return NotFound(); return Ok(d); }
        [HttpPost("doctors")] public async Task<IActionResult> CreateDoctor([FromBody] Doctor d) { _db.Doctors.Add(d); await _db.SaveChangesAsync(); return CreatedAtAction(nameof(GetDoctor), new { id = d.Id }, d); }
        [HttpPut("doctors/{id}")] public async Task<IActionResult> UpdateDoctor(int id, [FromBody] Doctor input) { var d = await _db.Doctors.FindAsync(id); if (d == null) return NotFound(); d.Name = input.Name; d.Specialty = input.Specialty; await _db.SaveChangesAsync(); return NoContent(); }
        [HttpDelete("doctors/{id}")] public async Task<IActionResult> DeleteDoctor(int id) { var d = await _db.Doctors.FindAsync(id); if (d == null) return NotFound(); _db.Doctors.Remove(d); await _db.SaveChangesAsync(); return NoContent(); }

        // Patients CRUD
        [HttpGet("patients")] public async Task<IActionResult> GetPatients() => Ok(await _db.Patients.ToListAsync());
        [HttpGet("patients/{id}")] public async Task<IActionResult> GetPatient(int id) { var p = await _db.Patients.FindAsync(id); if (p == null) return NotFound(); return Ok(p); }
        [HttpPost("patients")] public async Task<IActionResult> CreatePatient([FromBody] Patient p) { _db.Patients.Add(p); await _db.SaveChangesAsync(); return CreatedAtAction(nameof(GetPatient), new { id = p.Id }, p); }
        [HttpPut("patients/{id}")] public async Task<IActionResult> UpdatePatient(int id, [FromBody] Patient input) { var p = await _db.Patients.FindAsync(id); if (p == null) return NotFound(); p.Name = input.Name; p.Phone = input.Phone; p.Email = input.Email; await _db.SaveChangesAsync(); return NoContent(); }
        [HttpDelete("patients/{id}")] public async Task<IActionResult> DeletePatient(int id) { var p = await _db.Patients.FindAsync(id); if (p == null) return NotFound(); _db.Patients.Remove(p); await _db.SaveChangesAsync(); return NoContent(); }
    }
}
