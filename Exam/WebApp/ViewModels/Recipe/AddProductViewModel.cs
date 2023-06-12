using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Public.DTO.v1;

#pragma warning disable 1591

namespace WebApp.ViewModels.Recipe;

public class AddProductViewModel
{
    [ValidateNever]
    public Public.DTO.v1.Recipe Recipe { get; set; } = default!;
    [ValidateNever]
    public IEnumerable<SelectListItem> Products { get; set; } = default!;
    public RecipeProduct RecipeProduct { get; set; } = default!;
}