using App.ViewModels;
using EntityFramework.Models;

namespace App.View;

public partial class DetailTache : ContentPage
{
    public DetailTaskViewModel ViewModel { get; set; }

    public DetailTache(Tache tache)
    {
        InitializeComponent();
        ViewModel = new DetailTaskViewModel(tache);
        BindingContext = ViewModel;
    }


    private async void OnDeleteCommentButtonClicked(object sender, EventArgs e)
    {
        Button button = sender as Button;
        Commentaire comment = button?.CommandParameter as Commentaire;

        if (comment != null)
        {
            await ViewModel.DeleteComment(comment);
        }
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Rafraîchir les commentaires à chaque fois que la page apparaît
        await ViewModel.RefreshComments();
        await ViewModel.RefreshSousTaches();
    }
}