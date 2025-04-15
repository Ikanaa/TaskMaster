using System.Collections.ObjectModel;
using System.Windows.Input;
using taskMaster.Model;

namespace App.View;

public partial class ModificationTask : ContentPage
{
    public string Titre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Statut { get; set; } = string.Empty;
    public string Priorite { get; set; } = string.Empty;
    public string Categorie { get; set; } = string.Empty;

    public ObservableCollection<string> StatutOptions { get; set; }
    public ObservableCollection<string> PrioriteOptions { get; set; }
    public ObservableCollection<string> CategorieOptions { get; set; }

    public ICommand SaveTaskCommand { get; }
    public ICommand CancelCommand { get; }

    private Tache CurrentTask;
   
    private Action<Tache> OnTaskModified;

    public ModificationTask(Tache task, Action<Tache> onTaskModified)
    {
        InitializeComponent();

        // Initialiser les options pour les Pickers
        StatutOptions = new ObservableCollection<string> { "� faire", "En cours", "Termin�" };
        PrioriteOptions = new ObservableCollection<string> { "Basse", "Moyenne", "Haute" };
        CategorieOptions = new ObservableCollection<string> { "D�veloppement", "Test", "Design" };

        // Charger les donn�es de la t�che existante
        CurrentTask = task;
        Titre = task.titre;
        Description = task.description;
        Statut = task.statut;
        Priorite = task.priorite;
        Categorie = task.categorie;

        OnTaskModified = onTaskModified;

        // Initialiser les commandes
        SaveTaskCommand = new Command(async () => await OnSaveTask());
        CancelCommand = new Command(async () => await OnCancel());

        BindingContext = this;
    }

    private async Task OnSaveTask()
    {
        if (string.IsNullOrWhiteSpace(Titre) || string.IsNullOrWhiteSpace(Description) ||
            string.IsNullOrWhiteSpace(Statut) || string.IsNullOrWhiteSpace(Priorite) ||
            string.IsNullOrWhiteSpace(Categorie))
        {
            await DisplayAlert("Erreur", "Tous les champs doivent �tre remplis.", "OK");
            return;
        }

        // Mettre � jour les propri�t�s de la t�che
        CurrentTask.titre = Titre;
        CurrentTask.description = Description;
        CurrentTask.statut = Statut;
        CurrentTask.priorite = Priorite;
        CurrentTask.categorie = Categorie;

        // Appeler le callback pour signaler la modification
        OnTaskModified?.Invoke(CurrentTask);

        // Afficher un message de succ�s
        await DisplayAlert("Succ�s", "La t�che a �t� modifi�e avec succ�s.", "OK");

        // Revenir � la page pr�c�dente
        await Navigation.PopAsync();
    }

    private async Task OnCancel()
    {
        // Annuler et revenir � la page pr�c�dente
        await Navigation.PopAsync();
    }
}
