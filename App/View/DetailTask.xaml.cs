using EntityFramework.Models;

namespace App.View;

public partial class DetailTache : ContentPage
{
    public Tache Tache { get; set; }
    public DetailTache(Tache tache)
    {
        InitializeComponent();
        Tache = tache;
        BindingContext = tache; // Lier la t�che s�lectionn�e au contexte de la page
    }

    private void OnAddCommentButtonClicked(object sender, EventArgs e)
    {
        string newCommentText = NewCommentEditor.Text;

        if (string.IsNullOrWhiteSpace(newCommentText))
        {
            DisplayAlert("Erreur", "Le commentaire ne peut pas �tre vide.", "OK");
            return;
        }

        var user = new Utilisateur();
        user.Nom = "Anonyme";
        user.Id_uti = 0;
        user.Date_inscription = DateTime.Now;
        user.Email = "";
        user.Mot_de_passe = "";
        user.Prenom = "";

        // Cr�er un nouveau commentaire
        var newComment = new Commentaire();

            newComment.Id_com = new Random().Next(1000, 9999);
            newComment.Contenu = newCommentText;
        newComment.Utilisateur = Tache.Auteur ?? user;
            newComment.Date_creation = DateTime.Now;

        

        // Ajouter le commentaire � la t�che
        Tache.Commentaires?.Add(newComment);

        // Rafra�chir la CollectionView
        TaskListView.ItemsSource = null;
        TaskListView.ItemsSource = Tache.Commentaires;

        // R�initialiser l'�diteur
        NewCommentEditor.Text = string.Empty;
    }

    private void OnDeleteCommentButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var commentToDelete = button?.CommandParameter as Commentaire;

        if (commentToDelete != null)
        {
            // Supprimer le commentaire
            Tache.Commentaires?.Remove(commentToDelete);

            // Rafra�chir la CollectionView
            TaskListView.ItemsSource = null;
            TaskListView.ItemsSource = Tache.Commentaires;
        }
    }
}