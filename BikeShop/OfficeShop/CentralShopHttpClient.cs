namespace OfficeShop;

public class CentralShopHttpClient
{
    private readonly HttpClient _httpClient;

    public CentralShopHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<(bool success, string error)> RetBike(string customerId)
    {
        var response = await _httpClient.PutAsync($"/customer/{customerId}/rent", null);
        if (!response.IsSuccessStatusCode)
        {
            return (false, response.ReasonPhrase ?? "");
        }
        return (true, "");
    }
    
    public async Task<(bool success, string error)> ReleaseBike(string customerId)
    {
        var response = await _httpClient.PutAsync($"/customer/{customerId}/release", null);
        if (!response.IsSuccessStatusCode)
        {
            return (false, response.ReasonPhrase ?? "");
        }
        return (true, "");
    }
}