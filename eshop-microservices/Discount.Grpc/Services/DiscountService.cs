using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger): DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if (coupon is null)
        {
            logger.LogWarning("Coupon with productName {ProductName} not found", request.ProductName);
            coupon = new Coupon{ProductName = "No Discount", Amount = 0, Description = "No Discount Found"};
        }
        
        logger.LogInformation($"Discount retrieved for ProductName: {coupon.ProductName}, Amount: {coupon.Amount}, Description: {coupon.Description}");
        
        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }
    
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        if (coupon is null)
        {
            logger.LogError("Failed to create discount");
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));
        }
        
        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync();
        
        logger.LogInformation($"Discount created with ProductName: {coupon.ProductName}, Amount: {coupon.Amount}, Description: {coupon.Description}");
        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        if (coupon is null)
        {
            logger.LogError("Failed to update discount");
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));
        }
        
        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();
        
        logger.LogInformation($"Discount updated with ProductName: {coupon.ProductName}, Amount: {coupon.Amount}, Description: {coupon.Description}");
        return coupon.Adapt<CouponModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if (coupon is null)
        {
            logger.LogWarning("Coupon with productName {ProductName} not found", request.ProductName);
            throw new RpcException(new Status(StatusCode.NotFound, $"No coupon found with ProductName {request.ProductName}."));
        }
        
        logger.LogInformation($"Discount retrieved for ProductName: {coupon.ProductName}, Amount: {coupon.Amount}, Description: {coupon.Description}");
        
        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();
        
        return new DeleteDiscountResponse{Success = true};
    }
}