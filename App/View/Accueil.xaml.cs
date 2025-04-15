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



        // Exemple de t�ches initiales
        Tasks = new List<Tache>
        {
            new Tache(1, "T�che 1", "Description de la t�che 1", "En cours", "Haute", "D�veloppement", "Urgent",Commentaire, DateTime.Now),
            new Tache(2, "T�che 2", "Description de la t�che 2", "� faire", "Moyenne", "Test", "Important",Commentaire ,DateTime.Now),
            new Tache(1, "T�che 1", "Description de la t�che 1", "En cours", "Haute", "D�veloppement", "Urgent",Commentaire, DateTime.Now),
            new Tache(2, "T�che 2", "Description de la t�che 2", "� faire", "Moyenne", "Test", "Important",Commentaire, DateTime.Now),
            new Tache(1, "T�che 1", "Description de la t�che 1", "En cours", "Haute", "D�veloppement", "Urgent",Commentaire, DateTime.Now),
            new Tache(2, "T�che 2", "Description de la t�che 2", "� faire", "Moyenne", "Test", "Important",Commentaire, DateTime.Now),
            new Tache(1, "T�che 1", "Description de la t�che 1", "En cours", "Haute", "D�veloppement", "Urgent",Commentaire, DateTime.Now),
            new Tache(2, "T�che 2", "Description de la t�che 2", "� faire", "Moyenne", "Test", "Important",Commentaire, DateTime.Now),
            new Tache(1, "T�che 1", "Description de la t�che 1", "En cours", "Haute", "D�veloppement", "Urgent",Commentaire, DateTime.Now),
            new Tache(2, "T�che 2", "Description de la t�che 2", "� faire", "Moyenne", "Test", "Important",Commentaire, DateTime.Now)
        };

        // Lier la liste des t�ches � la CollectionView
        TaskListView.ItemsSource = Tasks;
    }

    private void OnDeleteTaskButtonClicked(object sender, EventArgs e)
    {
        // R�cup�rer la t�che � supprimer � partir du CommandParameter
        var button = sender as Button;
        var taskToDelete = button?.CommandParameter as Tache;

        if (taskToDelete != null)
        {
            // Supprimer la t�che de la liste
            Tasks.Remove(taskToDelete);

            // Rafra�chir la CollectionView
            TaskListView.ItemsSource = null;
            TaskListView.ItemsSource = Tasks;
        }
    }

    private async void OnTaskSelected(object sender, SelectionChangedEventArgs e)
    {
        // R�cup�rer la t�che s�lectionn�e
        var selectedTask = e.CurrentSelection.FirstOrDefault() as Tache;

        if (selectedTask != null)
        {
            // Naviguer vers la page des d�tails de la t�che
            await Navigation.PushAsync(new DetailTache(selectedTask));

            // D�s�lectionner l'�l�ment apr�s la navigation
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    
        // Naviguer vers le formulaire d'ajout de t�che
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
        // R�cup�rer la t�che � modifier � partir du CommandParameter
        var button = sender as Button;
        var taskToModify = button?.CommandParameter as Tache;

        if (taskToModify != null)
        {
            // Naviguer vers la page ModificationTask et passer la t�che
            await Navigation.PushAsync(new ModificationTask(taskToModify, OnTaskModified));
        }
    }

    // Callback pour g�rer la modification de la t�che
    private void OnTaskModified(Tache modifiedTask)
    {
        if (modifiedTask != null)
        {
            // Rafra�chir la CollectionView
            TaskListView.ItemsSource = null;
            TaskListView.ItemsSource = Tasks;
        }
    }

}