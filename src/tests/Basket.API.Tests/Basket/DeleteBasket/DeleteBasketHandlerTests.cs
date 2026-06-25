using Basket.API.Basket.DeleteBasket;
using Basket.API.Data;
using FluentAssertions;
using Marten;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.API.Tests.Basket.DeleteBasket
{
    public class DeleteBasketHandlerTests
    {
        [Fact]
        public async Task Handle_WithValidUserName_ShouldDeleteBasketAndReturnTrue()
        {
            // 1. Arrange (Hazırlık)
            var userName = "testuser";
            var command=new DeleteBasketCommand(userName);
            var mockRepository = new Mock<IBasketRepository>();
            mockRepository.Setup(r=>r.DeleteBasket(userName,It.IsAny<CancellationToken>())).ReturnsAsync(true);
            var handler=new DeleteBasketCommandHandler(mockRepository.Object);

            // 2. Act (Çalıştırma)
            var result=await handler.Handle(command,CancellationToken.None);

            // 3. Assert (Doğrulama)
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue(); 

            mockRepository.Verify(r=>r.DeleteBasket(userName,It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
