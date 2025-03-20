# Projet de Jeu Lexus - GOTY 2025

## Comment Jouer

1. **Contrôles**
   - ZQSD / Flèches : Déplacement
   - Souris : Viser
   - Clic Gauche : Tirer
   - R : Recharger
   - B : Ouvrir la boutique d'armes
   - Échap : Pause

2. **Objectifs**
   - Survivre aux vagues d'ennemis
   - Gagner de l'argent en tuant des ennemis
   - Acheter de meilleures armes
   - Battre votre meilleur score

3. **Armes Disponibles**
   - Pistolet (Gratuit) : Équilibré, bon pour débuter
   - Fusil d'Assaut (600€) : Tir rapide, dégâts moyens
   - Sniper (1200€) : Dégâts élevés, tir lent

4. **Conseils**
   - Commencez par les ennemis les plus proches
   - Économisez pour le Fusil d'Assaut
   - Utilisez le décor comme couverture
   - Rechargez quand vous êtes en sécurité

## Structure du Projet

```
Assets/
├── _Scripts/          # Scripts du jeu
├── _Scenes/          # Scènes du jeu
├── _Prefabs/         # Éléments préfabriqués
├── _Materials/       # Matériaux et textures
├── _Animations/      # Animations des personnages
├── _Audio/          # Musique et effets sonores
│   ├── Music/
│   └── SFX/
└── _Art/
    ├── Models/
    ├── Textures/
    └── UI/
```

## Fonctionnalités Principales

1. **Boucle de Jeu**
   - Menu Principal
   - Gameplay Principal
   - Game Over/Redémarrage

2. **Systèmes du Joueur**
   - Système de Santé
   - Armes Multiples
   - Suivi du Score

3. **Système d'Ennemis**
   - Gestion des Apparitions
   - Comportements IA
   - Différents Types d'Ennemis

4. **Système Économique**
   - Monnaie/Points
   - Améliorations/Boutique

## Directives de Développement

1. **Organisation du Code**
   - Utiliser des noms significatifs pour les scripts et variables
   - Commenter la logique complexe
   - Suivre les conventions de codage C#

2. **Gestion des Scènes**
   - Garder les scènes organisées
   - Utiliser des prefabs pour les éléments réutilisables
   - Implémenter des transitions de scènes appropriées

3. **Directives pour les Assets**
   - Maintenir un style artistique cohérent
   - Optimiser les assets pour la performance
   - Utiliser les formats de fichiers appropriés

4. **Contrôle de Version**
   - Faire des messages de commit significatifs
   - Garder les gros fichiers binaires dans LFS
   - Commits et pushes réguliers

## Flux de Travail d'Équipe

1. **Gestion des Tâches**
   - Utiliser les fonctionnalités de collaboration Unity
   - Réunions d'équipe régulières
   - Attribution claire des tâches

2. **Contrôle Qualité**
   - Tests réguliers
   - Suivi des bugs
   - Surveillance des performances

## Installation

1. Ouvrir le projet avec Unity 2022.3 ou supérieur
2. Ouvrir la scène "MainMenu" dans Assets/_Scenes
3. Appuyer sur Play

## Crédits

- Assets 3D : Sci-Fi Styled Modular Pack
- Armes : Modern Guns Pack
- UI : Clean Vector Icons

## Version

1.0.0 - Mars 2025
