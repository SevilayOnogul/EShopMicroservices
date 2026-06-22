

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
        var productId=Guid.NewGuid();
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

        var result=await handler.Handle(query,CancellationToken.None);


        result.Should().NotBeNull();
        result.Product.Name.Should().Be("Test Mock Phone");
        result.Product.Id.Should().Be(productId);


        mockSession.Verify(s=>s.LoadAsync<Product>(productId, It.IsAny<CancellationToken>()), Times.Once); 
    }
}
