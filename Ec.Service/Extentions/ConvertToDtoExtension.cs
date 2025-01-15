using Ec.Common.DtoModels;
using Ec.Data.Entities;
using Mapster;

namespace Ec.Service.Extentions;

public static class ConvertToDtoExtension
{
    public static UserDto ParseToDto(this User user)
    {
        return user.Adapt<UserDto>();
    }

    public static ProductDto ParseToDto(this Product product)
    {
        return product.Adapt<ProductDto>();
    }

    public static List<UserDto> ParseToDtos(this List<User> users)
    {
        if (users == null || users.Count == 0) return new List<UserDto>();
        var userDtos = new List<UserDto>();
        foreach (var user in users)
        {
            userDtos.Add(user.ParseToDto());
        }
        return userDtos;
    }

    public static List<ProductDto> ParseToDtos(this List<Product> products)
    {
        if (products == null || products.Count == 0)
            return new List<ProductDto>();
        var productDtos = products.Select(product => product.ParseToDto()).ToList();
        return productDtos;
    }

}
