using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ATS.Domain.Entities;

namespace ATS.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Country> countries { get; set; }
        public DbSet<City> Cities { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Education>()
                .HasOne(e => e.Resume)
                .WithMany(r => r.EducationDetails)
                .HasForeignKey(e => e.ResumeId);

            modelBuilder.Entity<WorkExperience>()
            .HasOne(w => w.Resume)
            .WithMany(r => r.WorkExperiences)
            .HasForeignKey(w => w.ResumeId);

            modelBuilder.Entity<Resume>()
        .HasOne(r => r.User)
        .WithMany(u => u.Resumes)
        .HasForeignKey(r => r.UserId);
            modelBuilder.Entity<Skills>()
               .HasOne(e => e.Resume)
               .WithMany(r => r.Skills)
               .HasForeignKey(e => e.ResumeId);

            modelBuilder.Entity<City>()
              .HasOne(c => c.Country)
              .WithMany(c => c.Cities)
              .HasForeignKey(c => c.CountryId);

            base.OnModelCreating(modelBuilder);




        }
    }
}
