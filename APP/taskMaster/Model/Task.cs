using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace taskMaster.Model
{
   
    public class Task
    {
        private int id_tac{ get; set; }
        private List<Task> sous_tache { get; set; }
        private List<Commentaire> commentaires { get; set; }
        private Projet projet { get; set; }
        private User auteur { get; set; }
        private User assignee { get; set; }
        private string titre { get; set; }
        private string description { get; set; }
        private string statut { get; set; }
        private string priorite { get; set; }
        private string categorie { get; set; }
        private string etiquettes { get; set; }
        private DateTime date_creation { get; set; }


        public Task(int id, string titre, string description, string statut, string priorite, string categorie, string etiquettes, DateTime date_creation)
        {
            this.id_tac = id;
            this.titre = titre;
            this.description = description;
            this.statut = statut;
            this.priorite = priorite;
            this.categorie = categorie;
            this.etiquettes = etiquettes;
            this.date_creation = date_creation;

        }



    }
}
