using taskMaster.Model;

namespace App.View;

public partial class DetailTache : ContentPage
{
    public Tache Tache { get; set; }
    public DetailTache(Tache tache)
    {
        InitializeComponent();
        Tache = tache;
        BindingContext = tache; // Lier la tâche sélectionnée au contexte de la page
    }

    private void OnAddCommentButtonClicked(object sender, EventArgs e)
    {
        string newCommentText = NewCommentEditor.Text;

        if (string.IsNullOrWhiteSpace(newCommentText))
        {
            DisplayAlert("Erreur", "Le commentaire ne peut pas être vide.", "OK");
            return;
        }

        // Créer un nouveau commentaire
        var newComment = new Commentaire(
            id: new Random().Next(1000, 9999),
            texte: newCommentText,
            utilisateur: Tache.auteur ?? new User(0, "Anonyme", "", "", "", DateTime.Now),
            date: DateTime.Now
        );

        // Ajouter le commentaire à la tâche
        Tache.commentaires?.Add(newComment);

        // Rafraîchir la CollectionView
        TaskListView.ItemsSource = null;
        TaskListView.ItemsSource = Tache.commentaires;

        // Réinitialiser l'éditeur
        NewCommentEditor.Text = string.Empty;
    }

    private void OnDeleteCommentButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var commentToDelete = button?.CommandParameter as Commentaire;

        if (commentToDelete != null)
        {
            // Supprimer le commentaire
            Tache.commentaires?.Remove(commentToDelete);

            // Rafraîchir la CollectionView
            TaskListView.ItemsSource = null;
            TaskListView.ItemsSource = Tache.commentaires;
        }
    }
}