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
    public static AddressDto ParseToDto(this Address address)
    {
        return address.Adapt<AddressDto>();
    }
    public static MessageDto ParseToDto(this Message message)
    {
        return message.Adapt<MessageDto>();
    }

    public static ProductDto ParseToDto(this Product product)
    {
        return product.Adapt<ProductDto>();
    }

    public static ChatDto ParseToDto(this Chat chat)
    {
        return chat.Adapt<ChatDto>();
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


    public static List<MessageDto> ParseToDtos(this List<Message> messages)
    {
        if (messages == null || messages.Count == 0) return new List<MessageDto>(); 

        var messagesDtos = messages.Select(message => message.ParseToDto()).ToList();
        return messagesDtos;

    }

    public static List<ProductDto> ParseToDtos(this List<Product> products)
    {
        if (products == null || products.Count == 0)
            return new List<ProductDto>();

        var productDtos = products.Select(product => product.ParseToDto()).ToList();
        return productDtos;
    }

    public static List<ChatDto> ParseToDtos(this List<Chat> chats)
    {
        if (chats is null || chats.Count == 0) { return new List<ChatDto>(); }
        var chatDtos = chats.Select(chat => chat.ParseToDto()).ToList();
        return chatDtos;
    }

}
