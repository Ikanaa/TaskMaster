using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskMaster.Model
{
    public class User
    {
        public int id_uti { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string email { get; set; }
        public string mot_de_passe { get; set; }
        public DateTime date_inscription { get; set; }

        public User()
        {
            
        }

            public User(int id, string nom, string prenom, string email, string mot_de_passe, DateTime date)
        {
            this.id_uti = id;
            this.nom = nom;
            this.prenom = prenom;
            this.email = email;
            this.mot_de_passe = mot_de_passe;
            this.date_inscription = date;
        }
    }
}
