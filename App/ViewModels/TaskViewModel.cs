using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EntityFramework.Data;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace App.ViewModels;

public class TaskViewModel : INotifyPropertyChanged
{
    private Tache _selectedTask;

    public ObservableCollection<Tache> Tasks { get; set; } = new ObservableCollection<Tache>();

    public Tache SelectedTask
    {
        get => _selectedTask;
        set
        {
            _selectedTask = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddTaskCommand { get; }
    public ICommand DeleteTaskCommand { get; }
    public ICommand ModifyTaskCommand { get; }
    public ICommand LoadTasksCommand { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    public TaskViewModel()
    {
       
        DeleteTaskCommand = new Command<Tache>(async (task) => await DeleteTask(task));
        ModifyTaskCommand = new Command<Tache>(async (task) => await ModifyTask(task));
        LoadTasksCommand = new Command(async () => await LoadTasks());

        // Charger les tâches au démarrage
        LoadTasksCommand.Execute(null);
    }

    private async Task LoadTasks()
    {
        using (var context = new TaskmasterContext())
        {
            var tasks = await context.Taches.ToListAsync();
            Tasks.Clear();
            foreach (var task in tasks)
            {
                Tasks.Add(task);
            }
        }
    }

   

    private async Task DeleteTask(Tache task)
    {
        if (task == null) return;

        using (var context = new TaskmasterContext())
        {
            context.Taches.Remove(task);
            await context.SaveChangesAsync();
        }

        Tasks.Remove(task);
    }

    

    private async Task ModifyTask(Tache task)
    {

        if (task == null || App.Current?.MainPage == null) return;

        ObservableCollection<Utilisateur> utilisateursAssocies;
        using (var context = new TaskmasterContext())
        {
            var utilisateurs = await context.UtilisateurProjets
                .Where(up => up.Projet_id == task.Projet_id)
                .Select(up => up.Utilisateur)
                .ToListAsync();

            utilisateursAssocies = new ObservableCollection<Utilisateur>(utilisateurs);
        }

        // Naviguer vers la page de modification
        await App.Current.MainPage.Navigation.PushAsync(new View.ModificationTask(task, utilisateursAssocies, OnTaskModified));

    }

    // Callback pour mettre à jour la tâche après modification
    private void OnTaskModified(Tache modifiedTask)
    {
        if (modifiedTask != null)
        {
            // Rechercher la tâche existante et la mettre à jour
            var existingTask = Tasks.FirstOrDefault(t => t.Id_tac == modifiedTask.Id_tac);
            if (existingTask != null)
            {
                var index = Tasks.IndexOf(existingTask);
                Tasks[index] = modifiedTask;
            }
        }


    }
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}