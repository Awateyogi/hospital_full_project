////using Microsoft.AspNetCore.Mvc;
////using Microsoft.EntityFrameworkCore;
////using Microsoft.AspNetCore.Authorization;
////using HospitalProjBackend.Data;
////using HospitalProjBackend.Models;
////using HospitalProjBackend.Services;

////namespace HospitalProjBackend.Controllers
////{
////    [ApiController]
////    [Route("api/[controller]")]
////    public class AppointmentsController : ControllerBase
////    {
////        private readonly AppDbContext _db;
////        private readonly IEmailService _email;
////        private readonly ISmsService _sms;

////        public AppointmentsController(AppDbContext db, IEmailService email, ISmsService sms)
////        {
////            _db = db; _email = email; _sms = sms;
////        }

////        [HttpGet, Authorize] public async Task<IActionResult> Get() => Ok(await _db.Appointments.Include(a => a.Patient).Include(a => a.Doctor).ToListAsync());

////        [HttpPost, Authorize]
////        public async Task<IActionResult> Create([FromBody] Appointment appt)
////        {
////            var conflict = await _db.Appointments.AnyAsync(a => a.DoctorId == appt.DoctorId && a.Date == appt.Date);
////            if (conflict) return BadRequest(new { error = "Doctor has another appointment at this time" });
////            _db.Appointments.Add(appt);
////            await _db.SaveChangesAsync();

////            var patient = await _db.Patients.FindAsync(appt.PatientId);
////            if (patient != null)
////            {
////                if (!string.IsNullOrEmpty(patient.Email))
////                    await _email.SendAsync(patient.Email, "Appointment booked", $"Your appointment is at {appt.Date}");
////                if (!string.IsNullOrEmpty(patient.Phone))
////                    await _sms.SendAsync(patient.Phone, $"Appointment at {appt.Date}");
////            }

////            return CreatedAtAction(nameof(GetById), new { id = appt.Id }, appt);
////        }

////        [HttpGet("{id}"), Authorize]
////        public async Task<IActionResult> GetById(int id)
////        {
////            var appt = await _db.Appointments.Include(a => a.Patient).Include(a => a.Doctor).FirstOrDefaultAsync(a => a.Id == id);
////            if (appt == null) return NotFound();
////            return Ok(appt);
////        }

////        [HttpPut("{id}"), Authorize]
////        public async Task<IActionResult> Update(int id, [FromBody] Appointment input)
////        {
////            var appt = await _db.Appointments.FindAsync(id);
////            if (appt == null) return NotFound();
////            var conflict = await _db.Appointments.AnyAsync(a => a.Id != id && a.DoctorId == input.DoctorId && a.Date == input.Date);
////            if (conflict) return BadRequest(new { error = "Conflict with another appointment" });
////            appt.Date = input.Date; appt.PatientId = input.PatientId; appt.DoctorId = input.DoctorId; appt.Reason = input.Reason;
////            await _db.SaveChangesAsync();
////            return NoContent();
////        }

////        [HttpDelete("{id}"), Authorize]
////        public async Task<IActionResult> Delete(int id)
////        {
////            var appt = await _db.Appointments.FindAsync(id);
////            if (appt == null) return NotFound();
////            _db.Appointments.Remove(appt);
////            await _db.SaveChangesAsync();
////            return NoContent();
////        }
////    }
////}
//using HospitalProjBackend.Data;
//using HospitalProjBackend.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Claims;

//namespace HospitalProjBackend.Controllers
//{
//    [Authorize]
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AppointmentsController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;

//        public AppointmentsController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // =========================================
//        // GET: List appointments
//        // Admin sees all, staff/patient sees only their own
//        // =========================================
//        [HttpGet]
//        public async Task<IActionResult> GetAppointments()
//        {
//            var role = User.FindFirstValue("role"); // JWT claim "role"
//            var userIdClaim = User.FindFirstValue("userId");

//            if (role == "Admin")
//            {
//                // Admin sees all appointments
//                var allAppointments = await _context.Appointments
//                    .Include(a => a.Patient)
//                    .Include(a => a.Doctor)
//                    .ToListAsync();

//                return Ok(allAppointments);
//            }
//            else
//            {
//                // Staff / Patient sees only their appointments
//                if (!int.TryParse(userIdClaim, out int userId))
//                    return Unauthorized();

//                var myAppointments = await _context.Appointments
//                    .Include(a => a.Patient)
//                    .Include(a => a.Doctor)
//                    .Where(a => a.PatientId == userId)
//                    .ToListAsync();

//                return Ok(myAppointments);
//            }
//        }

//        // =========================================
//        // POST: Create appointment
//        // =========================================
//        [HttpPost]
//        public async Task<IActionResult> CreateAppointment([FromBody] Appointment appointment)
//        {
//            // Set PatientId from JWT claim if the user is a patient
//            var role = User.FindFirstValue("role");
//            var userIdClaim = User.FindFirstValue("userId");

//            if (role != "Admin")
//            {
//                if (!int.TryParse(userIdClaim, out int patientId))
//                    return Unauthorized();

//                appointment.PatientId = patientId;
//            }

//            _context.Appointments.Add(appointment);
//            await _context.SaveChangesAsync();

//            // Load navigation properties
//            await _context.Entry(appointment).Reference(a => a.Patient).LoadAsync();
//            await _context.Entry(appointment).Reference(a => a.Doctor).LoadAsync();

//            return Ok(appointment);
//        }

//        // =========================================
//        // DELETE: Delete an appointment (Admin only)
//        // =========================================
//        [HttpDelete("{id}")]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> DeleteAppointment(int id)
//        {
//            var appointment = await _context.Appointments.FindAsync(id);
//            if (appointment == null)
//                return NotFound();

//            _context.Appointments.Remove(appointment);
//            await _context.SaveChangesAsync();
//            return NoContent();
//        }

//        // =========================================
//        // PUT: Update appointment (Admin only)
//        // =========================================
//        [HttpPut("{id}")]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] Appointment updated)
//        {
//            var appointment = await _context.Appointments.FindAsync(id);
//            if (appointment == null)
//                return NotFound();

//            appointment.Date = updated.Date;
//            appointment.DoctorId = updated.DoctorId;
//            appointment.PatientId = updated.PatientId;
//            appointment.Reason = updated.Reason;
//            appointment.ReminderSent = updated.ReminderSent;

//            await _context.SaveChangesAsync();
//            return Ok(appointment);
//        }
//    }
//}
using HospitalProjBackend.Data;
using HospitalProjBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HospitalProjBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public AppointmentsController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var role = User.FindFirstValue("role");
            var userIdClaim = User.FindFirstValue("userId");

            if (role == "Admin")
            {
                var all = await _ctx.Appointments.Include(a => a.Patient).Include(a => a.Doctor).ToListAsync();
                return Ok(all);
            }

            int userId = int.Parse(userIdClaim);
            var mine = await _ctx.Appointments.Include(a => a.Doctor).Include(a => a.Patient)
                .Where(a => a.PatientId == userId).ToListAsync();
            return Ok(mine);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Appointment a)
        {
            var role = User.FindFirstValue("role");
            var userIdClaim = User.FindFirstValue("userId");

            if (role != "Admin")
            {
                int pid = int.Parse(userIdClaim);
                a.PatientId = pid;
            }

            _ctx.Appointments.Add(a);
            await _ctx.SaveChangesAsync();

            await _ctx.Entry(a).Reference(a => a.Patient).LoadAsync();
            await _ctx.Entry(a).Reference(a => a.Doctor).LoadAsync();

            return Ok(a);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var a = await _ctx.Appointments.FindAsync(id);
            if (a == null) return NotFound();

            _ctx.Appointments.Remove(a);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}
