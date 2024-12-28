using EMGATA.API.Services;
using EMGATA.API.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EMGATA.API.Models.Enums;

namespace EMGATA.Tests.Services;

[TestClass]
public class CarServiceTests : TestBase
{
  private ICarService _carService;
  private ICarRepository _carRepository;
  private IModelRepository _modelRepository;

  [TestInitialize]
  public void Setup()
  {
    _carRepository = new CarRepository(_context);
    _modelRepository = new ModelRepository(_context);
    _carService = new CarService(_carRepository, _modelRepository);
    SeedTestData();
  }

  [TestCleanup]
  public void Cleanup()
  {
    ClearDatabase();
  }

  [TestMethod]
  public async Task GetAllCars_ShouldReturnAllCars()
  {
    // Act
    var result = await _carService.GetAllCarsAsync();

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual(2, result.Count());
  }

  [TestMethod]
  public async Task GetAvailableCars_ShouldReturnOnlyAvailableCars()
  {
    // Act
    var result = await _carService.GetAvailableCarsAsync();

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual(1, result.Count());
    Assert.IsTrue(result.All(c => c.Status == CarStatus.Available));
  }

  [TestMethod]
  public async Task UpdateCarStatus_ShouldChangeCarStatus()
  {
    // Arrange
    int carId = 1;
    var newStatus = CarStatus.Sold;

    // Act
    var result = await _carService.UpdateCarStatusAsync(carId, newStatus);

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual(newStatus, result.Status);
  }
}
