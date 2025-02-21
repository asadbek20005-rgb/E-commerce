namespace Ec.Common.DtoModels;

public class MessageDto
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public string FromUser { get; set; }    
    public MessageContentDto Content { get; set; }
}
