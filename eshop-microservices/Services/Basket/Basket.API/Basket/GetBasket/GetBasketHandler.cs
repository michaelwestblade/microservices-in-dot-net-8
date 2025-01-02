using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;

namespace Basket.API.Basket.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResponse>;

public record GetBasketResponse(ShoppingCart Cart);

public class GetBasketQueryHandler(IBasketRepository repository): IQueryHandler<GetBasketQuery, GetBasketResponse>
{
    public async Task<GetBasketResponse> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(request.UserName);
        
        return new GetBasketResponse(new ShoppingCart("SWN"));
    }
}