# TEXTUEL

[tache, projet, utilisateur, commentaire]

- une tache est assigné a un projet
- une tache est créé par un utilisateur
- une tache est assigné a un utilisateur
- un utilisateur participe a un ou plusieur projet
- un commentaire est assigné a une tache
- un commentaire est créé par un utilisateur
- une tache peut être une sous tache est être enfant d'une tache
---
# MCD
```mermaid
erDiagram
    TACHE ||--o{ TACHE : "est parent de"
    TACHE }o--|| PROJET : "est assigné à"
    TACHE }o--|| UTILISATEUR : "est créé par"
    TACHE }o--|| UTILISATEUR : "est assigné à"
    UTILISATEUR }o--o{ PROJET : "participe à"
    COMMENTAIRE }o--|| TACHE : "est assigné à"
    COMMENTAIRE ||--o{ UTILISATEUR : "est creer par"
```
---
# MLD
```mermaid
erDiagram
    TACHE {
        int id_tac PK
        int parent_tache_id FK
        int projet_id FK
        int auteur_id FK
        int assignee_id FK
        string titre
        string description
        string statut
        string priorite
        string categorie
        string etiquettes
        datetime date_creation
    }
    
    PROJET {
        int id_pro PK
        string nom
        string description
        datetime date_creation
    }
    
    UTILISATEUR {
        int id_uti PK
        string nom
        string prenom
        string email
        string mot_de_passe
        datetime date_inscription
    }
    
    COMMENTAIRE {
        int id_com PK
        int tache_id FK
        int utilisateur_id FK
        string contenu
        datetime date_creation
    }
    
    UTILISATEUR_PROJET {
        int utilisateur_id PK,FK
        int projet_id PK,FK
        datetime date_ajout
    }
    TACHE ||--o{ TACHE : "est parent de"
    TACHE }o--|| PROJET : "est assigné à"
    TACHE }o--|| UTILISATEUR : "est créé par"
    TACHE }o--|| UTILISATEUR : "est assigné à"
    UTILISATEUR }o--o{ PROJET : "participe à"
    COMMENTAIRE }o--|| TACHE : "est assigné à"
    COMMENTAIRE }o--|| UTILISATEUR : "est créé par"
    UTILISATEUR_PROJET }|--|| UTILISATEUR : "appartient à"
    UTILISATEUR_PROJET }|--|| PROJET : "concerne"
```