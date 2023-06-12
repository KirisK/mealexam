using System.Net.Http.Headers;
using App.BLL.Contracts;
using Public.DTO.v1;

namespace WebApp.HttpClient;

public class RecipeClient : IRecipeClient
{
    private readonly System.Net.Http.HttpClient _httpClient;
    private readonly string _baseUrl = "http://localhost:5184/api/v1";
    private readonly string _controller = "Recipe";

    public RecipeClient(System.Net.Http.HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ClientResponse<IEnumerable<Recipe>>> GetAllRecipes(string jwt, bool getAllPublicRecipes = false)
    {
        var uri = $"{_baseUrl}/{_controller}?getAllPublicRecipes={getAllPublicRecipes}";

        var response = await AuthorizedClient(jwt).GetAsync(uri);
        var responseString = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode 
            ? ClientResponse<IEnumerable<Recipe>>.Successful(responseString) 
            : ClientResponse<IEnumerable<Recipe>>.Error(responseString);
    }

    public async Task<ClientResponse<Recipe?>> GetRecipe(string jwt, Guid id)
    {
        var uri = $"{_baseUrl}/{_controller}/{id}";
        
        var response = await AuthorizedClient(jwt).GetAsync(uri);
        var responseString = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode 
            ? ClientResponse<Recipe?>.Successful(responseString) 
            : ClientResponse<Recipe?>.Error(responseString);
    }

    public async Task<ClientResponse<Recipe?>> AddRecipe(string jwt, Recipe recipe)
    {
        var uri = $"{_baseUrl}/{_controller}";
        
        var response = await AuthorizedClient(jwt).PostAsJsonAsync(uri, recipe);
        var responseString = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode 
            ? ClientResponse<Recipe?>.Successful(responseString)
            : ClientResponse<Recipe?>.Error(responseString);

    }

    public async Task<ClientResponse<Recipe?>> AddRecipeProduct(string jwt, RecipeProduct recipeProduct)
    {
        var uri = $"{_baseUrl}/{_controller}/AddProduct/{recipeProduct.RecipeId}";
        
        var response = await AuthorizedClient(jwt).PostAsJsonAsync(uri, recipeProduct);
        var responseString = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode 
            ? ClientResponse<Recipe?>.NoContentSuccessful()
            : ClientResponse<Recipe?>.Error(responseString);
    }

    public async Task<ClientResponse<Recipe?>> UpdateRecipe(string jwt, Recipe recipe)
    {
        var uri = $"{_baseUrl}/{_controller}/{recipe.Id}";
        
        var response = await AuthorizedClient(jwt).PutAsJsonAsync(uri, recipe);
        var responseString = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode 
            ? ClientResponse<Recipe?>.Successful(responseString)
            : ClientResponse<Recipe?>.Error(responseString);
    }

    public async Task<ClientResponse<object>> DeleteAsync(string jwt, Guid id)
    {
        var uri = $"{_baseUrl}/{_controller}/{id}";
        
        var response = await AuthorizedClient(jwt).DeleteAsync(uri);
        var responseString = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode 
            ? ClientResponse<object>.NoContentSuccessful()
            : ClientResponse<object>.Error(responseString);
    }

    private System.Net.Http.HttpClient AuthorizedClient(string jwt)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        return _httpClient;
    }
}