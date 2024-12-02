    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ProyectoRRHH.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;


namespace ProyectoRRHH.Context
{
    public class EmpresaDatabaseContext : DbContext
    {
        public EmpresaDatabaseContext(DbContextOptions<EmpresaDatabaseContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ReciboSueldo> ReciboSueldos { get; set; }
        public DbSet<Licencia> Licencias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para la relación Usuario - Licencia
            modelBuilder.Entity<Licencia>()
                .HasOne(l => l.Usuario) // Relación con Usuario
                .WithMany(u => u.ListaLicencias) // Un usuario tiene muchas licencias
                .HasForeignKey(l => l.UsuarioDni) // Clave foránea
                .OnDelete(DeleteBehavior.Cascade); // Elimina en cascada
        }
    }
}