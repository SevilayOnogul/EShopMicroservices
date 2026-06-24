using Moq;
using FluentAssertions;
using Xunit;
using Basket.API.Data; // IBasketRepository veya session katmanınız neredeyse
using Basket.API.Models;
using Basket.API.Basket.GetBasket;

namespace Basket.API.Tests.Basket.GetBasket;

public class GetBasketHandlerTests
{
    [Fact]
    public async Task Handle_WhenBasketExists_ShouldReturnCorrectBasket()
    {
        // 1. Arrange (Hazırlık)
        var userName = "testuser";
        var query = new GetBasketQuery(userName);

        // Basket.API'de kullandığınız veri katmanı arayüzünü (Örn: IBasketRepository) mock'luyoruz
        var mockRepository = new Mock<IBasketRepository>();

        // Sahte bir sepet içeriği hazırlıyoruz
        var fakeBasket = new ShoppingCart
        {
            UserName = userName,
            Items = new List<ShoppingCartItem>
            {
                new() { ProductId = Guid.NewGuid(), ProductName = "Test Product", Price = 100, Quantity = 2 }
            }
        };

        // Depodan bu kullanıcı istendiğinde sahte sepetimizi dönmesini söylüyoruz
        mockRepository
            .Setup(r => r.GetBasket(userName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeBasket);

        // Handler'ı ayağa kaldırıyoruz 
        var handler = new GetBasketQueryHandler(mockRepository.Object);

        // 2. Act (Çalıştırma)
        var result = await handler.Handle(query, CancellationToken.None);

        // 3. Assert (Doğrulama)
        result.Should().NotBeNull();
        result.Cart.UserName.Should().Be(userName);
        result.Cart.Items.Should().HaveCount(1);

        // Doğrulama: Depodan veri çekme metodu gerçekten 1 kere tetiklendi mi?
        mockRepository.Verify(r => r.GetBasket(userName, It.IsAny<CancellationToken>()), Times.Once);
    }
}