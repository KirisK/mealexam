using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.App
{
    public class RecipeProduct : DomainEntityMetaId
    {

        public int RequiredAmount { get; set; }
        public string Units { get; set; } = default!;
        
        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }

        public Product? Product { get; set; }
        
        [ForeignKey(nameof(Recipe))]
        public Guid RecipeId { get; set; }
        public Recipe? Recipe { get; set; }
    }
}