// Data/TaskmasterContext.cs
    using EntityFramework.Models;
    using Microsoft.EntityFrameworkCore;
    
    namespace EntityFramework.Data
    {
        public class TaskmasterContext : DbContext
        {
            public DbSet<Utilisateur> Utilisateurs { get; set; } = null!;
            public DbSet<Projet> Projets { get; set; } = null!;
            public DbSet<Tache> Taches { get; set; } = null!;
            public DbSet<Commentaire> Commentaires { get; set; } = null!;
            public DbSet<UtilisateurProjet> UtilisateurProjets { get; set; } = null!;

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         {
            try
            {
                optionsBuilder.UseMySql(
                    "server=localhost;database=taskmaster;user=root;password=rootpassword;",
                    new MySqlServerVersion(new Version(10, 5, 0)) // Remplacez par la version de votre MariaDB
                );
            }
            catch (Exception ex)
            {
                // Log de l'erreur pour le débogage
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la configuration de la base de données : {ex.Message}");
                // Vous pouvez également lever une exception personnalisée si nécessaire
                throw new InvalidOperationException("Impossible de configurer la base de données.", ex);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Table names
                modelBuilder.Entity<Utilisateur>().ToTable("UTILISATEUR");
                modelBuilder.Entity<Projet>().ToTable("PROJET");
                modelBuilder.Entity<Tache>().ToTable("TACHE");
                modelBuilder.Entity<Commentaire>().ToTable("COMMENTAIRE");
                modelBuilder.Entity<UtilisateurProjet>().ToTable("UTILISATEUR_PROJET");
                
                // Primary keys
                modelBuilder.Entity<UtilisateurProjet>().HasKey(up => new { up.Utilisateur_id, up.Projet_id });
                
                // Relationships
                modelBuilder.Entity<Tache>()
                    .HasOne(t => t.ParentTache)
                    .WithMany(t => t.SousTaches)
                    .HasForeignKey(t => t.Parent_tache_id)
                    .OnDelete(DeleteBehavior.SetNull);
                    
                modelBuilder.Entity<Tache>()
                    .HasOne(t => t.Projet)
                    .WithMany(p => p.Taches)
                    .HasForeignKey(t => t.Projet_id)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                modelBuilder.Entity<Tache>()
                    .HasOne(t => t.Auteur)
                    .WithMany(u => u.TachesCreees)
                    .HasForeignKey(t => t.Auteur_id)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                modelBuilder.Entity<Tache>()
                    .HasOne(t => t.Assignee)
                    .WithMany(u => u.TachesAssignees)
                    .HasForeignKey(t => t.Assignee_id)
                    .OnDelete(DeleteBehavior.SetNull);
                    
                modelBuilder.Entity<Commentaire>()
                    .HasOne(c => c.Tache)
                    .WithMany(t => t.Commentaires)
                    .HasForeignKey(c => c.Tache_id)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                modelBuilder.Entity<Commentaire>()
                    .HasOne(c => c.Utilisateur)
                    .WithMany(u => u.Commentaires)
                    .HasForeignKey(c => c.Utilisateur_id)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                modelBuilder.Entity<UtilisateurProjet>()
                    .HasOne(up => up.Utilisateur)
                    .WithMany(u => u.UtilisateurProjets)
                    .HasForeignKey(up => up.Utilisateur_id)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                modelBuilder.Entity<UtilisateurProjet>()
                    .HasOne(up => up.Projet)
                    .WithMany(p => p.UtilisateurProjets)
                    .HasForeignKey(up => up.Projet_id)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }