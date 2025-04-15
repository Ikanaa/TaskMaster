using taskMaster.Model;



namespace App.View;

public partial class Accueil : ContentPage
{
    public List<Tache> Tasks { get; set; }

    public List<Commentaire> Commentaire { get; set; } = new List<Commentaire>();

    public User utilisateur { get; set; } = new User();


    public Accueil()
    {
        InitializeComponent();

            utilisateur.id_uti = 65;
            utilisateur.nom = "moulin";
            utilisateur.prenom = "jean";
            utilisateur.email = "Email@gmail.com";
            utilisateur.mot_de_passe = "oui oui";
            utilisateur.date_inscription = DateTime.Now;
        




        Commentaire = new List<Commentaire>
        {
            new Commentaire(1, "Commentaire alpha romeo ",utilisateur, DateTime.Now),
            new Commentaire(2, "Commentaire beta composition",utilisateur,DateTime.Now),
            new Commentaire(2, "Commentaire golden rise ",utilisateur, DateTime.Now)
        }; 



        // Exemple de tâches initiales
        Tasks = new List<Tache>
        {
            new Tache(1, "Tâche 1", "Description de la tâche 1", "En cours", "Haute", "Développement", "Urgent",Commentaire, DateTime.Now),
            new Tache(2, "Tâche 2", "Description de la tâche 2", "À faire", "Moyenne", "Test", "Important",Commentaire ,DateTime.Now),
            new Tache(1, "Tâche 1", "Description de la tâche 1", "En cours", "Haute", "Développement", "Urgent",Commentaire, DateTime.Now),
            new Tache(2, "Tâche 2", "Description de la tâche 2", "À faire", "Moyenne", "Test", "Important",Commentaire, DateTime.Now),
            new Tache(1, "Tâche 1", "Description de la tâche 1", "En cours", "Haute", "Développement", "Urgent",Commentaire, DateTime.Now),
            new Tache(2, "Tâche 2", "Description de la tâche 2", "À faire", "Moyenne", "Test", "Important",Commentaire, DateTime.Now),
            new Tache(1, "Tâche 1", "Description de la tâche 1", "En cours", "Haute", "Développement", "Urgent",Commentaire, DateTime.Now),
            new Tache(2, "Tâche 2", "Description de la tâche 2", "À faire", "Moyenne", "Test", "Important",Commentaire, DateTime.Now),
            new Tache(1, "Tâche 1", "Description de la tâche 1", "En cours", "Haute", "Développement", "Urgent",Commentaire, DateTime.Now),
            new Tache(2, "Tâche 2", "Description de la tâche 2", "À faire", "Moyenne", "Test", "Important",Commentaire, DateTime.Now)
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

}