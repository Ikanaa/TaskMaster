using System.Collections.ObjectModel;
using System.Windows.Input;
using App.ViewModels;
using EntityFramework.Models;

namespace App.View;

public partial class AjoutTask : ContentPage
{


    public AjoutTask(Action<Tache> onTaskAdded, Projet projet)
    {
        InitializeComponent();

        // Passer le projet sélectionné au ViewModel
        BindingContext = new AjoutTaskViewModel(onTaskAdded, projet);
    }

}
