// Models/Commentaire.cs

using System.ComponentModel.DataAnnotations;

namespace EntityFramework.Models
    {
        public class Commentaire
        {
            [Key]
            public int Id_com { get; set; }
            public int Tache_id { get; set; }
            public int Utilisateur_id { get; set; }
            public string Contenu { get; set; } = null!;
            public DateTime Date_creation { get; set; }
            
            // Navigation properties
            public Tache Tache { get; set; } = null!;
            public Utilisateur Utilisateur { get; set; } = null!;
        }
    }