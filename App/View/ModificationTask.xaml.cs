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

        // Passer la t�che et les utilisateurs associ�s au ViewModel
        ViewModel = new ModificationTaskViewModel(task, utilisateursAssocies);
        BindingContext = ViewModel;

        // Souscrire � un �v�nement pour �tre notifi� lorsque la t�che est modifi�e
        ViewModel.TaskModified += (sender, modifiedTask) =>
        {
            OnTaskModified?.Invoke(modifiedTask);
        };
    }
}
