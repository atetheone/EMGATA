```markdown
# EMG Voitures - Frontend

## Description
Application web de gestion de vente de voitures développée avec Angular 18+. Cette application permet aux utilisateurs de parcourir les véhicules disponibles et aux administrateurs de gérer l'inventaire des voitures.

## Technologies Utilisées
- Angular 18+
- TailwindCSS avec DaisyUI
- SSR (Server Side Rendering)
- JWT pour l'authentification

## Fonctionnalités
### Interface Publique
- Visualisation des voitures disponibles
- Galerie d'images pour chaque voiture
- Page de détails des véhicules
- Page de contact
- Page "À propos"

### Interface Administrateur
- Tableau de bord administrateur
- Gestion des voitures (ajout, modification, suppression)
- Gestion des marques et modèles
- Gestion des images des véhicules

## Prérequis
- Node.js (version 20.x ou supérieure)
- npm (dernière version stable)

## Installation

1. Cloner le repository
```bash
git clone https://github.com/atetheone/EMGATA.git
cd EMGATA/emgata_client
```

2. Installer les dépendances

```bash
npm install
```

3. Créer un fichier environment.development.ts

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'
};
```

## Commandes Disponibles

```bash
# Démarrer le serveur de développement
npm run start

# Démarrer le serveur de développement avec SSR
npm run dev:ssr

# Construire l'application
npm run build

# Construire l'application avec SSR
npm run build:ssr

# Démarrer le serveur SSR en production
npm run serve:ssr

# Lancer les tests
npm run test
```

## Structure du Projet

```
src/
├── app/
│   ├── core/            # Services, guards, interceptors
│   ├── features/        # Composants des fonctionnalités
│   ├── layouts/         # Layouts public et admin
│   └── shared/          # Composants partagés
├── assets/             # Images, fonts, etc.
└── environments/       # Configuration des environnements
```

## Routes Principales

- `/` - Page d'accueil
- `/cars` - Liste des voitures
- `/cars/:id` - Détails d'une voiture
- `/about` - À propos
- `/contact` - Contact
- `/admin/*` - Interface d'administration (accès protégé)

## Connexion Admin

```
Email: admin@emgata.com
Password: Admin@123
```
