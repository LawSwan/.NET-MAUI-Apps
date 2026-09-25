using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using AssessmentApp.Models;

namespace AssessmentApp.Services;

/// <summary>
/// Wraps every call the app makes to the AssessmentApi web service.
/// </summary>
/// <param name="httpClient">Client whose BaseAddress points at the web service (set in MauiProgram).</param>
public sealed class ApiService(HttpClient httpClient)
{
    /// <summary>
    /// Sends the user name and password to POST api/auth/login using HTTP Basic Authentication.
    /// </summary>
    /// <returns>True if the web service accepted the credentials.</returns>
    /// <exception cref="HttpRequestException">The web service could not be reached.</exception>
    public async Task<bool> AuthenticateAsync(string username, string password)
    {
        // Basic auth sends "username:password" encoded as base64 in the Authorization header.
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        using var request = new HttpRequestMessage(HttpMethod.Post, "api/auth/login");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        using var response = await httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    /// <summary>
    /// Gets every stored item from GET api/items.
    /// </summary>
    /// <exception cref="HttpRequestException">The web service could not be reached.</exception>
    public async Task<IReadOnlyList<Item>> GetItemsAsync()
    {
        return await httpClient.GetFromJsonAsync<List<Item>>("api/items") ?? [];
    }

    /// <summary>
    /// Sends a new item as JSON to POST api/items, which saves it to the database.
    /// </summary>
    /// <returns>
    /// The response status: OK when saved, Conflict when the item ID is already used,
    /// or BadRequest when a field is missing.
    /// </returns>
    /// <exception cref="HttpRequestException">The web service could not be reached.</exception>
    public async Task<HttpStatusCode> AddItemAsync(Item item)
    {
        using var response = await httpClient.PostAsJsonAsync("api/items", item);
        return response.StatusCode;
    }
}
