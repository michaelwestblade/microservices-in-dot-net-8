using BuildingBlocks.CQRS;
using Catalog.API.Models;
using MediatR;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price): ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductCommandHandler: ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // create product entity from command object
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            CategoryIds = command.Category,
            Description = command.Description,
            ImageFileName = command.ImageFile,
            Price = command.Price
        };

        // save to db

        // return CreateProductResult result
        return new CreateProductResult(product.Id);
    }
}