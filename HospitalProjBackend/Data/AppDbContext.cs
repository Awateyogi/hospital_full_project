using HospitalProjBackend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace HospitalProjBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Appointment> Appointments => Set<Appointment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed doctors and patients
            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { Id = 1, Name = "Dr. Alice Smith", Specialty = "General" },
                new Doctor { Id = 2, Name = "Dr. Bob Lee", Specialty = "Dermatology" }
            );
            modelBuilder.Entity<Patient>().HasData(
                new Patient { Id = 1, Name = "John Doe", Phone = "0500000001", Email = "john@example.com" },
                new Patient { Id = 2, Name = "Jane Roe", Phone = "0500000002", Email = "jane@example.com" }
            );

            // Seed admin user (hashed password)
            var hasher = new PasswordHasher<User>();
            var admin = new User
            {
                Id = 1,
                Username = "admin",
                Role = "Admin",
                PasswordHash = ""
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin@123");
            modelBuilder.Entity<User>().HasData(admin);
        }
    }
}
