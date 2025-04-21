using EntityFramework.Data;
using EntityFramework.Models;

namespace App.View;

public partial class InscriptionUser : ContentPage
{
    public InscriptionUser()
    {
        InitializeComponent();
        VerifierConnexionBD();
    }

    private async void VerifierConnexionBD()
    {
        try
        {
            using (var context = new TaskmasterContext())
            {
                // Tenter une op�ration simple pour v�rifier la connexion
                await context.Database.CanConnectAsync();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur de connexion",
                "Impossible de se connecter � la base de donn�es. Veuillez v�rifier votre connexion internet et r�essayer.",
                "OK");

            MessageLabel.Text = "Erreur de connexion � la base de donn�es";
            MessageLabel.TextColor = Colors.Red;

            System.Diagnostics.Debug.WriteLine($"Erreur de connexion BD: {ex.Message}");
        }
    }

    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        string name = NameEntry.Text;
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        string firstName = FirstNameEntry.Text;
        string PasswordConfirm = PasswordConfirmEntry.Text;

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(PasswordConfirm))
        {
            MessageLabel.Text = "Tous les champs sont obligatoires.";
            MessageLabel.TextColor = Colors.Red;
            return;
        }
        else if (!email.Contains("@"))
        {
            MessageLabel.Text = "l'email doit comporter @";
            MessageLabel.TextColor = Colors.Red;
            return;
        }
        else if (password.Length < 8)
        {
            MessageLabel.Text = "le mot de passe doit conternir au minimum 8 charact�re";
            MessageLabel.TextColor = Colors.Red;
            return;
        }
        else if (password != PasswordConfirm)
        {
            MessageLabel.Text = "les mot de passe doivent etre identique";
            MessageLabel.TextColor = Colors.Red;
            return;
        }

        try
        {
            using (var context = new TaskmasterContext()) // lien vers la bdd  
            {
                var utilisateur = new Utilisateur
                {
                    Nom = name,
                    Prenom = firstName,
                    Email = email,
                    Mot_de_passe = password,
                    Date_inscription = DateTime.Now,
                };

                context.Utilisateurs.Add(utilisateur); // ajout de l'utilisateur � la bdd
                await context.SaveChangesAsync(); // sauvegarde des modifications

                // Connecter automatiquement l'utilisateur
                Session.UtilisateurConnecte = utilisateur;
            }

            // Logique d'inscription (exemple : validation ou envoi au serveur)
            MessageLabel.Text = "Inscription r�ussie !";
            MessageLabel.TextColor = Colors.Green;
            await DisplayAlert("Succ�s", "Inscription r�ussi", "OK");
            await Navigation.PushAsync(new ConnexionUser());
        }
        catch (Exception ex)
        {
            MessageLabel.Text = "Erreur de connexion � la base de donn�es";
            MessageLabel.TextColor = Colors.Red;
            await DisplayAlert("Erreur", $"Impossible de se connecter � la base de donn�es: {ex.Message}", "OK");
            System.Diagnostics.Debug.WriteLine($"Erreur lors de l'inscription: {ex.Message}");
        }
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ConnexionUser());
    }
}
