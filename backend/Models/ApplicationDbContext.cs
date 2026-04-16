using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Estatus> Estatus { get; set; }
        public DbSet<RamaJuridica> RamasJuridicas { get; set; }
        public DbSet<Caso> Casos { get; set; }
        public DbSet<Cita> Citas { get; set; }
    }
}