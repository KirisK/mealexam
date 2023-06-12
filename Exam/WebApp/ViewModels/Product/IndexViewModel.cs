#pragma warning disable 1591

namespace WebApp.ViewModels.Product;

public class IndexViewModel
{
    public IEnumerable<Public.DTO.v1.UserProduct>? UserProducts { get; set; }
    public IEnumerable<Public.DTO.v1.Product>? Products { get; set; }
}