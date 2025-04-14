// Models/Utilisateur.cs

using System.ComponentModel.DataAnnotations;

namespace EntityFramework.Models
{
    public class Utilisateur
    {
        [Key]
        public int Id_uti { get; set; }
        public string Nom { get; set; } = null!;
        public string Prenom { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Mot_de_passe { get; set; } = null!;
        public DateTime Date_inscription { get; set; }
        
        // Navigation properties
        public ICollection<Tache> TachesCreees { get; set; } = new List<Tache>();
        public ICollection<Tache> TachesAssignees { get; set; } = new List<Tache>();
        public ICollection<Commentaire> Commentaires { get; set; } = new List<Commentaire>();
        public ICollection<UtilisateurProjet> UtilisateurProjets { get; set; } = new List<UtilisateurProjet>();
    }
}