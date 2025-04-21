using App.ViewModels;
using EntityFramework.Models;
using System.Collections.ObjectModel;

namespace App.View;

public partial class ModificationTask : ContentPage
{
    public ModificationTaskViewModel ViewModel { get; set; }
    private Action<Tache> OnTaskModified;

    public ModificationTask(Tache task, ObservableCollection<Utilisateur> utilisateursAssocies, Action<Tache> onTaskModified)
    {
        InitializeComponent();
        OnTaskModified = onTaskModified;

        // Passer la tâche et les utilisateurs associés au ViewModel
        ViewModel = new ModificationTaskViewModel(task, utilisateursAssocies);
        BindingContext = ViewModel;

        // Souscrire à un événement pour être notifié lorsque la tâche est modifiée
        ViewModel.TaskModified += (sender, modifiedTask) =>
        {
            OnTaskModified?.Invoke(modifiedTask);
        };
    }
}
