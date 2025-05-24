using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using GP_Backend.Models;
using Microsoft.AspNetCore.Identity;

namespace GP_Backend.Data;

/// <summary>
/// classe responsável pela criação e gestão da Base de dados
/// </summary>
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    //definição das 'tabelas' 
    public DbSet<Utilizadores> Utilizadores { get; set; }
    public DbSet<Escolas> Escolas { get; set; }
    public DbSet<Salas> Salas { get; set; }
    public DbSet<Docentes> Docentes { get; set; }
    public DbSet<Cursos> Cursos { get; set; }
    public DbSet<UnidadesCurriculares> UCs { get; set; }
    public DbSet<Turmas> Turmas { get; set; }
    public DbSet<Horarios> Horarios { get; set; }
    public DbSet<ManchasHorarias> ManchasHorarias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        // Criar dados das ROLES
        modelBuilder.Entity<IdentityRole>().HasData(
           new IdentityRole { Id = "A", Name = "Administrador", NormalizedName = "ADMINISTRADOR" },
           new IdentityRole { Id = "CH", Name = "ComissaoHorarios", NormalizedName = "COMISSAOHORARIOS" },
           new IdentityRole { Id = "CC", Name = "ComissaoCurso", NormalizedName = "COMISSAOCURSO" }

        );
    }

}

