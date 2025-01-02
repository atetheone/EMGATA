using AutoMapper;
using EMGATA.API.Controllers;
using EMGATA.API.Dtos;
using EMGATA.API.Models;
using EMGATA.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EMGATA.Tests.Controllers;

[TestClass]
public class ModelControllerTests
{
  private Mock<IModelService> _mockModelService;
  private Mock<IMapper> _mockMapper;
  private ModelController _controller;

  [TestInitialize]
  public void Setup()
  {
    _mockModelService = new Mock<IModelService>();
    _mockMapper = new Mock<IMapper>();
    _controller = new ModelController(_mockModelService.Object, _mockMapper.Object);
  }

  [TestMethod]
  public async Task GetModels_ReturnsOkResultWithModels()
  {
    // Arrange
    var models = new List<Model>
    {
      new() { Id = 1, Name = "Camry", BrandId = 1 },
      new() { Id = 2, Name = "Corolla", BrandId = 1 }
    };
    var modelDtos = new List<ModelDto>
    {
      new() { Id = 1, Name = "Camry", Brand = new BrandDto { Id = 1 } },
      new() { Id = 2, Name = "Corolla", Brand = new BrandDto { Id = 1 } }
    };

    _mockModelService.Setup(x => x.GetAllModelsAsync())
        .ReturnsAsync(models);
    _mockMapper.Setup(x => x.Map<IEnumerable<ModelDto>>(models))
        .Returns(modelDtos);

    // Act
    var result = await _controller.GetModels();

    // Assert
    Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    var okResult = result.Result as OkObjectResult;
    var returnedModels = okResult.Value as IEnumerable<ModelDto>;
    Assert.AreEqual(2, returnedModels.Count());
  }

  [TestMethod]
  public async Task GetModel_WithValidId_ReturnsOkResult()
  {
    // Arrange
    var model = new Model { Id = 1, Name = "Camry", BrandId = 1 };
    var modelDto = new ModelDto { Id = 1, Name = "Camry", Brand = new BrandDto { Id = 1 } };

    _mockModelService.Setup(x => x.GetModelByIdAsync(1))
      .ReturnsAsync(model);
    _mockMapper.Setup(x => x.Map<ModelDto>(model))
      .Returns(modelDto);

    // Act
    var result = await _controller.GetModel(1);

    // Assert
    Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    var okResult = result.Result as OkObjectResult;
    var returnedModel = okResult.Value as ModelDto;
    Assert.AreEqual("Camry", returnedModel.Name);
  }

  [TestMethod]
  public async Task GetModel_WithInvalidId_ReturnsNotFound()
  {
    // Arrange
    _mockModelService.Setup(x => x.GetModelByIdAsync(999))
        .ReturnsAsync((Model)null);

    // Act
    var result = await _controller.GetModel(999);

    // Assert
    Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
  }

  [TestMethod]
  public async Task GetModelsByBrand_ReturnsOkResultWithModels()
  {
    // Arrange
    var brandId = 1;
    var models = new List<Model>
    {
      new() { Id = 1, Name = "Camry", BrandId = brandId },
      new() { Id = 2, Name = "Corolla", BrandId = brandId }
    };
    var modelDtos = new List<ModelDto>
    {
      new() { Id = 1, Name = "Camry", Brand = new BrandDto { Id = brandId } },
      new() { Id = 2, Name = "Corolla", Brand = new BrandDto { Id = brandId } }
    };

    _mockModelService.Setup(x => x.GetModelsByBrandAsync(brandId))
      .ReturnsAsync(models);
    _mockMapper.Setup(x => x.Map<IEnumerable<ModelDto>>(models))
      .Returns(modelDtos);

    // Act
    var result = await _controller.GetModelsByBrand(brandId);

    // Assert
    Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    var okResult = result.Result as OkObjectResult;
    var returnedModels = okResult.Value as IEnumerable<ModelDto>;
    Assert.AreEqual(2, returnedModels.Count());
    Assert.IsTrue(returnedModels.All(m => m.Brand.Id == brandId));
  }

  [TestMethod]
  public async Task CreateModel_WithValidData_ReturnsCreatedResult()
  {
    // Arrange
    var createDto = new CreateModelDto
    {
      BrandId = 1,
      Name = "Camry",
      Description = "Sedan"
    };
    var model = new Model { Id = 1, BrandId = 1, Name = "Camry" };
    var modelDto = new ModelDto { Id = 1, Name = "Camry", Brand = new BrandDto { Id = 1 } };

    _mockMapper.Setup(x => x.Map<Model>(createDto))
        .Returns(model);
    _mockModelService.Setup(x => x.CreateModelAsync(model))
        .ReturnsAsync(model);
    _mockMapper.Setup(x => x.Map<ModelDto>(model))
        .Returns(modelDto);

    // Act
    var result = await _controller.CreateModel(createDto);

    // Assert
    Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
    var createdResult = result.Result as CreatedAtActionResult;
    Assert.AreEqual("GetModel", createdResult.ActionName);
    Assert.AreEqual(1, createdResult.RouteValues["id"]);
  }

  [TestMethod]
  public async Task UpdateModel_WithValidData_ReturnsOkResult()
  {
    // Arrange
    var id = 1;
    var updateDto = new UpdateModelDto
    {
      BrandId = 1,
      Name = "Updated Camry",
      Description = "Updated Sedan"
    };
    var existingModel = new Model { Id = id, BrandId = 1, Name = "Camry" };

    var updatedModelDto = new ModelDto { Id = id, Name = "Updated Camry" };

    _mockModelService.Setup(x => x.GetModelByIdAsync(id))
        .ReturnsAsync(existingModel);
    _mockMapper.Setup(x => x.Map(updateDto, existingModel))
        .Returns(existingModel);
    _mockMapper.Setup(x => x.Map<ModelDto>(existingModel))
        .Returns(updatedModelDto);

    // Act
    var result = await _controller.UpdateModel(id, updateDto);

    // Assert
    Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    var okResult = result as OkObjectResult;
    var returnedDto = okResult.Value as ModelDto;
    Assert.AreEqual("Updated Camry", returnedDto.Name);
    _mockModelService.Verify(x => x.UpdateModelAsync(existingModel), Times.Once);
  }

  [TestMethod]
  public async Task DeleteModel_WithValidId_ReturnsNoContent()
  {
    // Arrange
    var id = 1;
    _mockModelService.Setup(x => x.DeleteModelAsync(id))
        .Returns(Task.CompletedTask);

    // Act
    var result = await _controller.DeleteModel(id);

    // Assert
    Assert.IsInstanceOfType(result, typeof(NoContentResult));
    _mockModelService.Verify(x => x.DeleteModelAsync(id), Times.Once);
  }
}