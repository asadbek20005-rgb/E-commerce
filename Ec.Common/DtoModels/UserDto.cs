namespace Ec.Common.DtoModels;

public class UserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Username { get; set; }
    public required string Role { get; set; }
}
