using Microsoft.EntityFrameworkCore;
using desawebback.Models;

namespace desawebback.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    public DbSet<Equipo> Equipos { get; set; }
    public DbSet<Jugador> Jugadores { get; set; }
    public DbSet<Partido> Partidos { get; set; }
    public DbSet<JugadorPartido> JugadoresPartidos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Role>()
            .HasMany(r => r.Users)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId);

      
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<JugadorPartido>()
            .HasKey(jp => new { jp.JugadorId, jp.PartidoId });

        modelBuilder.Entity<JugadorPartido>()
            .HasOne(jp => jp.Jugador)
            .WithMany(j => j.PartidosJugados)
            .HasForeignKey(jp => jp.JugadorId);

        modelBuilder.Entity<JugadorPartido>()
            .HasOne(jp => jp.Partido)
            .WithMany(p => p.Roster)
            .HasForeignKey(jp => jp.PartidoId);

        modelBuilder.Entity<Partido>()
            .HasOne(p => p.EquipoLocal)
            .WithMany(e => e.PartidosLocal)
            .HasForeignKey(p => p.EquipoLocalId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Partido>()
            .HasOne(p => p.EquipoVisitante)
            .WithMany(e => e.PartidosVisitante)
            .HasForeignKey(p => p.EquipoVisitanteId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}