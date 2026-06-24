using Catalog.API.Models;
using Catalog.API.Products.UpdateProduct;
using FluentAssertions;
using Marten;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.API.Tests.Products.UpdateProduct
{
    public class UpdateProductHandlerTests
    {
        [Fact]
        public async Task Handle_WithValidCommand_ShouldUpdateProductAndReturnTrue()
        { 
            // 1. Arrange(Hazırlık)
            var productId= Guid.NewGuid();

            var command = new UpdateProductCommand(
                        Id: productId,
                        Name: "Updated Phone Name",
                        Category: new List<string> { "Electronics" },
                        Description: "Updated Description",
                        ImageFile: "updated_phone.png",
                        Price: 6000
                    );

            var mockSession =new Mock<IDocumentSession>();

            var existingProduct = new Product { Id = productId, Name = "Old Name", Price = 5000 };
            mockSession
                .Setup(s => s.LoadAsync<Product>(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            mockSession
            .Setup(s => s.Update(It.IsAny<Product>()));

            mockSession
                .Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new UpdateProductCommandHandler(mockSession.Object);

            // 2. Act (Çalıştırma)
            var result = await handler.Handle(command, CancellationToken.None);

            // 3. Assert (Doğrulama)
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();

            // Metotların doğru tetiklendiğini doğruluyorum
            mockSession.Verify(s => s.LoadAsync<Product>(productId, It.IsAny<CancellationToken>()), Times.Once);
            mockSession.Verify(s => s.Update(It.IsAny<Product>()), Times.Once);
            mockSession.Verify(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}
