using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using App.View;
using EntityFramework.Data;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace App.ViewModels;

public class ProjetsViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Projet> Projets { get; set; } = new ObservableCollection<Projet>();

    public ICommand AddProjectCommand { get; }
    public ICommand DeleteProjectCommand { get; }

    public ICommand GererUtilisateursCommand { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    public ProjetsViewModel()
    {
        LoadProjects();

        AddProjectCommand = new Command(async () => await AddProject());
        DeleteProjectCommand = new Command<Projet>(async (projet) => await DeleteProject(projet));
        GererUtilisateursCommand = new Command<Projet>(async (projet) => await GererUtilisateurs(projet));
    }

    private async Task GererUtilisateurs(Projet projet)
    {
        if (projet == null) return;

        await App.Current.MainPage.Navigation.PushAsync(new GestionUtilisateurs(projet));
    }

    private async void LoadProjects()
    {
        using (var context = new TaskmasterContext())
        {
            var projets = await context.Projets.ToListAsync();
            Projets.Clear();
            foreach (var projet in projets)
            {
                Projets.Add(projet);
            }
        }
    }

    private async Task AddProject()
    {
        string nom = await App.Current.MainPage.DisplayPromptAsync("Nouveau Projet", "Entrez le nom du projet :");
        string description = await App.Current.MainPage.DisplayPromptAsync("Nouveau Projet", "Entrez une description :");

        if (string.IsNullOrWhiteSpace(nom)) return;

        var nouveauProjet = new Projet
        {
            Nom = nom,
            Description = description,
            Date_creation = DateTime.Now
        };

        using (var context = new TaskmasterContext())
        {
            context.Projets.Add(nouveauProjet);
            await context.SaveChangesAsync();
        }

        Projets.Add(nouveauProjet);
    }

    private async Task DeleteProject(Projet projet)
    {
        if (projet == null) return;

        bool confirm = await App.Current.MainPage.DisplayAlert("Confirmation", $"Voulez-vous supprimer le projet '{projet.Nom}' ?", "Oui", "Non");
        if (!confirm) return;

        using (var context = new TaskmasterContext())
        {
            context.Projets.Remove(projet);
            await context.SaveChangesAsync();
        }

        Projets.Remove(projet);
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}