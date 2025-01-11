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

}
