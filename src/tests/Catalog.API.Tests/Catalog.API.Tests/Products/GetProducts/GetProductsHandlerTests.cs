using Moq;
using FluentAssertions;
using Xunit;
using Marten;
using Catalog.API.Products.DeleteProduct;

namespace Catalog.API.Tests.Products.DeleteProduct;

public class DeleteProductHandlerTests
{
    [Fact]
    public async Task Handle_WithValidId_ShouldDeleteProductAndReturnTrue()
    {
        // 1. Arrange (Hazırlık)
        var productId = Guid.NewGuid();
        var command = new DeleteProductCommand(productId);

        var mockSession = new Mock<IDocumentSession>();

        mockSession
            .Setup(s => s.Delete<Catalog.API.Models.Product>(productId));

        mockSession
            .Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new DeleteProductCommandHandler(mockSession.Object);

        // 2. Act (Çalıştırma)
        var result = await handler.Handle(command, CancellationToken.None);

        // 3. Assert (Doğrulama)
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue(); 

        // Doğrulama:
        mockSession.Verify(s => s.Delete<Catalog.API.Models.Product>(productId), Times.Once);
        mockSession.Verify(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}