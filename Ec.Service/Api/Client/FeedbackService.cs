using Ec.Common.Constants;
using Ec.Common.Models.Feedback;
using Ec.Data.Entities;
using Ec.Data.Enums;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Exceptions;
using Ec.Service.Helpers;

namespace Ec.Service.Api.Client;

public class FeedbackService(IFeedbackRepository feedbackRepository,
    IUserRepository userRepository,
    IProductRepository productRepository)
{
    private readonly IFeedbackRepository _feedbackRepository = feedbackRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IProductRepository _productRepository = productRepository;

    public async Task Create(Guid clientId, Guid sellerId, Guid prodcutId, CreateFeedbackModel model, Rank rank)
    {
        try
        {
            var client = await CheckClient(clientId);
            var seller = await CheckSeller(sellerId);
            var product = await CheckProduct(seller.Id, prodcutId);
            var newFeedback = new Feedback
            {
                Comment = model.Comment,
                Rank = rank,
                SellerId = seller.Id,
                ClientId = client.Id,
                ProductId = product.Id,
            };

            await _feedbackRepository.AddAsync(newFeedback);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

    }



    private async Task<User> CheckSeller(Guid sellerId)
    {
        var seller = await _userRepository.GetUserById(sellerId);
        if (seller is null)
            throw new SellerNotFoundException();
        Helper.CheckSellerRole(seller.Role);
        return seller;
    }
    private async Task<User> CheckClient(Guid clientId)
    {
        var client = await _userRepository.GetUserById(clientId);
        Helper.CheckClientRole(client.Role);
        return client;
    }
    private async Task<Product> CheckProduct(Guid sellerId, Guid prodcutId)
    {
        var product = await _productRepository.GetProductById(sellerId, prodcutId);
        if (product is null)
            throw new ProductNotFoundException();
        return product;
    }
   

   
}
