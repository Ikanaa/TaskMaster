using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskMaster.Model
{
    public class Projet
    {
        public int id_pro { get; set; }
        public string nom { get; set; }
        public string description { get; set; }
        public DateTime date_creation { get; set; }
        public List<User> utilisateurs_projet { get; set; }
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
