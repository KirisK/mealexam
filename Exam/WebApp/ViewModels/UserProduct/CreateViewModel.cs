using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.UserProduct;

public class CreateViewModel
{
    public Public.DTO.v1.UserProduct UserProduct { get; set; } = default!;
    [ValidateNever]
    public List<SelectListItem> Products { get; set; } = default!;
}