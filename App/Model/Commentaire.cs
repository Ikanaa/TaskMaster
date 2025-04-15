using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskMaster.Model
{
    public class Commentaire
    {
        public User utilisateur { get; set; }
        public int id_com { get; set; }
        public string contenu { get; set; }
        public DateTime date_creation { get; set; }
     

        public Commentaire(int id,string texte, User utilisateur, DateTime date)
        {
            this.utilisateur = utilisateur;
            this.contenu = texte;
            this.date_creation = date;
            this.id_com = id;
        }   





    }
}
