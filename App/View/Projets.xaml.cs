using EntityFramework.Models;

namespace App.View;

public partial class Projets : ContentPage
{
	public Projets()
	{
		InitializeComponent();
	}

    private async void OnProjectSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedProject = e.CurrentSelection.FirstOrDefault() as Projet;
        if (selectedProject != null)
        {
            await Navigation.PushAsync(new Accueil(selectedProject));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
    private async void OnLogoutButtonClicked(object sender, EventArgs e)
    {
        Session.Deconnecter();
        await Navigation.PushAsync(new ConnexionUser());
    }

    private async void OnManageUsersButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var selectedProject = button?.BindingContext as Projet;

        if (selectedProject != null)
        {
            await Navigation.PushAsync(new GestionUtilisateurs(selectedProject));
        }
    }



}