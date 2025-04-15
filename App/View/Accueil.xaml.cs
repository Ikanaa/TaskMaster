using EntityFramework.Data;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;



namespace App.View;

public partial class Accueil : ContentPage
{
    public List<Tache> Tasks { get; set; }

    public List<Commentaire> Commentaire { get; set; } = new List<Commentaire>();



    public Utilisateur utilisateur { get; set; } = new Utilisateur();

    public Commentaire com1 = new Commentaire();
    public Commentaire com2 = new Commentaire();
    public Commentaire com3 = new Commentaire();

    public Tache task1 = new Tache();
    public Tache task2 = new Tache();
    public Tache task3 = new Tache();
    public Tache task4 = new Tache();
    public Tache task5 = new Tache();

    public Accueil()
    {
        InitializeComponent();
       


            utilisateur.Id_uti = 65;
            utilisateur.Nom = "moulin";
            utilisateur.Prenom = "jean";
            utilisateur.Email = "Email@gmail.com";
            utilisateur.Mot_de_passe = "oui oui";
            utilisateur.Date_inscription = DateTime.Now;
            
            com1.Id_com = 1;
            com1.Contenu = "Commentaire 1";
            com1.Utilisateur = utilisateur;

            com1.Id_com = 2;
            com1.Contenu = "Commentaire 2";
            com1.Utilisateur = utilisateur;

            com1.Id_com = 3;
            com1.Contenu = "Commentaire 3";
            com1.Utilisateur = utilisateur;

            task1.Id_tac = 1;
            task1.Auteur = utilisateur;
            task1.Assignee = utilisateur;
            task1.Commentaires = Commentaire;
            task1.Date_creation = DateTime.Now;
            task1.Statut = "En cours";
            task1.Priorite = "Haute";
            task1.Description = "Description de la tâche 1";
            task1.Titre = "Tâche 1";
            task1.Categorie = "Développement";

            task2.Id_tac = 2;
            task2.Auteur = utilisateur;
            task2.Assignee = utilisateur;
            task2.Commentaires = Commentaire;
            task2.Date_creation = DateTime.Now;
            task2.Statut = "À faire";
            task2.Priorite = "Moyenne";
            task2.Description = "Description de la tâche 2";
            task2.Titre = "Tâche 2";
            task2.Categorie = "Test";




        Commentaire = new List<Commentaire>
        {
            com1,
            com2,
            com3
            
        }; 



        // Exemple de tâches initiales
        Tasks = new List<Tache>
        {
            task1,
            task2,
            
        };

        // Lier la liste des tâches à la CollectionView
        TaskListView.ItemsSource = Tasks;
    }

    private void OnDeleteTaskButtonClicked(object sender, EventArgs e)
    {
        // Récupérer la tâche à supprimer à partir du CommandParameter
        var button = sender as Button;
        var taskToDelete = button?.CommandParameter as Tache;

        if (taskToDelete != null)
        {
            // Supprimer la tâche de la liste
            Tasks.Remove(taskToDelete);

            // Rafraîchir la CollectionView
            TaskListView.ItemsSource = null;
            TaskListView.ItemsSource = Tasks;
        }
    }

    private async void OnTaskSelected(object sender, SelectionChangedEventArgs e)
    {
        // Récupérer la tâche sélectionnée
        var selectedTask = e.CurrentSelection.FirstOrDefault() as Tache;

        if (selectedTask != null)
        {
            // Naviguer vers la page des détails de la tâche
            await Navigation.PushAsync(new DetailTache(selectedTask));

            // Désélectionner l'élément après la navigation
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    
        // Naviguer vers le formulaire d'ajout de tâche
    private async void OnAddTaskButtonClicked(object sender, EventArgs e)
    {
        // Pass a callback to handle the task addition
        await Navigation.PushAsync(new AjoutTask(OnTaskAdded));
    }

    // Callback method to handle the task addition
    private void OnTaskAdded(Tache newTask)
    {
        if (newTask != null)
        {
            // Add the new task to the list
            Tasks.Add(newTask);

            // Refresh the CollectionView
            TaskListView.ItemsSource = null;
            TaskListView.ItemsSource = Tasks;
        }
    }

    private async void OnModifyTaskButtonClicked(object sender, EventArgs e)
    {
        // Récupérer la tâche à modifier à partir du CommandParameter
        var button = sender as Button;
        var taskToModify = button?.CommandParameter as Tache;

        if (taskToModify != null)
        {
            // Naviguer vers la page ModificationTask et passer la tâche
            await Navigation.PushAsync(new ModificationTask(taskToModify, OnTaskModified));
        }
    }

    // Callback pour gérer la modification de la tâche
    private void OnTaskModified(Tache modifiedTask)
    {
        if (modifiedTask != null)
        {
            // Rafraîchir la CollectionView
            TaskListView.ItemsSource = null;
            TaskListView.ItemsSource = Tasks;
        }
    }

    private async void OnTestConnectionClicked(object sender, EventArgs e)
    {
        await TestDatabaseConnection();
    }
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


}