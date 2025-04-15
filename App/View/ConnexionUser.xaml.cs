namespace App.View;

public partial class ConnexionUser : ContentPage
{
	public ConnexionUser()
	{
		InitializeComponent();
	}


    private void OnLoginButtonClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            MessageLabel.Text = "Tous les champs sont obligatoires.";
            return;
        }
        // Logique de connexion (exemple : validation ou envoi au serveur)
        MessageLabel.Text = "Connexion réussie !";
        MessageLabel.TextColor = Colors.Green;

        if (MessageLabel.Text == "Connexion réussie !")
        {
            loadHome();
        }
    }

    private async void loadHome()
    {
        await Navigation.PushAsync(new Accueil());
    }


    private async void OnReturnButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InscriptionUser());
    }

}