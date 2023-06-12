using System.Net.Http.Headers;
using App.BLL.Contracts;
using Public.DTO.v1;

#pragma warning disable 1591

namespace WebApp.HttpClient;

public class ProductClient : IProductClient
{
    
    private readonly System.Net.Http.HttpClient _httpClient;
    private readonly string _baseUrl = "http://localhost:5184/api/v1";
    private readonly string _productController = "Product";
    private readonly string _userProductController = "UserProduct";

    public ProductClient(System.Net.Http.HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<ClientResponse<IEnumerable<Product>>> GetAllProducts(string jwt, bool filterOutOwned = false)
    {
        var uri = $"{_baseUrl}/{_productController}?filterOutOwned={filterOutOwned}";

        var response = await AuthorizedClient(jwt).GetAsync(uri);
        var responseString = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? ClientResponse<IEnumerable<Product>>.Successful(responseString)
            : ClientResponse<IEnumerable<Product>>.Error(responseString);
    }

    public async Task<ClientResponse<Product?>> GetProduct(string jwt, Guid id)
    {
        var uri = $"{_baseUrl}/{_productController}/{id}";
        
        var response = await AuthorizedClient(jwt).GetAsync(uri);
        var responseString = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? ClientResponse<Product?>.Successful(responseString)
            : ClientResponse<Product?>.Error(responseString);
    }

    public async Task<ClientResponse<Product>> AddProduct(string jwt, Product product)
    {
        var uri = $"{_baseUrl}/{_productController}";
        
        var response = await AuthorizedClient(jwt).PostAsJsonAsync(uri, product);
        var responseString = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? ClientResponse<Product>.Successful(responseString)
            : ClientResponse<Product>.Error(responseString);
    }

    public async Task<ClientResponse<Product?>> UpdateProduct(string jwt, Product product)
    {
        var uri = $"{_baseUrl}/{_productController}/{product.Id}";
        
        var response = await AuthorizedClient(jwt).PutAsJsonAsync(uri, product);
        var responseString = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? ClientResponse<Product?>.Successful(responseString)
            : ClientResponse<Product?>.Error(responseString);
    }

    public async Task<ClientResponse<object>> DeleteProduct(string jwt, Guid id)
    {
        var uri = $"{_baseUrl}/{_productController}/{id}";
        
        var response = await AuthorizedClient(jwt).DeleteAsync(uri);
        var responseString = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? ClientResponse<object>.NoContentSuccessful()
            : ClientResponse<object>.Error(responseString);
    }

    public async Task<ClientResponse<IEnumerable<UserProduct>>> GetUserProducts(string jwt)
    {
        var uri = $"{_baseUrl}/{_userProductController}";

        var response = await AuthorizedClient(jwt).GetAsync(uri);
        var responseString = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? ClientResponse<IEnumerable<UserProduct>>.Successful(responseString)
            : ClientResponse<IEnumerable<UserProduct>>.Error(responseString);
    }

    public async Task<ClientResponse<UserProduct?>> GetUserProduct(string jwt, Guid id)
    {
        var uri = $"{_baseUrl}/{_userProductController}/{id}";

        var response = await AuthorizedClient(jwt).GetAsync(uri);
        var responseString = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? ClientResponse<UserProduct?>.Successful(responseString)
            : ClientResponse<UserProduct?>.Error(responseString);
    }

    public async Task<ClientResponse<UserProduct>> AddUserProduct(string jwt, UserProduct product)
    {
        var uri = $"{_baseUrl}/{_userProductController}";

        var response = await AuthorizedClient(jwt).PostAsJsonAsync(uri, product);
        var responseString = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? ClientResponse<UserProduct>.Successful(responseString)
            : ClientResponse<UserProduct>.Error(responseString);
    }

    public async Task<ClientResponse<UserProduct?>> UpdateUserProduct(string jwt, UserProduct product)
    {
        var uri = $"{_baseUrl}/{_userProductController}/{product.Id}";

        var response = await AuthorizedClient(jwt).PutAsJsonAsync(uri, product);
        var responseString = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? ClientResponse<UserProduct?>.Successful(responseString)
            : ClientResponse<UserProduct?>.Error(responseString);
    }

    public async Task<ClientResponse<object>> DeleteUserProduct(string jwt, Guid id)
    {
        var uri = $"{_baseUrl}/{_userProductController}/{id}";

        var response = await AuthorizedClient(jwt).DeleteAsync(uri);
        var responseString = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? ClientResponse<object>.Successful(responseString)
            : ClientResponse<object>.Error(responseString);
    }

    private System.Net.Http.HttpClient AuthorizedClient(string jwt)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        return _httpClient;
    }
}