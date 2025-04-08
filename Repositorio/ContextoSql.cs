using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Repositorio;

public class ContextoSql : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Promocion> Promociones { get; set; }
    public DbSet<Deposito> Depositos { get; set; }
    public DbSet<Reserva> Reservas { get; set; }

    public ContextoSql(DbContextOptions<ContextoSql> opciones) : base(opciones) {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Usuario>().HasKey(usuario => usuario.Email);

        modelBuilder.Entity<Deposito>()
            .HasMany<Promocion>(x => x.ListaPromociones)
            .WithMany(x => x.listaDepositos);

        modelBuilder.Entity<Deposito>()
            .HasMany<RangoDeFechas>(x => x.ListaDisponibilidad)
            .WithMany(x => x.listaDepositos);
        
        modelBuilder.Entity<Reserva>()
            .Property(r => r.MotivoRechazo)
            .IsRequired(false);
    }
}