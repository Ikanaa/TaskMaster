using EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Session
{
    // Propriété pour stocker l'utilisateur connecté
    public static Utilisateur? UtilisateurConnecte { get; set; }

    // Méthode pour vérifier si un utilisateur est connecté
    public static bool EstConnecte()
    {
        return UtilisateurConnecte != null;
    }

    // Méthode pour déconnecter l'utilisateur
    public static void Deconnecter()
    {
        UtilisateurConnecte = null;
    }
}
