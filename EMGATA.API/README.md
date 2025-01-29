# EMG Voitures - Backend API

## Description

API REST développée avec .NET 8 pour la gestion de vente de voitures. Cette API gère l'authentification, les opérations CRUD sur les voitures, marques, et modèles, ainsi que la gestion des images.

## Technologies

- .NET 8
- Entity Framework Core
- SQLite
- JWT Authentication
- Swagger/OpenAPI

## Configuration Requise

- .NET 8 SDK
- Un éditeur de code (VS Code, Visual Studio, etc.)
- SQLite

## Installation

1. Cloner le repository (si pas déjà fait)

```bash
git clone https://github.com/atetheone/EMGATA.git
cd EMGATA/EMGATA.API
```

2. Restaurer les packages

```bash
dotnet restore
```

3. Appliquer les migrations

```bash
dotnet ef database update
```

4. Démarrer l'API

```bash
dotnet run
```

## Structure du Projet

```
EMGATA.API/
├── Controllers/          # Contrôleurs API
├── Models/              # Modèles de données
├── Services/            # Services métier
├── Repositories/        # Couche d'accès aux données
├── Data/                # Configuration EF Core
├── Migrations/          # Migrations de base de données
└── DTOs/                # Objets de transfert de données
```

## Endpoints API

### Authentication

- POST /api/auth/register
- POST /api/auth/login

### Voitures

- GET /api/cars
- GET /api/cars/{id}
- POST /api/cars
- PUT /api/cars/{id}
- DELETE /api/cars/{id}
- PATCH /api/cars/{id}/status

### Marques

- GET /api/brands
- GET /api/brands/{id}
- POST /api/brands
- PUT /api/brands/{id}
- DELETE /api/brands/{id}

### Modèles

- GET /api/models
- GET /api/models/{id}
- POST /api/models
- PUT /api/models/{id}
- DELETE /api/models/{id}

### Images

- POST /api/cars/{carId}/images
- DELETE /api/cars/{carId}/images/{imageId}
- PUT /api/cars/{carId}/images/{imageId}/set-main

## Configuration

Le fichier `appsettings.json` contient les configurations importantes:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=EMGATA.db"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32CharsLong",
    "Issuer": "EMGATA.API",
    "Audience": "EMGATA.Client"
  }
}
```

## Gestion des Erreurs

L'API utilise un middleware personnalisé pour gérer les erreurs de manière cohérente:

- 400 Bad Request - Erreurs de validation
- 401 Unauthorized - Non authentifié
- 403 Forbidden - Non autorisé
- 404 Not Found - Ressource non trouvée
- 500 Internal Server Error - Erreurs serveur

## Tests

Les tests unitaires et d'intégration sont disponibles dans le projet EMGATA.Tests.

```bash
dotnet test ../EMGATA.Tests
```

## Déploiement

L'API est configurée pour être déployée sur Azure App Service:

1. Créer une ressource App Service sur Azure
2. Configurer les variables d'environnement
3. Déployer via Azure DevOps ou GitHub Actions

## Documentation API

La documentation Swagger est disponible à l'adresse:

```
http://localhost:5291/swagger
```

## Aide au Développement

1. Activer le mode watch pour le développement:

```bash
dotnet watch run
```

2. Générer une nouvelle migration:

```bash
dotnet ef migrations add NomDeLaMigration
```
