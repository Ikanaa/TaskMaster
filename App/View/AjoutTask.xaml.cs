using System.Collections.ObjectModel;
using System.Windows.Input;
using taskMaster.Model;

namespace App.View;

public partial class AjoutTask : ContentPage
{
    public string Titre { get; set; }
    public string Description { get; set; }
    public string Statut { get; set; }
    public string Priorite { get; set; }
    public string Categorie { get; set; }

    public ObservableCollection<string> StatutOptions { get; set; }
    public ObservableCollection<string> PrioriteOptions { get; set; }
    public ObservableCollection<string> CategorieOptions { get; set; }

    public ICommand AddTaskCommand { get; }
    public ICommand CancelCommand { get; }

    private Action<Tache> OnTaskAdded;

    public AjoutTask(Action<Tache> onTaskAdded)
    {
        InitializeComponent();

        // Initialiser les options pour les Pickers
        StatutOptions = new ObservableCollection<string> { "� faire", "En cours", "Termin�" };
        PrioriteOptions = new ObservableCollection<string> { "Basse", "Moyenne", "Haute" };
        CategorieOptions = new ObservableCollection<string> { "D�veloppement", "Test", "Design" };

        OnTaskAdded = onTaskAdded;

        // Initialiser les commandes
        AddTaskCommand = new Command(async () => await OnAddTask());
        CancelCommand = new Command(async () => await OnCancel());

        BindingContext = this;
    }

    private async Task OnAddTask()
    {
        if (string.IsNullOrWhiteSpace(Titre) || string.IsNullOrWhiteSpace(Description) ||
            string.IsNullOrWhiteSpace(Statut) || string.IsNullOrWhiteSpace(Priorite) ||
            string.IsNullOrWhiteSpace(Categorie))
        {
            await DisplayAlert("Erreur", "Tous les champs doivent �tre remplis.", "OK");
            return;
        }

        // Cr�er une nouvelle t�che
        var nouvelleTache = new Tache(
            id: new Random().Next(1000, 9999),
            titre: Titre,
            description: Description,
            statut: Statut,
            priorite: Priorite,
            categorie: Categorie,
            etiquettes: "Urgent",
            null,
            date_creation: DateTime.Now
        );

        // Appeler le callback pour ajouter la t�che
        OnTaskAdded?.Invoke(nouvelleTache);

        // Afficher un message de succ�s
        await DisplayAlert("Succ�s", "La t�che a �t� ajout�e avec succ�s.", "OK");

        // Revenir � la page pr�c�dente
        await Navigation.PopAsync();
    }

    private async Task OnCancel()
    {
        // Annuler et revenir � la page pr�c�dente
        await Navigation.PopAsync();
    }
}
