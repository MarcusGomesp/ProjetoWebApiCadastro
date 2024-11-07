using Microsoft.EntityFrameworkCore;
using WFConfin.Models;

namespace WFConfin.Data
{
    public class WfConfinDbContext : DbContext //herdando de DbContext
    {
        public WfConfinDbContext(DbContextOptions<WfConfinDbContext> options) : base(options)
        {

        }

        public DbSet<Cidade> Cidade { get; set; }
        public DbSet<Conta> Conta { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<Pessoa> Pessoa { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}
