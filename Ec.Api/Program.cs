using Ec.Common.Models.Jwt;
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
using Ec.Service.Jwt;
using Ec.Service.MemoryCache;
using Ec.Service.Minio;
using Ec.Service.Otp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("Connection");
var jwtSettings = builder.Configuration.GetSection("JwtSetting").Get<JwtSetting>();
var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));// Add services to the container.
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
builder.Services.AddScoped<JwtService>();
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("enum", typeof(EnumRouteConstraint<Category>));
});

builder.Services.AddSignalR();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 30000000; 
})
.ConfigureServices(services =>
{
    services.AddControllersWithViews();
});



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = securityKey,
        ClockSkew = TimeSpan.FromDays(1),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = false,
    };
});


builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "JWT Bearer. : \"Authorization: Bearer { token } \"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

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

app.UseAuthentication();            
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


