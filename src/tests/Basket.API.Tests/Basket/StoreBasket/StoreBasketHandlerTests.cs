using Moq;
using FluentAssertions;
using Xunit;
using Basket.API.Data;
using Basket.API.Models;
using Basket.API.Basket.StoreBasket;
using Discount.Grpc; 

namespace Basket.API.Tests.Basket.StoreBasket;

public class StoreBasketHandlerTests
{
    [Fact]
    public async Task Handle_WithValidCommand_ShouldDeductDiscount_StoreBasket_AndReturnUserName()
    {
        // 1. Arrange (Hazırlık)
        var userName = "testuser";
        var incomingCart = new ShoppingCart
        {
            UserName = userName,
            Items = new List<ShoppingCartItem>
            {
                new() { ProductId = Guid.NewGuid(), ProductName = "Keyboard", Price = 1500, Quantity = 1 }
            }
        };

        var command = new StoreBasketCommand(incomingCart);

        var mockRepository = new Mock<IBasketRepository>();

        var mockDiscountProto = new Mock<DiscountProtoService.DiscountProtoServiceClient>();

        // Sahte gRPC indirim cevabı hazırlıyoruz (Örn: 200 TL indirim kuponu)
        var fakeCoupon = new CouponModel { ProductName = "Keyboard", Amount = 200, Description = "Keyboard Discount" };

        mockDiscountProto
            .Setup(d => d.GetDiscountAsync(It.IsAny<GetDiscountRequest>(), null, null, It.IsAny<CancellationToken>()))
            .Returns(new Grpc.Core.AsyncUnaryCall<CouponModel>(
                Task.FromResult(fakeCoupon),
                Task.FromResult(new Grpc.Core.Metadata()),
                () => Grpc.Core.Status.DefaultSuccess,
                () => new Grpc.Core.Metadata(),
                () => { }));

        // Repository kuralı
        mockRepository
            .Setup(r => r.StoreBasket(incomingCart, It.IsAny<CancellationToken>()))
            .ReturnsAsync(incomingCart);

        // Handler'ı iki bağımlılıkla beraber ayağa kaldırıyoruz
        var handler = new StoreBasketCommandHandler(mockRepository.Object, mockDiscountProto.Object);

        // 2. Act (Çalıştırma)
        var result = await handler.Handle(command, CancellationToken.None);

        // 3. Assert (Doğrulama)
        result.Should().NotBeNull();
        result.UserName.Should().Be(userName); // Dönüş tipi artık string UserName

        // Fiyat kontrolü: 1500 TL olan klavye, 200 TL indirimle 1300 TL'ye düşmüş mü?
        incomingCart.Items.First().Price.Should().Be(1300);

        // Doğrulama: Metotlar tetiklendi mi?
        mockRepository.Verify(r => r.StoreBasket(incomingCart, It.IsAny<CancellationToken>()), Times.Once);
    }
}