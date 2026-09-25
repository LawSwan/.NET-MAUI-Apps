using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using AssessmentApp.Models;

namespace AssessmentApp.Services;

public sealed class ApiService(HttpClient httpClient)
{
    public async Task<bool> AuthenticateAsync(string username, string password)
    {
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        using var request = new HttpRequestMessage(HttpMethod.Post, "api/auth/login");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        using var response = await httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<IReadOnlyList<Item>> GetItemsAsync()
    {
        return await httpClient.GetFromJsonAsync<List<Item>>("api/items") ?? [];
    }

    public async Task<bool> AddItemAsync(Item item)
    {
        using var response = await httpClient.PostAsJsonAsync("api/items", item);
        return response.IsSuccessStatusCode;
    }
}