using Domain.Base;

namespace Domain.App
{
    public class RecipeProduct : DomainEntityMetaId
    {

        public int RequiredAmount { get; set; }
        
        public Guid ProductId { get; set; }

        public Product? Product { get; set; }
        
        public Guid RecipeId { get; set; }
        public Recipe? Recipe { get; set; }
    }
}