using Domain.Base;
using Shared.Contracts.Base.Entity;

namespace Public.DTO.v1
{
    public class RecipeProduct : EntityId
    {

        public int RequiredAmount { get; set; }
        public string Units { get; set; } = default!;
        
        public Guid ProductId { get; set; }

        public Product? Product { get; set; }
        
        public Guid RecipeId { get; set; }
    }
}