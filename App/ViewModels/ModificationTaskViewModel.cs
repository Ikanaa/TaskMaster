using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EntityFramework.Data;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

public class ModificationTaskViewModel : INotifyPropertyChanged
{
    private Tache _tache;
    private Utilisateur _utilisateurAssigne;
    private DateTime _dateEcheance;
    private Projet _projetSelectionne;

    public string Titre
    {
        get => _tache.Titre;
        set
        {
            _tache.Titre = value;
            OnPropertyChanged();
        }
    }

    public string Description
    {
        get => _tache.Description;
        set
        {
            _tache.Description = value;
            OnPropertyChanged();
        }
    }

    public string Statut
    {
        get => _tache.Statut;
        set
        {
            _tache.Statut = value;
            OnPropertyChanged();
        }
    }

    public string Priorite
    {
        get => _tache.Priorite;
        set
        {
            _tache.Priorite = value;
            OnPropertyChanged();
        }
    }

    public string Categorie
    {
        get => _tache.Categorie;
        set
        {
            _tache.Categorie = value;
            OnPropertyChanged();
        }
    }


    public Utilisateur UtilisateurAssigne
    {
        get => _utilisateurAssigne;
        set
        {
            _utilisateurAssigne = value;
            OnPropertyChanged();
        }
    }

    public DateTime DateEcheance
    {
        get => _dateEcheance;
        set
        {
            _dateEcheance = value;
            OnPropertyChanged();
        }
    }

    public Projet ProjetSelectionne
    {
        get => _projetSelectionne;
        set
        {
            _projetSelectionne = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Projet> ListeProjets { get; set; }
    public ObservableCollection<Utilisateur> UtilisateursAssocies { get; set; }

    public ObservableCollection<string> StatutOptions { get; set; }
    public ObservableCollection<string> PrioriteOptions { get; set; }
    public ObservableCollection<string> CategorieOptions { get; set; }

    public ICommand SaveTaskCommand { get; }
    public ICommand CancelCommand { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    // Ajout de l'événement TaskModified
    public event EventHandler<Tache> TaskModified;

    public ModificationTaskViewModel(Tache tache, ObservableCollection<Utilisateur> utilisateursAssocies)
    {
        using (var context = new TaskmasterContext())
        {
            _tache = context.Taches
                .AsNoTracking()
                .Include(t => t.Assignee) // Inclure l'utilisateur assigné
                .Include(t => t.Projet)   // Inclure le projet
                .FirstOrDefault(t => t.Id_tac == tache.Id_tac);
        }

        StatutOptions = new ObservableCollection<string> { "À faire", "En cours", "Terminé" };
        PrioriteOptions = new ObservableCollection<string> { "Basse", "Moyenne", "Haute" };
        CategorieOptions = new ObservableCollection<string> { "Développement", "Test", "Design" };

        UtilisateursAssocies = utilisateursAssocies;
        UtilisateurAssigne = _tache?.Assignee; // Initialiser l'utilisateur assigné

        System.Diagnostics.Debug.WriteLine($"Utilisateur assigné au chargement : {UtilisateurAssigne?.Prenom} {UtilisateurAssigne?.Nom}");

        DateEcheance = _tache?.Date_echeance != default ? _tache.Date_echeance : DateTime.Now;

        using (var context = new TaskmasterContext())
        {
            ListeProjets = new ObservableCollection<Projet>(context.Projets.ToList());
        }

        ProjetSelectionne = _tache?.Projet;
        System.Diagnostics.Debug.WriteLine($"Projet sélectionné au chargement : {ProjetSelectionne?.Nom}");

        SaveTaskCommand = new Command(async () => await SaveTask());
        CancelCommand = new Command(async () => await Cancel());
    }

    private async Task SaveTask()
    {
        try
        {
            using (var context = new TaskmasterContext())
            {
                var existingTask = await context.Taches
                    .FirstOrDefaultAsync(t => t.Id_tac == _tache.Id_tac);

                if (existingTask != null)
                {
                    // Mettre à jour les propriétés simples
                    existingTask.Titre = Titre;
                    existingTask.Description = Description;
                    existingTask.Statut = Statut;
                    existingTask.Priorite = Priorite;
                    existingTask.Categorie = Categorie;
                    existingTask.Date_echeance = DateEcheance;

                    // Mettre à jour les IDs relationnels
                    existingTask.Assignee_id = UtilisateurAssigne?.Id_uti;
                    existingTask.Projet_id = ProjetSelectionne?.Id_pro ?? existingTask.Projet_id;

                    // Ajouter des logs de débogage
                    System.Diagnostics.Debug.WriteLine($"Mise à jour de la tâche ID: {existingTask.Id_tac}");
                    System.Diagnostics.Debug.WriteLine($"Titre: {existingTask.Titre}");
                    System.Diagnostics.Debug.WriteLine($"Assignee ID: {existingTask.Assignee_id}");
                    System.Diagnostics.Debug.WriteLine($"Projet ID: {existingTask.Projet_id}");

                    await context.SaveChangesAsync();

                    // Recharger la tâche complète avec toutes ses relations pour l'événement
                    var updatedTask = await context.Taches
                        .AsNoTracking()
                        .Include(t => t.Assignee)
                        .Include(t => t.Auteur)
                        .Include(t => t.Projet)
                        .FirstOrDefaultAsync(t => t.Id_tac == _tache.Id_tac);

                    // Déclencher l'événement TaskModified
                    TaskModified?.Invoke(this, updatedTask);

                    // Vérifier que les modifications ont été sauvegardées
                    System.Diagnostics.Debug.WriteLine($"Après sauvegarde - Assignee ID: {updatedTask?.Assignee_id}");
                    System.Diagnostics.Debug.WriteLine($"Après sauvegarde - Utilisateur assigné: {updatedTask?.Assignee?.Prenom} {updatedTask?.Assignee?.Nom}");
                    System.Diagnostics.Debug.WriteLine($"Après sauvegarde - Projet ID: {updatedTask?.Projet_id}");
                    System.Diagnostics.Debug.WriteLine($"Après sauvegarde - Projet: {updatedTask?.Projet?.Nom}");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", "La tâche n'existe pas dans la base de données.", "OK");
                    return;
                }
            }

            await Application.Current.MainPage.DisplayAlert("Succès", "La tâche a été modifiée avec succès.", "OK");
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erreur lors de la sauvegarde de la tâche: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");

            await Application.Current.MainPage.DisplayAlert("Erreur",
                $"Une erreur s'est produite lors de la sauvegarde de la tâche: {ex.Message}", "OK");
        }
    }

    private async Task Cancel()
    {
        await Application.Current.MainPage.Navigation.PopAsync();
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
