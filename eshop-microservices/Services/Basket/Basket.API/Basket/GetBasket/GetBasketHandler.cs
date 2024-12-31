using Basket.API.Models;
using BuildingBlocks.CQRS;

namespace Basket.API.Basket.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResponse>;

public record GetBasketResponse(ShoppingCart Cart);

public class GetBasketQueryHandler: IQueryHandler<GetBasketQuery, GetBasketResponse>
{
    public async Task<GetBasketResponse> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        // TODO: get basket from db
        //var basket = await _repository.GetBasket(request.UserName)
        
        return new GetBasketResponse(new ShoppingCart("SWN"));
    }
}