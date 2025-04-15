// Models/Tache.cs

using System.ComponentModel.DataAnnotations;

namespace EntityFramework.Models
    {
        public class Tache
        {
            [Key]
            public int Id_tac { get; set; }
            public int? Parent_tache_id { get; set; }
            public int Projet_id { get; set; }
            public int Auteur_id { get; set; }
            public int? Assignee_id { get; set; }
            public string Titre { get; set; } = null!;
            public string? Description { get; set; }
            public string Statut { get; set; } = "À faire";
            public string Priorite { get; set; } = "Normal";
            public string? Categorie { get; set; }
            public string? Etiquettes { get; set; }
            public DateTime Date_creation { get; set; }
            
            // Navigation properties
            public Tache? ParentTache { get; set; }
            public ICollection<Tache> SousTaches { get; set; } = new List<Tache>();
            public Projet Projet { get; set; } = null!;
            public Utilisateur Auteur { get; set; } = null!;
            public Utilisateur? Assignee { get; set; }
            public ICollection<Commentaire> Commentaires { get; set; } = new List<Commentaire>();
        }
    }