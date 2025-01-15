using Ec.Common.Models.Minio;
using Ec.Data.Context;
using Ec.Data.Entities;
using Ec.Data.Repositories.Implementations;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Api.Admin;
using Ec.Service.Api.Client;
using Ec.Service.Api.Seller;
using Ec.Service.In_memory_Storage;
using Ec.Service.MemoryCache;
using Ec.Service.Otp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Minio;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("Connection");
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connection);
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRepository<User_Chat>, User_ChatRepository>();
builder.Services.AddScoped<IRepository<Statistic>, StatisticRepository>();
builder.Services.AddScoped<IRepository<SearchHistory>, SearchHistoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IRepository<OTP>, OtpRepository>();
builder.Services.AddScoped<IRepository<Message>, MessageRepository>();
builder.Services.AddScoped<IRepository<Feedback>, FeedbackRepository>();
builder.Services.AddScoped<IRepository<Complaint>, ComplaintRepository>();
builder.Services.AddScoped<IRepository<Chat>, ChatRepository>();
builder.Services.AddScoped<IRepository<Address>, AddressRepository>();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<MemoryCacheService>();
builder.Services.AddScoped<OtpService>();
builder.Services.AddScoped<RedisService>();

builder.Services.Configure<MinIOSettings>(builder.Configuration.GetSection("MinIO"));
builder.Services.AddSingleton<MinioClient>(serviceProvider =>
{
    var minIOSettings = serviceProvider.GetRequiredService<IOptions<MinIOSettings>>().Value;
    return (MinioClient?)new MinioClient()
        .WithEndpoint(minIOSettings.Endpoint)
        .WithCredentials(minIOSettings.AccessKey, minIOSettings.SecretKey);
});
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false,connectTimeout=20000,syncTimeout=20000,defaultDatabase=0"));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
