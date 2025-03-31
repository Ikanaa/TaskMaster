# TaskMaster - Backlog de Produit Agile (Scrum)

## Vision du Produit
Créer une application multiplateforme intuitive de gestion de tâches permettant aux utilisateurs de gérer efficacement leurs tâches personnelles et professionnelles, de collaborer avec d'autres utilisateurs et de suivre leur progression.

## Épiques et User Stories

### Épique 1: Authentification et Gestion des Utilisateurs
- **US1.1**: En tant qu'utilisateur, je veux pouvoir créer un compte afin d'accéder à l'application.
    - Tâche 1.1.1: Concevoir l'interface d'inscription
    - Tâche 1.1.2: Implémenter la validation des données utilisateur dans l'API
    - Tâche 1.1.3: Stocker les informations utilisateur dans la base de données MariaDB

- **US1.2**: En tant qu'utilisateur, je veux pouvoir m'authentifier afin d'accéder à mes tâches.
    - Tâche 1.2.1: Concevoir l'interface de connexion
    - Tâche 1.2.2: Implémenter le système d'authentification dans l'API
    - Tâche 1.2.3: Gérer les sessions utilisateur, token API

### Épique 2: Gestion des Tâches de Base
- **US2.1**: En tant qu'utilisateur, je veux pouvoir créer une nouvelle tâche afin d'organiser mon travail.
    - Tâche 2.1.1: Concevoir le formulaire de création de tâche
    - Tâche 2.1.2: Implémenter la sauvegarde des tâches en base de données / API

- **US2.2**: En tant qu'utilisateur, je veux pouvoir modifier mes tâches afin de mettre à jour leurs informations.
    - Tâche 2.2.1: Concevoir l'interface de modification
    - Tâche 2.2.2: Implémenter la mise à jour en base de données / API

- **US2.3**: En tant qu'utilisateur, je veux pouvoir supprimer mes tâches afin de nettoyer ma liste.
    - Tâche 2.3.1: Ajouter la fonctionnalité de suppression
    - Tâche 2.3.2: Implémenter une confirmation avant suppression / API

### Épique 3: Fonctionnalités Avancées de Tâches
- **US3.1**: En tant qu'utilisateur, je veux pouvoir ajouter des sous-tâches afin de décomposer des tâches complexes.
    - Tâche 3.1.1: Concevoir l'interface de gestion des sous-tâches
    - Tâche 3.1.2: Implémenter la relation entre tâches et sous-tâches dans la base de donnée 

- **US3.2**: En tant qu'utilisateur, je veux pouvoir ajouter des étiquettes à mes tâches afin de mieux les catégoriser.
    - Tâche 3.2.1: Concevoir le système/interface d'étiquettes
    - Tâche 3.3.2: Ajouter fonctionnalité d'ajout/suppression d'étiquettes dans l'API
    - Tâche 3.2.2: Implémenter la relation many-to-many entre tâches et étiquettes

- **US3.3**: En tant qu'utilisateur, je veux pouvoir ajouter des commentaires aux tâches afin de fournir des informations supplémentaires.
    - Tâche 3.3.1: Concevoir l'interface de commentaires
    - Tâche 3.3.2: Ajouter fonctionnalité d'ajout/suppression de commentaire dans l'API
    - Tâche 3.3.2: Implémenter la relation entre tâches et commentaires

### Épique 4: Suivi et Visualisation
- **US4.1**: En tant qu'utilisateur, je veux pouvoir changer le statut d'une tâche afin de suivre son évolution.
    - Tâche 4.1.1: Implémenter les différents statuts (à faire, en cours, terminée, annulée) API / Base de donnée
    - Tâche 4.1.2: Concevoir une interface intuitive pour changer le statut (couleur de statut)

- **US4.2**: En tant qu'utilisateur, je veux consulter l'historique des modifications d'une tâche afin de suivre son évolution.
    - Tâche 4.2.1: Mettre en place un système de journalisation des modifications, base de donnée pur
    - Tâche 4.2.2: Concevoir l'affichage de l'historique

- **US4.3**: En tant qu'utilisateur, je veux filtrer et trier mes tâches selon différents critères afin de les visualiser efficacement.
    - Tâche 4.3.1: Implémenter le filtrage par priorité API
    - Tâche 4.3.2: Implémenter le filtrage par échéance API
    - Tâche 4.3.3: Implémenter le filtrage par catégorie API
    - Tâche 4.3.3: Interface de filtrage des requêtes

### Épique 5: Collaboration et Projets
- **US5.1**: En tant qu'utilisateur, je veux pouvoir associer une tâche à un projet afin de mieux organiser mon travail.
    - Tâche 5.1.1: Concevoir le système de projets interface
    - Tâche 5.1.2: Implémenter la relation entre tâches et projets BDD
    - Tâche 5.1.3: Implementer requêtes pour avoir projet tâche API

- **US5.2**: En tant qu'utilisateur, je veux pouvoir assigner une tâche à un autre utilisateur afin de déléguer le travail.
    - Tâche 5.2.1: Implémenter le système d'assignation utilisateur projet BDD / API
    - Tâche 5.2.2: Interface de recherche parmis tous les utilisateurs pour assigner au projet
    - Tâche 5.2.3: Implementer requête pour assigner un utilisateur a une tâche API
    - Tâche 5.2.4: Interface dropdown pour selectionner utilisateur pour une tache parmis les utilisateur du projet 

### Épique 6: Interface Utilisateur et Expérience
- **US6.1**: En tant qu'utilisateur, je veux une interface responsive qui fonctionne sur différents appareils.
    - Tâche 6.1.1: Adapter l'interface pour les téléphones mobiles OPTIONNEL
    - Tâche 6.1.2: Adapter l'interface pour les tablettes OPTIONNEL

- **US6.2**: En tant qu'utilisateur, je veux une vue détaillée de chaque tâche afin d'accéder à toutes ses informations.
    - Tâche 6.2.1: Concevoir la vue détaillée
    - Tâche 6.2.2: Implémenter la navigation entre vues
    - Tâche 6.2.3: Implémenter l'interface de filtrage des tâches, par projet, par utilisateur et par etiquettes...  
