[tache, projet, utilisateur, commentaire]

une tache est assigné a un projet
une tache est créé par un utilisateur
une tache est assigné a un utilisateur
un utilisateur participe a un ou plusieur projet
un commentaire est assigné a une tache
une tache peut être une sous tache est être enfant d'une tache

```mermaid
erDiagram
    TACHE ||--o{ TACHE : "est parent de"
    TACHE }o--|| PROJET : "est assigné à"
    TACHE }o--|| UTILISATEUR : "est créé par"
    TACHE }o--|| UTILISATEUR : "est assigné à"
    UTILISATEUR }o--o{ PROJET : "participe à"
    COMMENTAIRE }o--|| TACHE : "est assigné à"
```