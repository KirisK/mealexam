using Domain.Base;

namespace Public.DTO.v1
{
    public class RecipeProduct : DomainEntityMetaId
    {

        public int RecipeProductAmount { get; set; }
        
        public Guid ProductId { get; set; }

        public Product? Product { get; set; }
        
        public Guid RecipeId { get; set; }
        public Domain.App.Recipe? Recipe { get; set; }
    }
}