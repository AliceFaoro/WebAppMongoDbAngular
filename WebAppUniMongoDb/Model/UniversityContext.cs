using Microsoft.EntityFrameworkCore;

namespace WebAppUniMongoDb.Model
{
    public class UniversityContext : DbContext
    {
        public UniversityContext()
        {
        }

        public UniversityContext(DbContextOptions<UniversityContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Faculty> Faculty { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Professor> Professor { get; set; }
    }
}
