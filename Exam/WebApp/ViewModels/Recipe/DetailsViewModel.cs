namespace WebApp.ViewModels.Recipe;

public class DetailsViewModel
{
    public Public.DTO.v1.Recipe Recipe { get; set; } = default!;
    public IEnumerable<Public.DTO.v1.UserProduct> UserProducts { get; set; } = default!;
}