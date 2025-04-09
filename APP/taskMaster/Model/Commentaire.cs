using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskMaster.Model
{
    public class Commentaire
    {
        private int id_com { get; set; }
        private string contenu { get; set; }   
        private DateTime date_creation { get; set; }
     

        public Commentaire(int id,string texte, DateTime date)
        {
            this.contenu = texte;
            this.date_creation = date;
            this.id_com = id;
        }   


        public Commentaire GetCommentaire()
        {
            return this;
        }




    }
}
