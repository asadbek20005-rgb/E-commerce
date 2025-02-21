using Ec.Common.DtoModels;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Extentions;
using Ec.Service.Helpers;
using Newtonsoft.Json.Linq;

namespace Ec.Service.Api.Seller;

public class AddressService(IAddressRepository addressRepository,
    IUserRepository userRepository)
{
    private readonly IAddressRepository _addressRepository = addressRepository;
    private readonly IUserRepository _userRepository = userRepository;
    public async Task<AddressDto> AddOrUpdateAddress(Guid sellerId, string address)
    {
        try
        {
            var seller = await CheckSeller(sellerId);
            await CheckAddressForExist(seller.Id, address);
            var addressEnitity = await GetAddress(sellerId);
            var (latitude, longitude) = await GetCoordinates(address);

            if (latitude == null || longitude == null)
            {
                Console.WriteLine("Manzil topilmadi.");
                return null;
            }

            if (addressEnitity is null)
            {
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

                addressEnitity.Name = address;
                addressEnitity.Latitude = latitude;
                addressEnitity.Longitude = longitude;
                addressEnitity.SellerId = seller.Id;
                await _addressRepository.UpdateAsync(addressEnitity);
                return addressEnitity.ParseToDto();
            }

        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    private async Task CheckAddressForExist(Guid sellerId, string addressName)
    {
        var address = await _addressRepository.GetAddressByName(sellerId, addressName);
        if (address is not null)
            throw new Exception($"Address {addressName} is already exist");
    }
    private async Task<(string Latitude, string Longitude)> GetCoordinates(string address)
    {
        string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json";

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "C# App");

        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}");
            return (null, null);
        }

        var result = await response.Content.ReadAsStringAsync();
        var json = JArray.Parse(result);

        return json.Count > 0
            ? (json[0]["lat"].ToString(), json[0]["lon"].ToString())
            : (null, null);
    }
    private async Task<User> CheckSeller(Guid sellerId)
    {
        var seller = await _userRepository.GetUserById(sellerId);
        if (seller == null)
            throw new Exception("Seller Not Found");
        Helper.CheckSellerRole(seller.Role);
        return seller;
    }

    private async Task<Address> GetAddress(Guid sellerId)
    {
        var addressEnitity = await _addressRepository.GetAddressBySellerId(sellerId);
        Helper.CheckAddressForNull(addressEnitity);
        return addressEnitity;
    }



    public async Task<AddressDto> GetAddressBySellerId(Guid sellerId)
    {
        try
        {
            var seller = await CheckSeller(sellerId);
            var address = await GetAddressById(seller.Id);
            return address.ParseToDto();

        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    private async Task<Address> GetAddressById(Guid sellerId)
    {
        var address = await _addressRepository.GetAddressBySellerId(sellerId);
        Helper.CheckAddressForNull(address);
        return address;
    }


    public async Task<bool> DeleteAddress(Guid sellerId)
    {
        try
        {
            var seller = await CheckSeller(sellerId);
            var address = await GetAddressById(seller.Id);
            await _addressRepository.DeleteAsync(address);
            return true;

        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}