using AppRod.Model;
using Microsoft.EntityFrameworkCore;

namespace AppRod.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) 
        {
        
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Unidade> Unidades { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Colaborador>()
                .HasOne(c => c.Unidade)
                .WithMany(u => u.Colaboradores)
                .HasForeignKey(c => c.fk_Unidade);

            modelBuilder.Entity<Colaborador>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.fk_Usuario);
        }
    }
}
