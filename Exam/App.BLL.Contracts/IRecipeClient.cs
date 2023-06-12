using Public.DTO.v1;

namespace App.BLL.Contracts;

public interface IRecipeClient
{
    Task<ClientResponse<IEnumerable<Recipe>>> GetAllRecipes(string jwt, bool getAllPublicRecipes = false);
    
    Task<ClientResponse<Recipe?>> GetRecipe(string jwt, Guid id);    
    
    Task<ClientResponse<Recipe?>> AddRecipe(string jwt, Recipe recipe);
    
    Task<ClientResponse<Recipe?>> AddRecipeProduct(string jwt, RecipeProduct recipeProduct);
    
    Task<ClientResponse<Recipe?>> UpdateRecipe(string jwt, Recipe recipe);

    Task<ClientResponse<object>> DeleteAsync(string jwt, Guid id);
}