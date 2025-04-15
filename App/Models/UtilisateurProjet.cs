// Models/UtilisateurProjet.cs

using System.ComponentModel.DataAnnotations;

namespace EntityFramework.Models
    {
        public class UtilisateurProjet
        {
            [Key]
            public int Utilisateur_id { get; set; }
            [Key]
            public int Projet_id { get; set; }
            public DateTime Date_ajout { get; set; }
            
            // Navigation properties
            public Utilisateur Utilisateur { get; set; } = null!;
            public Projet Projet { get; set; } = null!;
        }
    }