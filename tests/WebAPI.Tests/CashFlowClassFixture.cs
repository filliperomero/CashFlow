using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebAPI.Test;

public class CashFlowClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    
    public CashFlowClassFixture(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }

    protected async Task<HttpResponseMessage> PostAsync(string requestUri, object request, string token = "", string culture = "en")
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(culture);
        
        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }
    
    protected async Task<HttpResponseMessage> GetAsync(string requestUri, string token = "", string culture = "en")
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(culture);
        
        return await _httpClient.GetAsync(requestUri);
    }
    
    protected async Task<HttpResponseMessage> DeleteAsync(string requestUri, string token = "", string culture = "en")
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(culture);
        
        return await _httpClient.DeleteAsync(requestUri);
    }

    private void AuthorizeRequest(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return;
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void ChangeRequestCulture(string culture)
    {
        if (string.IsNullOrWhiteSpace(culture)) return;
        
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
    }
}