

using Catalog.API.Exceptions;
using Catalog.API.Models;
using Catalog.API.Products.GetProductById;
using FluentAssertions;
using Marten;
using Moq;

namespace Catalog.API.Tests.Products.GetProductById;

public class GetProductByIdHandlerTests
{
    [Fact]
    public async Task Handle_WhenProductExists_ShouldReturnProduct()
    {
        //Arrange (Hazırlık)
        var productId =Guid.NewGuid();
        var fakeProduct = new Product
        {
            Id = productId,
            Name = "Test Mock Phone",
            Category = new List<string> { "Smart Phone" },
            Description = "Test",
            ImageFile = "mock.png",
            Price = 9500
        };

        var mockSession=new Mock<IDocumentSession>();

        mockSession.Setup(s=>s.LoadAsync<Product>(productId,It.IsAny<CancellationToken>())).ReturnsAsync(fakeProduct);

        var query = new GetProductByIdQuery(productId);
        var handler=new GetProductByIdQueryHandler(mockSession.Object);

        //Act (Çalıştırma)

        var result=await handler.Handle(query,CancellationToken.None);

        //Assert (Doğrulama)
        result.Should().NotBeNull();
        result.Product.Name.Should().Be("Test Mock Phone");
        result.Product.Id.Should().Be(productId);


        mockSession.Verify(s=>s.LoadAsync<Product>(productId, It.IsAny<CancellationToken>()), Times.Once); 
    }

    [Fact]
    public async Task Handle_WhenProductDoesNotExist_ShouldThrowProductNotFoundException()
    {
        // 1. Arrange (Hazırlık)
        var nonExistingProductId=Guid.NewGuid();
        var mockSession= new Mock<IDocumentSession>();

        mockSession
        .Setup(s => s.LoadAsync<Product>(nonExistingProductId, It.IsAny<CancellationToken>()))
        .ReturnsAsync((Product)null!);

        var query=new GetProductByIdQuery(nonExistingProductId);
        var handler = new GetProductByIdQueryHandler(mockSession.Object);

        // 2. Act (Çalıştırma) & 3. Assert (Doğrulama) birlikte ele alınır
        Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

        // Doğrulama aşaması: Gerçekten ProductNotFoundException fırlattı mı?
        await act.Should().ThrowAsync<ProductNotFoundException>();


    }
}
