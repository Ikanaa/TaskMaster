using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EntityFramework.Data;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace App.ViewModels;

public class GestionUtilisateursViewModel : INotifyPropertyChanged
{
    private Projet _projet;

    private ObservableCollection<Utilisateur> _utilisateursAssocies;
    public ObservableCollection<Utilisateur> UtilisateursAssocies
    {
        get => _utilisateursAssocies;
        set
        {
            _utilisateursAssocies = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Utilisateur> _tousLesUtilisateurs;
    public ObservableCollection<Utilisateur> TousLesUtilisateurs
    {
        get => _tousLesUtilisateurs;
        set
        {
            _tousLesUtilisateurs = value;
            OnPropertyChanged();
        }
    }

    public ICommand AjouterUtilisateurCommand { get; }
    public ICommand RetirerUtilisateurCommand { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    public GestionUtilisateursViewModel()
    {
        UtilisateursAssocies = new ObservableCollection<Utilisateur>();
        TousLesUtilisateurs = new ObservableCollection<Utilisateur>();

        AjouterUtilisateurCommand = new Command(async () => await AjouterUtilisateur());
        RetirerUtilisateurCommand = new Command<Utilisateur>(async (utilisateur) => await RetirerUtilisateur(utilisateur));
    }

    public GestionUtilisateursViewModel(Projet projet) : this()
    {
        _projet = projet;

        ChargerUtilisateursAssocies();
        ChargerTousLesUtilisateurs();
    }

    private async void ChargerUtilisateursAssocies()
    {
        using (var context = new TaskmasterContext())
        {
            var utilisateurs = await context.UtilisateurProjets
                .Where(up => up.Projet_id == _projet.Id_pro)
                .Select(up => up.Utilisateur)
                .ToListAsync();

            UtilisateursAssocies = new ObservableCollection<Utilisateur>(utilisateurs);
        }
    }

    private async void ChargerTousLesUtilisateurs()
    {
        using (var context = new TaskmasterContext())
        {
            var utilisateurs = await context.Utilisateurs.ToListAsync();
            TousLesUtilisateurs = new ObservableCollection<Utilisateur>(utilisateurs);
        }
    }

    private async Task AjouterUtilisateur()
    {
        var utilisateur = await App.Current.MainPage.DisplayActionSheet(
            "Sélectionnez un utilisateur",
            "Annuler",
            null,
            TousLesUtilisateurs.Select(u => $"{u.Nom} {u.Prenom}").ToArray());

        if (string.IsNullOrWhiteSpace(utilisateur)) return;

        var utilisateurSelectionne = TousLesUtilisateurs.FirstOrDefault(u => $"{u.Nom} {u.Prenom}" == utilisateur);
        if (utilisateurSelectionne == null) return;

        using (var context = new TaskmasterContext())
        {
            // Vérifiez si la relation existe déjà
            var relationExistante = await context.UtilisateurProjets
                .AnyAsync(up => up.Utilisateur_id == utilisateurSelectionne.Id_uti && up.Projet_id == _projet.Id_pro);

            if (relationExistante)
            {
                // Affichez un message ou ignorez l'ajout
                await App.Current.MainPage.DisplayAlert("Info", "Cet utilisateur est déjà associé à ce projet.", "OK");
                return;
            }

            // Créez une nouvelle relation UtilisateurProjet
            var utilisateurProjet = new UtilisateurProjet
            {
                Utilisateur_id = utilisateurSelectionne.Id_uti,
                Projet_id = _projet.Id_pro,
                Date_ajout = DateTime.Now
            };

            // Ajoutez la relation sans attacher explicitement les entités
            context.UtilisateurProjets.Add(utilisateurProjet);
            await context.SaveChangesAsync();
        }

        ChargerUtilisateursAssocies(); // Recharge les utilisateurs associés
    }

    private async Task RetirerUtilisateur(Utilisateur utilisateur)
    {
        if (utilisateur == null) return;

        using (var context = new TaskmasterContext())
        {
            var utilisateurProjet = await context.UtilisateurProjets
                .FirstOrDefaultAsync(up => up.Utilisateur_id == utilisateur.Id_uti && up.Projet_id == _projet.Id_pro);

            if (utilisateurProjet != null)
            {
                context.UtilisateurProjets.Remove(utilisateurProjet);
                await context.SaveChangesAsync();
            }
        }

        ChargerUtilisateursAssocies(); // Recharge les utilisateurs associés
    }

    
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
