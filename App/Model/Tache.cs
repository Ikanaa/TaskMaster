using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace taskMaster.Model
{
   
    public class Tache
    {
        public int id_tac{ get; set; }
        public List<Tache>? sous_tache { get; set; }
        public List<Commentaire>? commentaires { get; set; }
        public Projet? projet { get; set; }
        public User? auteur { get; set; }
        public User? assignee { get; set; }
        public string titre { get; set; }
        public string description { get; set; }
        public string statut { get; set; }
        public string priorite { get; set; }
        public string categorie { get; set; }
        public string etiquettes { get; set; }
        public DateTime date_creation { get; set; }


        public Tache(int id, string titre, string description, string statut, string priorite, string categorie, string etiquettes, List<Commentaire>? commentaires, DateTime date_creation)
        {
            this.id_tac = id;
            this.titre = titre;
            this.description = description;
            this.statut = statut;
            this.priorite = priorite;
            this.categorie = categorie;
            this.etiquettes = etiquettes;
            this.date_creation = date_creation;
            this.commentaires = commentaires;

        }



    }
}
