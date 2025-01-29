# EMG Voitures - Projet Complet

## Description
Système complet de gestion de vente de voitures comprenant une API backend en .NET 8, une interface utilisateur en Angular 18+, et une suite de tests.

## Structure du Projet
```

emgata/
├── EMGATA.API/           # Backend API (.NET 8)
├── emgata_client/        # Frontend (Angular 18+)
└── EMGATA.Tests/         # Tests d'intégration et unitaires

```

## Composants du Projet

### 1. Backend (EMGATA.API)
- API REST développée avec .NET 8
- Base de données SQLite
- Authentication JWT
- Architecture en couches (Repository Pattern)
- Documentation Swagger

### 2. Frontend (emgata_client)
- Application Angular 18+
- TailwindCSS avec DaisyUI
- Server-Side Rendering (SSR)
- Interface publique et administrative
- Gestion des images

### 3. Tests (EMGATA.Tests)
- Tests unitaires
- Tests d'intégration
- Mocks et fixtures

## Prérequis
- .NET 8 SDK
- Node.js (v20+)
- npm (dernière version stable)
- Git

## Installation

1. Cloner le repository
```bash
git clone https://github.com/atetheone/EMGATA.git
cd EMGATA
```

2. Backend Setup

```bash
cd EMGATA.API
dotnet restore
dotnet ef database update
dotnet run
```

3. Frontend Setup

```bash
cd ../emgata_client
npm install
npm run start
```

4. Tests Setup

```bash
cd ../EMGATA.Tests
dotnet restore
dotnet test
```

## Ports et URLs

- Backend API: `http://localhost:5000`
- Frontend: `http://localhost:4200`
- Swagger Documentation: `http://localhost:5000/swagger`

## Credentials de Test

```
Admin:
Email: admin@emgata.com
Password: Admin@123
```

## CI/CD

Le projet utilise Azure DevOps pour l'intégration et le déploiement continus:

- Pipeline de build pour le backend et le frontend
- Tests automatisés
- Déploiement automatique vers Azure

## Développement

### Backend

```bash
cd EMGATA.API
dotnet watch run
```

### Frontend

```bash
cd emgata_client
npm run dev:ssr
```

### Tests

```bash
cd EMGATA.Tests
dotnet watch test
```

## Documentation

Chaque composant du projet dispose de sa propre documentation détaillée:

- [Documentation Backend](./EMGATA.API/README.md)
- [Documentation Frontend](./emgata_client/README.md)
- [Documentation des Tests](./EMGATA.Tests/README.md)

## Architecture

```
Backend (EMGATA.API)
├── Controllers/    # Contrôleurs API
├── Models/         # Modèles de données
├── Services/       # Logique métier
├── Repositories/   # Accès aux données
└── Migrations/     # Migrations DB

Frontend (emgata_client)
├── src/app/
│   ├── core/      # Services et guards
│   ├── features/  # Composants
│   └── shared/    # Éléments partagés
```

## Scripts Utiles

```bash
# Démarrer tout le projet
./run-all.sh

# Lancer les tests
./run-tests.sh

# Build complet
./build-all.sh
```

## Contribution

1. Fork du projet
2. Création d'une branche

```bash
git checkout -b feature/nom-fonctionnalite
```

3. Commit des changements

```bash
git commit -m "feat: description"
```

4. Push sur la branche

```bash
git push origin feature/nom-fonctionnalite
```

5. Création d'une Pull Request

## Versioning

Ce projet suit la convention [SemVer](http://semver.org/).

## Support

Pour toute question ou problème:

1. Consulter les issues GitHub existantes
2. Créer une nouvelle issue si nécessaire
3. Contacter l'équipe de développement

## Licence

Ce projet est sous licence MIT.

## Auteurs

- [Votre Nom] - Développement initial

## Remerciements

- M. E. H. Ousmane Diallo - Superviseur du projet
- École Supérieure Polytechnique (ESP)

