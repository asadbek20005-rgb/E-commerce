namespace Ec.Common.Constants;

public static class Constants
{
    public const string SellerRole = "seller";
    public const string ClientRole = "client";
    public const string AdminRole = "admin";
    public const string SellerKey = "seller";
    public const string UserDtos = "user dtos";
    public const string ProductDtos = "product dtos";
    public const string ProductCacheKey = "product";
    public const string BucketName = "my-bucket2";
    private static TimeSpan memoryExpirationTime = TimeSpan.FromMinutes(30);
    public static TimeSpan MemoryExpirationTime { get => memoryExpirationTime; set => memoryExpirationTime = value; }
}
