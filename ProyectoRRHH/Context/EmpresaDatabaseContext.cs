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
    }

}

