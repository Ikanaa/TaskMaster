using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using App.View;
using EntityFramework.Data;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace App.ViewModels;

public class AjoutTaskViewModel : INotifyPropertyChanged
{
    private string _titre;
    private string _description;
    private string _statut;
    private string _priorite;
    private string _categorie;
    private DateTime _dateEcheance = DateTime.Now;

    public string Titre
    {
        get => _titre;
        set
        {
            _titre = value;
            OnPropertyChanged();
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }

    public string Statut
    {
        get => _statut;
        set
        {
            _statut = value;
            OnPropertyChanged();
        }
    }

    public string Priorite
    {
        get => _priorite;
        set
        {
            _priorite = value;
            OnPropertyChanged();
        }
    }

    public string Categorie
    {
        get => _categorie;
        set
        {
            _categorie = value;
            OnPropertyChanged();
        }
    }

    private string _etiquettes;
    public string Etiquettes
    {
        get => _etiquettes;
        set
        {
            _etiquettes = value;
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

    public ObservableCollection<string> StatutOptions { get; set; }
    public ObservableCollection<string> PrioriteOptions { get; set; }
    public ObservableCollection<string> CategorieOptions { get; set; }

    public ObservableCollection<Utilisateur> UtilisateursAssocies { get; set; } = new ObservableCollection<Utilisateur>();

    public ICommand AddTaskCommand { get; }
    public ICommand CancelCommand { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    private Action<Tache> OnTaskAdded;

    public ObservableCollection<Projet> ListeProjets { get; set; }
    private Projet _projetSelectionne;

    public Projet ProjetSelectionne => _projetSelectionne;

    private Utilisateur _utilisateurAssigne;
    public Utilisateur UtilisateurAssigne
    {
        get => _utilisateurAssigne;
        set
        {
            _utilisateurAssigne = value;
            OnPropertyChanged();
        }
    }

    public AjoutTaskViewModel(Action<Tache> onTaskAdded, Projet projet)
    {
        // Initialiser les options pour les Pickers
        StatutOptions = new ObservableCollection<string> { "À faire", "En cours", "Terminé" };
        PrioriteOptions = new ObservableCollection<string> { "Basse", "Moyenne", "Haute" };
        CategorieOptions = new ObservableCollection<string> { "Développement", "Test", "Design" };

        // Stocker le projet passé en paramètre
        _projetSelectionne = projet;

        // Si aucun projet n'est spécifié, utiliser le premier disponible
        if (_projetSelectionne == null)
        {
            using (var context = new TaskmasterContext())
            {
                ListeProjets = new ObservableCollection<Projet>(context.Projets.ToList());
                _projetSelectionne = ListeProjets.FirstOrDefault();
            }
        }

        OnTaskAdded = onTaskAdded;
        ChargerUtilisateursAssocies();

        // Initialiser les commandes
        AddTaskCommand = new Command(async () => await AddTask());
        CancelCommand = new Command(async () => await Cancel());
    }



    private async Task AddTask()
    {
        if (string.IsNullOrWhiteSpace(Titre) || string.IsNullOrWhiteSpace(Description) ||
            string.IsNullOrWhiteSpace(Statut) || string.IsNullOrWhiteSpace(Priorite) ||
            string.IsNullOrWhiteSpace(Categorie) || string.IsNullOrWhiteSpace(Etiquettes) ||
            UtilisateurAssigne == null || ProjetSelectionne == null)
        {
            await App.Current.MainPage.DisplayAlert("Erreur", "Tous les champs doivent être remplis.", "OK");
            return;
        }

        using (var context = new TaskmasterContext())
        {
            // Créer une nouvelle tâche en utilisant uniquement les IDs
            var nouvelleTache = new Tache
            {
                Titre = Titre,
                Description = Description,
                Statut = Statut,
                Priorite = Priorite,
                Categorie = Categorie,
                Etiquettes = Etiquettes,
                Date_creation = DateTime.Now,
                Date_echeance = DateEcheance,
                Auteur_id = Session.UtilisateurConnecte.Id_uti,
                Assignee_id = UtilisateurAssigne.Id_uti,
                Projet_id = ProjetSelectionne.Id_pro // Associer directement au projet sélectionné
            };

            // Ajouter la tâche à la base de données
            context.Taches.Add(nouvelleTache);
            await context.SaveChangesAsync();

            // Charger la tâche complète avec ses relations pour l'événement
            var tacheComplete = await context.Taches
                .AsNoTracking()
                .Include(t => t.Auteur)
                .Include(t => t.Assignee)
                .Include(t => t.Projet)
                .FirstOrDefaultAsync(t => t.Id_tac == nouvelleTache.Id_tac);

            if (tacheComplete != null)
            {
                OnTaskAdded?.Invoke(tacheComplete);
            }
            else
            {
                OnTaskAdded?.Invoke(nouvelleTache);
            }
        }

        // Afficher un message de succès
        await App.Current.MainPage.DisplayAlert("Succès", "La tâche a été ajoutée avec succès.", "OK");

        // Revenir à la page précédente
        await App.Current.MainPage.Navigation.PopAsync();
    }


    private void ChargerUtilisateursAssocies()
    {
        UtilisateursAssocies.Clear();

        using (var context = new TaskmasterContext())
        {
            if (ProjetSelectionne != null)
            {
                // Charger les utilisateurs associés au projet sélectionné
                var utilisateurs = context.UtilisateurProjets
                    .Where(up => up.Projet_id == ProjetSelectionne.Id_pro)
                    .Select(up => up.Utilisateur)
                    .ToList();

                foreach (var utilisateur in utilisateurs)
                {
                    UtilisateursAssocies.Add(utilisateur);
                }
            }

            // Ajouter l'utilisateur connecté par défaut s'il n'est pas déjà dans la liste
            if (!UtilisateursAssocies.Any(u => u.Id_uti == Session.UtilisateurConnecte.Id_uti))
            {
                UtilisateursAssocies.Add(Session.UtilisateurConnecte);
            }
        }
    }

    private async Task Cancel()
    {
        // Annuler et revenir à la page précédente
        await App.Current.MainPage.Navigation.PopAsync();
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}