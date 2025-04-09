using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskMaster.Model
{
    public class Projet
    {
        private int id_pro { get; set; }
        private string nom { get; set; }
        private string description { get; set; }
        private DateTime date_creation { get; set; }
        private List<User> utilisateurs_projet { get; set; }
        public Projet(int id, string nom, string description, List<User> utilisateurs, DateTime date)
        {
            this.id_pro = id;
            this.nom = nom;
            this.description = description;
            this.date_creation = date;
            this.utilisateurs_projet = utilisateurs;    
        }
    }
}
