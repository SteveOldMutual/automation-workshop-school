using Automation_School_API.Models.Identity;
using Automation_School_API.Models.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Automation_School_API.Context
{
    public class SchoolDBContext: DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Rollcall> RollCall { get; set; }
        public SchoolDBContext(DbContextOptions<SchoolDBContext> options, IConfiguration configuration)
        : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rollcall>()
                .HasKey(rc => rc.RollCallId);

            modelBuilder.Entity<Rollcall>()
                .HasOne(rc => rc.Student)
                .WithMany()
                .HasForeignKey(rc => rc.studentId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
