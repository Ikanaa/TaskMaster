using App.ViewModels;
using EntityFramework.Data;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;



namespace App.View;

public partial class Accueil : ContentPage
{
    public TaskViewModel ViewModel { get; set; }
    private Projet _selectedProject;

    public Accueil()
    {
        InitializeComponent();
        ViewModel = new TaskViewModel();
        BindingContext = ViewModel;

    }

    public Accueil(Projet selectedProject)
    {
        InitializeComponent();
        ViewModel = new TaskViewModel();
        BindingContext = ViewModel;

        _selectedProject = selectedProject;
        LoadTasksForProject();
    }

    private async void LoadTasksForProject()
    {
        using (var context = new TaskmasterContext())
        {
            var tasks = await context.Taches
                .Where(t => t.Projet_id == _selectedProject.Id_pro)
                .ToListAsync();

            ViewModel.Tasks.Clear();
            foreach (var task in tasks)
            {
                ViewModel.Tasks.Add(task);
            }
        }
    }

    private async void OnAddTaskButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AjoutTask(OnTaskAdded, _selectedProject));
    }

    // Callback pour gérer la tâche ajoutée
    private void OnTaskAdded(Tache newTask)
    {
        if (newTask != null)
        {
            ViewModel.Tasks.Add(newTask);
        }
    }
    private async void OnTaskSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedTask = e.CurrentSelection.FirstOrDefault() as Tache;
        if (selectedTask != null)
        {
            await Navigation.PushAsync(new DetailTache(selectedTask));
            ((CollectionView)sender).SelectedItem = null;
        }
    }


    private async void OnModifyTaskButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var task = button?.BindingContext as Tache;

        if (task != null)
        {
            // Charger les utilisateurs associés au projet de la tâche
            ObservableCollection<Utilisateur> utilisateursAssocies;
            using (var context = new TaskmasterContext())
            {
                var utilisateurs = await context.UtilisateurProjets
                    .Where(up => up.Projet_id == task.Projet_id)
                    .Select(up => up.Utilisateur)
                    .ToListAsync();

                utilisateursAssocies = new ObservableCollection<Utilisateur>(utilisateurs);
            }

            // Naviguer vers la page de modification avec les utilisateurs associés
            await Navigation.PushAsync(new ModificationTask(task, utilisateursAssocies, OnTaskModified));
        }
    }

    // Callback pour gérer la tâche modifiée
    private void OnTaskModified(Tache modifiedTask)
    {
        ViewModel.LoadTasksCommand.Execute(null); // Recharger toutes les tâches
    }

    private async void OnProjectsButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Projets());
    }









    private async void OnLogoutButtonClicked(object sender, EventArgs e)
    {
        Session.Deconnecter();
        await Navigation.PushAsync(new ConnexionUser());
    }

    //test de la base de donnée 
    private async Task TestDatabaseConnection()
    {
        using (var context = new TaskmasterContext())
        {
            var utilisateurs = await context.Utilisateurs.ToListAsync();
            foreach (var utilisateur in utilisateurs)
            {
                System.Diagnostics.Debug.WriteLine($"Utilisateur : {utilisateur.Nom} {utilisateur.Prenom}");
            }
        }
    }

    private async void OnTestConnectionClicked(object sender, EventArgs e)
    {
        await TestDatabaseConnection();
    }


}