using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
        
        context.Database.MigrateAsync().GetAwaiter().GetResult();

        await SeedAsync(context);
    }

    private static async Task SeedAsync(ApplicationDBContext context)
    {
        await SeedCustomerAsync(context);
    }

    private static async Task SeedCustomerAsync(ApplicationDBContext context)
    {
        if (!await context.Customers.AnyAsync())
        {
            await context.Customers.AddRangeAsync(InitialData.Customers);
            await context.SaveChangesAsync();
        }
    }
    
    private static async Task SeedProductAsync(ApplicationDBContext context)
    {
        if (!await context.Products.AnyAsync())
        {
            await context.Products.AddRangeAsync(InitialData.Products);
            await context.SaveChangesAsync();
        }
    }
    
    private static async Task SeedOrderAndItemsAsync(ApplicationDBContext context)
    {
        if (!await context.Orders.AnyAsync())
        {
            await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
            await context.SaveChangesAsync();
        }
    }
}