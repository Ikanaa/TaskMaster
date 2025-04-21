using App.ViewModels;
using EntityFramework.Models;

namespace App.View;

public partial class GestionUtilisateurs : ContentPage
{
    public GestionUtilisateursViewModel ViewModel { get; set; }

    public GestionUtilisateurs(Projet projet)
    {
        InitializeComponent();

        // Initialiser le ViewModel avec le projet sélectionné
        ViewModel = new GestionUtilisateursViewModel(projet);
        BindingContext = ViewModel;
    }

    private async void OnRetourButtonClicked(object sender, EventArgs e)
    {
        // Naviguer vers la page des projets
        await Navigation.PopAsync();
    }
}