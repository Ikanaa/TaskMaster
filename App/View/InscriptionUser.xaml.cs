namespace App.View;

public partial class InscriptionUser : ContentPage
{
    public InscriptionUser()
    {
        InitializeComponent();
    }
    /* protected override void OnAppearing()
    {
        base.OnAppearing();

        // Ajuster la taille de la fenêtre pour la page InscriptionUser
        var mainWindow = Application.Current?.Windows.FirstOrDefault();
        if (mainWindow != null)
        {
            mainWindow.Width = 400; // Largeur souhaitée
            mainWindow.Height = 600; // Hauteur souhaitée
        }
    }*/

    private void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        string name = NameEntry.Text;
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

       


        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            MessageLabel.Text = "Tous les champs sont obligatoires.";
            MessageLabel.TextColor = Colors.Red;
            return;
        }

        // Logique d'inscription (exemple : validation ou envoi au serveur)
        MessageLabel.Text = "Inscription réussie !";
        MessageLabel.TextColor = Colors.Green;
    }


    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ConnexionUser());
    }
}