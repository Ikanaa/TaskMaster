// Models/Projet.cs

using System.ComponentModel.DataAnnotations;

namespace EntityFramework.Models
    {
        public class Projet
        {
            [Key]
            public int Id_pro { get; set; }
            public string Nom { get; set; } = null!;
            public string? Description { get; set; }
            public DateTime Date_creation { get; set; }
            
            // Navigation properties
            public ICollection<Tache> Taches { get; set; } = new List<Tache>();
            public ICollection<UtilisateurProjet> UtilisateurProjets { get; set; } = new List<UtilisateurProjet>();
        }
    }