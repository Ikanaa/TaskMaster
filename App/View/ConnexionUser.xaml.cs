using EntityFramework.Data;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
namespace App.View;

public partial class ConnexionUser : ContentPage
{
    public ConnexionUser()
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
                // Tenter une opération simple pour vérifier la connexion
                await context.Database.CanConnectAsync();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur de connexion",
                "Impossible de se connecter à la base de données. Veuillez vérifier votre connexion internet et réessayer.",
                "OK");

            MessageLabel.Text = "Erreur de connexion à la base de données";
            MessageLabel.TextColor = Colors.Red;

            System.Diagnostics.Debug.WriteLine($"Erreur de connexion BD: {ex.Message}");
        }
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            MessageLabel.Text = "Tous les champs sont obligatoires.";
            MessageLabel.TextColor = Colors.Red;
            return;
        }

        try
        {
            using (var context = new TaskmasterContext())
            {
                var utilisateur = await context.Utilisateurs
                    .FirstOrDefaultAsync(u => u.Email == email && u.Mot_de_passe == password);

                if (utilisateur == null)
                {
                    MessageLabel.Text = "Email ou mot de passe incorrect.";
                    MessageLabel.TextColor = Colors.Red;
                    return;
                }

                // Connecter l'utilisateur
                Session.UtilisateurConnecte = utilisateur;
            }

            // Logique de connexion (exemple : validation ou envoi au serveur)
            MessageLabel.Text = "Connexion réussie !";
            MessageLabel.TextColor = Colors.Green;
            // await DisplayAlert("Succès", "connexion réussi", "OK");

            loadHome();
        }
        catch (Exception ex)
        {
            MessageLabel.Text = "Erreur de connexion à la base de données";
            MessageLabel.TextColor = Colors.Red;
            await DisplayAlert("Erreur", $"Impossible de se connecter à la base de données: {ex.Message}", "OK");
            System.Diagnostics.Debug.WriteLine($"Erreur lors de la connexion: {ex.Message}");
        }
    }

    private async void loadHome()
    {
        await Navigation.PushAsync(new Projets());
    }

    private async void OnReturnButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InscriptionUser());
    }
}
