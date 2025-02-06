using Ec.Common.Constants;
using Ec.Common.DtoModels;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Extentions;
using Newtonsoft.Json.Linq;

namespace Ec.Service.Api.Seller;

public class AddressService(IAddressRepository addressRepository,
    IUserRepository userRepository)
{
    private readonly IAddressRepository _addressRepository = addressRepository;
    private readonly IUserRepository _userRepository = userRepository;
    public async Task<AddressDto> Create(Guid sellerId, string address)
    {
        var seller = await CheckSeller(sellerId);
        string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json";

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                JArray json = JArray.Parse(result);

                if (json.Count > 0)
                {
                    string latitude = json[0]["lat"].ToString();
                    string longitude = json[0]["lon"].ToString();

                    var newAddress = new Address
                    {
                        Name = address,
                        Latitude = latitude,
                        Longitude = longitude,
                        SellerId = seller.Id,
                    };
                    await _addressRepository.AddAsync(newAddress);
                    return newAddress.ParseToDto();
                }
                else
                {
                    Console.WriteLine("Manzil topilmadi.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
                return null;
            }
        }

    }


    private async Task<User> CheckSeller(Guid sellerId)
    {
        var seller = await _userRepository.GetUserById(sellerId);
        if (seller == null)
            throw new Exception("Seller Not Found");
        CheckSellerRole(seller.Role);
        return seller;
    }

    private void CheckSellerRole(string role)
    {
        if (role != Constants.SellerRole) throw new Exception("Role Must Be seller");
    }
}