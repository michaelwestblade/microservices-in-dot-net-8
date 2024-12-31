using Basket.API.Basket.StoreBasket;
using Basket.API.Models;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommad(string UserName): ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommad>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage("UserName is required");
    }
}

public class DeleteBasketHandler: ICommandHandler<DeleteBasketCommad, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommad command, CancellationToken cancellationToken)
    {
        // TODO delete basket in db
        // TODO update cache
        
        return new DeleteBasketResult(true);
    }
}