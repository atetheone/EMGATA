# EMG Voitures - Tests

## Description
Suite de tests pour l'API EMG Voitures utilisant MSTest Framework et une base de données en mémoire (InMemoryDatabase) pour les tests. Cette approche permet des tests rapides et isolés sans dépendance à une base de données physique.

## Structure du Projet
```

EMGATA.Tests/
├── Common/             # Classes de base et utilitaires pour les tests
│   └── BaseTests.cs   # Classe abstraite avec configuration InMemoryDb
├── Controllers/        # Tests des contrôleurs
├── Services/          # Tests des services
├── Repositories/      # Tests des repositories
├── Integration/       # Tests d'intégration
└── TestResults/      # Résultats des tests

```

## Technologies Utilisées
- MSTest Framework
- Entity Framework Core InMemory Database
- .NET 8

## Configuration de la Base de Données En Mémoire
La classe BaseTests configure la base de données en mémoire :

```csharp
public abstract class BaseTests
{
    protected ApplicationDbContext Context { get; private set; }
    
    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            
        Context = new ApplicationDbContext(options);
    }
}
```

## Avantages de InMemoryDatabase

1. Tests rapides (pas d'I/O disque)
2. Isolation parfaite entre les tests
3. Pas besoin de nettoyer la base de données
4. Pas de configuration de base de données externe

## Exécution des Tests

### Tous les tests

```bash
dotnet test
```

### Tests spécifiques

```bash
dotnet test --filter "ClassName=EMGATA.Tests.Integration.IntegrationTests"
```

## Exemple de Test

```csharp
[TestClass]
public class CarControllerTests : BaseTests
{
    private CarController _controller;
    private ICarService _carService;

    [TestInitialize]
    public void Initialize()
    {
        _carService = new CarService(Context);
        _controller = new CarController(_carService);
    }

    [TestMethod]
    public async Task GetCars_ReturnsAllCars()
    {
        // Arrange
        var cars = new List<Car>
        {
            new Car { /* properties */ },
            new Car { /* properties */ }
        };
        await Context.Cars.AddRangeAsync(cars);
        await Context.SaveChangesAsync();

        // Act
        var result = await _controller.GetCars();

        // Assert
        Assert.IsNotNull(result);
        // Plus d'assertions...
    }
}
```

## Best Practices

1. Utiliser un nouveau nom de base de données pour chaque test
2. Ne pas partager le contexte entre les tests
3. Initialiser les données nécessaires dans chaque test
4. Nettoyer les ressources après utilisation

## Développement

### Créer un nouveau test

```csharp
[TestMethod]
public async Task MethodName_Scenario_ExpectedResult()
{
    // Arrange - Préparer les données dans InMemoryDb
    var entity = new Entity { /* properties */ };
    await Context.Entities.AddAsync(entity);
    await Context.SaveChangesAsync();
    
    // Act
    
    // Assert
}
```

## Limitations de InMemoryDatabase

1. Ne supporte pas les transactions SQL réelles
2. Pas de support pour les migrations
3. Certaines fonctionnalités SQL spécifiques ne sont pas disponibles

## Exécution

- Via Visual Studio Test Explorer
- Via ligne de commande (dotnet test)
- Via l'intégration continue
