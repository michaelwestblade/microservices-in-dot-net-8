using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string CategoryName) : IQuery<GetProductByCategoryResult>;

public record GetProductByCategoryResult(IEnumerable<Product> Products);

public class GetProductByCategoryQueryValidator : AbstractValidator<GetProductByCategoryQuery>
{
    public GetProductByCategoryQueryValidator()
    {
        RuleFor(x => x.CategoryName).NotEmpty().NotNull().WithMessage("Category name cannot be empty");
    }
}


internal class GetProductByCategoryQueryHandler(IDocumentSession session): IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>()
            .Where(p => p.CategoryIds.Contains(query.CategoryName))
            .ToListAsync();
        
        return new GetProductByCategoryResult(products);
    }
}