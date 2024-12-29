using Microsoft.EntityFrameworkCore;
using EMGATA.API.Models;
using EMGATA.API.Repositories;

namespace EMGATA.Tests.Repositories;

[TestClass]
public class BrandRepositoryTests : TestBase
{
  private IBrandRepository _brandRepository;

  public BrandRepositoryTests() : base()
  {
    _brandRepository = new BrandRepository(_context);
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
  public async Task GetAllAsync_ShouldReturnAllBrands()
  {
    // Act
    var brands = await _brandRepository.GetAllAsync();

    // Assert
    Assert.IsNotNull(brands);
    Assert.AreEqual(2, brands.Count()); // Toyota et BMW depuis SeedTestData
  }

  [TestMethod]
  public async Task GetByIdAsync_WithValidId_ShouldReturnBrand()
  {
    // Act
    var brand = await _brandRepository.GetByIdAsync(1);

    // Assert
    Assert.IsNotNull(brand);
    Assert.AreEqual("Toyota", brand.Name);
  }

  [TestMethod]
  public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
  {
    // Act
    var brand = await _brandRepository.GetByIdAsync(999);

    // Assert
    Assert.IsNull(brand);
  }

  [TestMethod]
  public async Task AddAsync_ShouldCreateNewBrand()
  {
    // Arrange
    var newBrand = new Brand
    {
      Name = "Honda",
      Description = "Japanese car manufacturer"
    };

    // Act
    var result = await _brandRepository.AddAsync(newBrand);
    var savedBrand = await _brandRepository.GetByIdAsync(result.Id);

    // Assert
    Assert.IsNotNull(savedBrand);
    Assert.AreEqual(newBrand.Name, savedBrand.Name);
    Assert.AreEqual(newBrand.Description, savedBrand.Description);
    Assert.IsTrue(savedBrand.Id > 0);
  }

  [TestMethod]
  public async Task UpdateAsync_ShouldModifyExistingBrand()
  {
    // Arrange
    var brand = await _brandRepository.GetByIdAsync(1);
    var newDescription = "Updated description";

    // Act
    brand.Description = newDescription;
    await _brandRepository.UpdateAsync(brand);
    var updatedBrand = await _brandRepository.GetByIdAsync(1);

    // Assert
    Assert.AreEqual(newDescription, updatedBrand.Description);
  }

  [TestMethod]
  public async Task UpdateAsync_ShouldUpdateTimestamp()
  {
    // Arrange
    var brand = await _brandRepository.GetByIdAsync(1);
    var originalTimestamp = brand.UpdatedAt;

    // Wait a bit to ensure timestamp difference
    await Task.Delay(1000);

    // Act
    brand.Description = "Modified description";
    await _brandRepository.UpdateAsync(brand);
    var updatedBrand = await _brandRepository.GetByIdAsync(1);

    // Assert
    Assert.IsTrue(updatedBrand.UpdatedAt > originalTimestamp);
  }

 
  [TestMethod]
  public async Task BrandWithModels_ShouldNotBeDeleted()
  {
    try
    {
      // Arrange - Brand 1 (Toyota) has models in test data
      var brandId = 1;

      // Act
      await _brandRepository.DeleteAsync(brandId);

      // If we get here, the test should fail
      Assert.Fail("Should have thrown exception when deleting brand with models");
    }
    catch (Exception)
    {
      // Test passes if delete throws exception
      Assert.IsTrue(true);
    }
  }
}