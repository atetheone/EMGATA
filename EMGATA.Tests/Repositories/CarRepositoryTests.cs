using Microsoft.EntityFrameworkCore;
using EMGATA.API.Models;
using EMGATA.API.Models.Enums;
using EMGATA.API.Repositories;

namespace EMGATA.Tests.Repositories;

[TestClass]
public class CarRepositoryTests : TestBase
{
  private ICarRepository _carRepository;

  public CarRepositoryTests() : base()
  {
    _carRepository = new CarRepository(_context);
  }

  [TestInitialize]
  public void Setup()
  {
    SeedTestData();
  }

  [TestCleanup]
  public void Cleanup()
  {
    ClearDatabase();
  }

  [TestMethod]
  public async Task GetAllAsync_ShouldReturnAllCars()
  {
    // Act
    var cars = await _carRepository.GetAllAsync();

    // Assert
    Assert.IsNotNull(cars);
    Assert.AreEqual(2, cars.Count()); // Based on seeded data
  }

  [TestMethod]
  public async Task GetCarWithDetailsAsync_ShouldReturnCarWithRelations()
  {
    // Act
    var car = await _carRepository.GetCarWithDetailsAsync(1);

    // Assert
    Assert.IsNotNull(car);
    Assert.IsNotNull(car.Model, "Model should be loaded");
    Assert.IsNotNull(car.Model.Brand, "Brand should be loaded");
    Assert.IsNotNull(car.User, "User should be loaded");
  }

  [TestMethod]
  public async Task GetAvailableCarsAsync_ShouldReturnOnlyAvailableCars()
  {
    // Act
    var availableCars = await _carRepository.GetAvailableCarsAsync();

    // Assert
    Assert.IsNotNull(availableCars);
    Assert.IsTrue(availableCars.All(c => c.Status == CarStatus.Available));
    Assert.AreEqual(1, availableCars.Count()); // Based on seeded data
  }

  [TestMethod]
  public async Task GetCarsByBrandAsync_ShouldReturnCarsForSpecificBrand()
  {
    // Act
    var cars = await _carRepository.GetCarsByBrandAsync(1); // Toyota cars

    // Assert
    Assert.IsNotNull(cars);
    Assert.IsTrue(cars.All(c => c.Model.BrandId == 1));
  }

  [TestMethod]
  public async Task GetCarsByModelAsync_ShouldReturnCarsForSpecificModel()
  {
    // Act
    var cars = await _carRepository.GetCarsByModelAsync(1); // Camry cars

    // Assert
    Assert.IsNotNull(cars);
    Assert.IsTrue(cars.All(c => c.ModelId == 1));
  }

  [TestMethod]
  public async Task GetCarsByUserAsync_ShouldReturnCarsForSpecificUser()
  {
    // Act
    var cars = await _carRepository.GetCarsByUserAsync(1);

    // Assert
    Assert.IsNotNull(cars);
    Assert.IsTrue(cars.All(c => c.UserId == 1));
  }

  [TestMethod]
  public async Task AddAsync_ShouldCreateNewCar()
  {
    // Arrange
    var newCar = new Car
    {
      ModelId = 1,
      UserId = 1,
      Year = 2022,
      Color = "Silver",
      Price = 35000,
      Status = CarStatus.Available,
      Description = "New test car"
    };

    // Act
    var result = await _carRepository.AddAsync(newCar);
    var savedCar = await _carRepository.GetByIdAsync(result.Id);

    // Assert
    Assert.IsNotNull(savedCar);
    Assert.AreEqual(newCar.ModelId, savedCar.ModelId);
    Assert.AreEqual(newCar.Color, savedCar.Color);
    Assert.AreEqual(newCar.Price, savedCar.Price);
  }

  [TestMethod]
  public async Task UpdateAsync_ShouldModifyExistingCar()
  {
    // Arrange
    var car = await _carRepository.GetByIdAsync(1);
    var originalColor = car.Color;
    var newColor = "Black";

    // Act
    car.Color = newColor;
    await _carRepository.UpdateAsync(car);
    var updatedCar = await _carRepository.GetByIdAsync(1);

    // Assert
    Assert.AreNotEqual(originalColor, updatedCar.Color);
    Assert.AreEqual(newColor, updatedCar.Color);
  }

  [TestMethod]
  public async Task DeleteAsync_ShouldRemoveCar()
  {
    // Arrange
    var carId = 1;

    // Act
    await _carRepository.DeleteAsync(carId);
    var deletedCar = await _carRepository.GetByIdAsync(carId);

    // Assert
    Assert.IsNull(deletedCar);
  }

  [TestMethod]
  public async Task GetCarWithDetailsAsync_NonexistentId_ShouldReturnNull()
  {
    // Act
    var car = await _carRepository.GetCarWithDetailsAsync(999);

    // Assert
    Assert.IsNull(car);
  }

  [TestMethod]
  public async Task GetAvailableCarsAsync_ShouldIncludeMainImage()
  {
    // Act
    var cars = await _carRepository.GetAvailableCarsAsync();

    // Assert
    foreach (var car in cars)
    {
      Assert.IsNotNull(car.Images);
    }
  }

  [TestMethod]
  public async Task UpdateAsync_ShouldUpdateTimestamp()
  {
    // Arrange
    var car = await _carRepository.GetByIdAsync(1);
    var originalTimestamp = car.UpdatedAt;

    // Wait a bit to ensure timestamp difference
    await Task.Delay(1000);

    // Act
    car.Color = "Updated Color";
    await _carRepository.UpdateAsync(car);
    var updatedCar = await _carRepository.GetByIdAsync(1);

    // Assert
    Assert.IsTrue(updatedCar.UpdatedAt > originalTimestamp);
  }
}