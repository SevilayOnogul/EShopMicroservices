
namespace Ordering.Application.Dtos
{
    public record OrderItemDto(Guid OrderId,Guid PRoductId,int Quantity,decimal Price);
}
