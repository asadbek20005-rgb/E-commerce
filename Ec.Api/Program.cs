using Ec.Common.Models.Minio;
using Ec.Data.Context;
using Ec.Data.Entities;
using Ec.Data.Enums;
using Ec.Data.Repositories.Implementations;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Api.Admin;
using Ec.Service.Api.Chat;
using Ec.Service.Api.Client;
using Ec.Service.Api.Seller;
using Ec.Service.Hubs;
using Ec.Service.In_memory_Storage;
using Ec.Service.MemoryCache;
using Ec.Service.Minio;
using Ec.Service.Otp;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("Connection");
// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseLazyLoadingProxies().UseNpgsql(connection);
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRepository<User_Chat>, User_ChatRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IUser_ChatRepository, User_ChatRepository>();
builder.Services.AddScoped<IRepository<SearchHistory>, SearchHistoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IRepository<OTP>, OtpRepository>();
builder.Services.AddScoped<IRepository<Message>, MessageRepository>();
builder.Services.AddScoped<IRepository<Feedback>, FeedbackRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IRepository<Complaint>, ComplaintRepository>();
builder.Services.AddScoped<IRepository<Address>, AddressRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IProductContentRepository, ProductContentRepostory>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<SellerProductService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<MemoryCacheService>();
builder.Services.AddScoped<OtpService>();
builder.Services.AddScoped<RedisService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<FeedbackService>();
builder.Services.AddScoped<AddressService>();
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("enum", typeof(EnumRouteConstraint<Category>));
});

builder.Services.AddSignalR();

builder.WebHost.ConfigureKestrel(options =>
{
    // Maksimal so'rov hajmini oshirish (masalan, 30MB)
    options.Limits.MaxRequestBodySize = 30000000; // 30 MB
})
.ConfigureServices(services =>
{
    // IIS Integration (Agar kerak bo'lsa)
    services.AddControllersWithViews();
});


builder.Services.AddScoped(serviceProvider =>
{
    var minIOSettings = builder.Configuration.GetSection("MinIO").Get<MinIOSettings>();
    if (minIOSettings is null)
        throw new Exception("Model is null");
    return new MinioService(endpoint: minIOSettings.Endpoint,
        accessKey: minIOSettings.AccessKey,
        secretKey: minIOSettings.SecretKey);
});
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false,connectTimeout=20000,syncTimeout=20000,defaultDatabase=0"));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.MapType<Category>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(Category))
            .Select(name => new Microsoft.OpenApi.Any.OpenApiString(name))
            .Cast<Microsoft.OpenApi.Any.IOpenApiAny>()
            .ToList()
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<ChatHub>("chat-hub");
app.MapControllers();

app.Run();



public class EnumRouteConstraint<T> : IRouteConstraint where T : struct, Enum
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (values[routeKey] == null)
            return false;

        var value = values[routeKey]?.ToString();
        return Enum.TryParse(typeof(T), value, true, out _);
    }
}


