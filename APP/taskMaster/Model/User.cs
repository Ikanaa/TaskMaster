using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskMaster.Model
{
    public class User
    {
       private int id_uti { get; set; }
       private  string nom { get; set; }
        private string prenom { get; set; }
        private string email { get; set; }
        private string mot_de_passe { get; set; }
        private  DateTime date_inscription { get; set; }

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
