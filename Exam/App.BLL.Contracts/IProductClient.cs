using Public.DTO.v1;

namespace App.BLL.Contracts;

public interface IProductClient
{
    Task<ClientResponse<IEnumerable<Product>>> GetAllProducts(string jwt, bool filterOutOwned = false);
    Task<ClientResponse<Product?>> GetProduct(string jwt, Guid id);
    Task<ClientResponse<Product>> AddProduct(string jwt, Product product);
    Task<ClientResponse<Product?>> UpdateProduct(string jwt, Product product);
    Task<ClientResponse<object>> DeleteProduct(string jwt, Guid id);
    
    Task<ClientResponse<IEnumerable<UserProduct>>> GetUserProducts(string jwt);
    Task<ClientResponse<UserProduct?>> GetUserProduct(string jwt, Guid id);
    Task<ClientResponse<UserProduct>> AddUserProduct(string jwt, UserProduct product);
    Task<ClientResponse<UserProduct?>> UpdateUserProduct(string jwt, UserProduct product);
    Task<ClientResponse<object>> DeleteUserProduct(string jwt, Guid id);

}