using Moq;
using FluentAssertions;
using Xunit;
using Marten;
using Catalog.API.Products.CreateProduct;
using Catalog.API.Models;

namespace Catalog.API.Tests.Products.CreateProduct;

public class CreateProductHandlerTests
{
    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateProductAndReturnId()
    {
        // 1. Arrange (Hazırlık)
        var command = new CreateProductCommand(
            Name: "New Test Phone",
            Category: new List<string> { "Electronics" },
            Description: "New Description",
            ImageFile: "new_phone.png",
            Price: 12000
        );

        var mockSession = new Mock<IDocumentSession>();

      
        mockSession
            .Setup(s => s.Store(It.IsAny<Product[]>()))
            .Callback<Product[]>(products => products[0].Id = Guid.NewGuid());

        mockSession
            .Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateProductCommandHandler(mockSession.Object);

        // 2. Act (Çalıştırma)
        var result = await handler.Handle(command, CancellationToken.None);

        // 3. Assert (Doğrulama)
        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);

        // (Verify): Metotlar gerçekten tetiklendi mi?
        mockSession.Verify(s => s.Store(It.IsAny<Product[]>()), Times.Once);
        mockSession.Verify(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}