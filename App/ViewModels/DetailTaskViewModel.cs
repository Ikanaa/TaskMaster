using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EntityFramework.Data;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace App.ViewModels;

public class DetailTaskViewModel : INotifyPropertyChanged
{
    private Tache _tache;


    public Tache Tache
    {
        get => _tache;
        set
        {
            _tache = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(NomPrenomCreateur));
            OnPropertyChanged(nameof(NomPrenomAssigne));
        }
    }
    public ObservableCollection<Tache> TachesDisponibles { get; set; } = new ObservableCollection<Tache>();

    // Tâche sélectionnée dans la liste déroulante
    private Tache _tacheSelectionnee;
    public Tache TacheSelectionnee
    {
        get => _tacheSelectionnee;
        set
        {
            _tacheSelectionnee = value;
            OnPropertyChanged();
        }
    }

    // Propriété pour afficher le nom et prénom du créateur
    public string NomPrenomCreateur => Tache?.Auteur != null
        ? $"{Tache.Auteur.Prenom} {Tache.Auteur.Nom}"
        : "Non spécifié";

    // Propriété pour afficher le nom et prénom de l'utilisateur assigné
    public string NomPrenomAssigne => Tache?.Assignee != null
        ? $"{Tache.Assignee.Prenom} {Tache.Assignee.Nom}"
        : "Non assigné";

    public ObservableCollection<Commentaire> Commentaires { get; set; }
    public ObservableCollection<Tache> SousTaches { get; set; } = new ObservableCollection<Tache>();
    public ICommand AddSubTaskCommand { get; }
    public ICommand RemoveSubTaskCommand { get; }

    public string NouveauCommentaire { get; set; }

    public ICommand AddCommentCommand { get; }
    public ICommand DeleteCommentCommand { get; }

    public event PropertyChangedEventHandler PropertyChanged;
    public ICommand LierSousTacheCommand { get; }
    public DetailTaskViewModel(Tache tache)
    {
        Tache = tache;
        using (var context = new TaskmasterContext())
        {
            var loadedTask = context.Taches
                .AsNoTracking()
                .Include(t => t.Auteur)
                .Include(t => t.Assignee)
                .Include(t => t.Commentaires)
                .ThenInclude(c => c.Utilisateur)
                .FirstOrDefault(t => t.Id_tac == tache.Id_tac);

            if (loadedTask != null)
            {
                Tache = loadedTask;
                Commentaires = new ObservableCollection<Commentaire>(loadedTask.Commentaires ?? new List<Commentaire>());
            }
            else
            {
                Tache = tache;
                Commentaires = new ObservableCollection<Commentaire>();
            }
        }

        LoadSousTaches();
        LierSousTacheCommand = new Command(async () => await LierSousTache());
        LoadTachesDisponibles();
        AddCommentCommand = new Command(async () => await AddComment());
        RemoveSubTaskCommand = new Command<Tache>(async (soustache) => await RemoveSubTask(soustache));
        DeleteCommentCommand = new Command<Commentaire>(async (comment) => await DeleteComment(comment));
    }

    private async Task AddComment()
    {
        if (string.IsNullOrWhiteSpace(NouveauCommentaire))
        {
            await App.Current.MainPage.DisplayAlert("Erreur", "Le commentaire ne peut pas être vide.", "OK");
            return;
        }

        try
        {
            var newComment = new Commentaire
            {
                Contenu = NouveauCommentaire,
                Date_creation = DateTime.Now,
                Utilisateur_id = Session.UtilisateurConnecte.Id_uti,
                Tache_id = Tache.Id_tac
            };

            using (var context = new TaskmasterContext())
            {
                // Ajouter le commentaire à la base de données (sans attacher d'entités)
                context.Commentaires.Add(newComment);
                await context.SaveChangesAsync();

                // Recharger le commentaire complet avec ses relations
                var loadedComment = await context.Commentaires
                    .AsNoTracking()
                    .Include(c => c.Utilisateur)
                    .FirstOrDefaultAsync(c => c.Id_com == newComment.Id_com);

                if (loadedComment != null)
                {
                    Commentaires.Add(loadedComment);
                }
                else
                {
                    Commentaires.Add(newComment);
                }
            }

            NouveauCommentaire = string.Empty;
            OnPropertyChanged(nameof(NouveauCommentaire));
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Erreur",
                $"Impossible d'ajouter le commentaire: {ex.Message}", "OK");

            System.Diagnostics.Debug.WriteLine($"Erreur lors de l'ajout du commentaire: {ex}");
        }
    }

    public async Task DeleteComment(Commentaire comment)
    {
        if (comment == null) return;

        try
        {
            using (var context = new TaskmasterContext())
            {
                // Rechercher le commentaire dans la base de données par son ID
                var commentToDelete = await context.Commentaires
                    .FirstOrDefaultAsync(c => c.Id_com == comment.Id_com);

                if (commentToDelete != null)
                {
                    // Supprimer le commentaire
                    context.Commentaires.Remove(commentToDelete);
                    await context.SaveChangesAsync();

                    // Supprimer de la collection observable
                    Commentaires.Remove(comment);

                    await App.Current.MainPage.DisplayAlert("Succès",
                        "Le commentaire a été supprimé.", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Erreur",
                        "Le commentaire n'a pas été trouvé dans la base de données.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            // Afficher un message d'erreur
            await App.Current.MainPage.DisplayAlert("Erreur",
                $"Impossible de supprimer le commentaire: {ex.Message}", "OK");

            System.Diagnostics.Debug.WriteLine($"Erreur lors de la suppression du commentaire: {ex}");
        }
    }

    public async Task RefreshComments()
    {
        try
        {
            using (var context = new TaskmasterContext())
            {
                var refreshedComments = await context.Commentaires
                    .Where(c => c.Tache_id == Tache.Id_tac)
                    .Include(c => c.Utilisateur)
                    .ToListAsync();

                Commentaires.Clear();
                foreach (var c in refreshedComments)
                {
                    Commentaires.Add(c);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erreur lors du rafraîchissement des commentaires: {ex}");
        }
    }




    private async void LoadTachesDisponibles()
    {
        try
        {
            using (var context = new TaskmasterContext())
            {

                var tachesDisponibles = await context.Taches
                    .AsNoTracking()
                    .Where(t => t.Projet_id == Tache.Projet_id
                            && t.Id_tac != Tache.Id_tac
                            && t.Parent_tache_id == null
                            && !context.Taches.Any(st => st.Parent_tache_id == Tache.Id_tac && st.Id_tac == t.Id_tac))
                    .ToListAsync();

                TachesDisponibles.Clear();
                foreach (var t in tachesDisponibles)
                {
                    TachesDisponibles.Add(t);
                }

                System.Diagnostics.Debug.WriteLine($"Tâches disponibles chargées : {TachesDisponibles.Count}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement des tâches disponibles : {ex.Message}");
        }
    }

    // Méthode pour lier une tâche existante comme sous-tâche
   
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }



    private async Task LierSousTache()
    {
        if (TacheSelectionnee == null)
        {
            await App.Current.MainPage.DisplayAlert("Erreur", "Veuillez sélectionner une tâche à lier.", "OK");
            return;
        }

        try
        {
            using (var context = new TaskmasterContext())
            {
                var tacheLier = await context.Taches
                    .FirstOrDefaultAsync(t => t.Id_tac == TacheSelectionnee.Id_tac);

                if (tacheLier != null)
                {
                    // Définir cette tâche comme parent
                    tacheLier.Parent_tache_id = Tache.Id_tac;
                    await context.SaveChangesAsync();

                    // Recharger la tâche pour l'ajouter à la liste des sous-tâches
                    var loadedTask = await context.Taches
                        .AsNoTracking()
                        .Include(t => t.Assignee)
                        .FirstOrDefaultAsync(t => t.Id_tac == tacheLier.Id_tac);

                    if (loadedTask != null)
                    {
                        SousTaches.Add(loadedTask);
                    }

                    // Rafraîchir la liste des tâches disponibles
                    TacheSelectionnee = null;
                    LoadTachesDisponibles();

                    await App.Current.MainPage.DisplayAlert("Succès", "La tâche a été liée comme sous-tâche.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Erreur",
                $"Impossible de lier la tâche: {ex.Message}", "OK");
            System.Diagnostics.Debug.WriteLine($"Erreur lors de la liaison de la sous-tâche: {ex}");
        }
    }
    private async void LoadSousTaches()
    {
        try
        {
            using (var context = new TaskmasterContext())
            {
                // Récupérer toutes les sous-tâches qui ont cette tâche comme parent
                var soustaches = await context.Taches
                    .AsNoTracking()
                    .Include(t => t.Assignee)
                    .Where(t => t.Parent_tache_id == Tache.Id_tac)
                    .ToListAsync();

                SousTaches.Clear();
                foreach (var soustache in soustaches)
                {
                    SousTaches.Add(soustache);
                }

                System.Diagnostics.Debug.WriteLine($"Sous-tâches chargées : {SousTaches.Count}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement des sous-tâches : {ex.Message}");
        }
    }

    public async Task RefreshSousTaches()
    {
        try
        {
            using (var context = new TaskmasterContext())
            {
                // Récupérer toutes les sous-tâches qui ont cette tâche comme parent
                var refreshedSousTaches = await context.Taches
                    .AsNoTracking()
                    .Include(t => t.Assignee)
                    .Where(t => t.Parent_tache_id == Tache.Id_tac)
                    .ToListAsync();

                SousTaches.Clear();
                foreach (var st in refreshedSousTaches)
                {
                    SousTaches.Add(st);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erreur lors du rafraîchissement des sous-tâches: {ex}");
        }
    }

    private async Task RemoveSubTask(Tache soustache)
    {
        if (soustache == null) return;

        // Demander confirmation
        bool confirm = await App.Current.MainPage.DisplayAlert(
            "Confirmation",
            "Voulez-vous détacher cette sous-tâche ? La tâche ne sera pas supprimée, mais elle ne sera plus liée à cette tâche parente.",
            "Oui", "Non");

        if (!confirm) return;

        try
        {
            using (var context = new TaskmasterContext())
            {
                // Récupérer la sous-tâche dans le contexte actuel
                var taskToUpdate = await context.Taches
                    .FirstOrDefaultAsync(t => t.Id_tac == soustache.Id_tac);

                if (taskToUpdate != null)
                {
                    // Détacher la sous-tâche en mettant son Parent_tache_id à null
                    taskToUpdate.Parent_tache_id = null;
                    await context.SaveChangesAsync();

                    // Supprimer de la collection observable
                    SousTaches.Remove(soustache);

                    // Rafraichir la liste des tâches disponibles car cette tâche peut maintenant être liée
                    LoadTachesDisponibles();

                    await App.Current.MainPage.DisplayAlert("Succès",
                        "La sous-tâche a été détachée avec succès.", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Erreur",
                        "La sous-tâche n'a pas été trouvée dans la base de données.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Erreur",
                $"Impossible de détacher la sous-tâche: {ex.Message}", "OK");
            System.Diagnostics.Debug.WriteLine($"Erreur lors du détachement de la sous-tâche: {ex}");
        }
    }
}