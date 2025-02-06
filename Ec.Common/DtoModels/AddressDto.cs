using System.ComponentModel.DataAnnotations;

namespace Ec.Common.DtoModels;

public class AddressDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
