using System.ComponentModel.DataAnnotations;
using Domain.App;
using Domain.App.Identity;
using Domain.Base;
using Shared.Contracts.Base.Entity;

namespace Public.DTO.v1
{
    public class Recipe : EntityId
    {
        [MaxLength(64)]
        public string RecipeName { get; set; }  = default!;

        public int RecipeTimeNeeded { get; set; }

        public ICollection<RecipeProduct>?  RecipeProducts { get; set; }
        
        public string Description { get; set; } = default!;

        public Boolean IsPublic { get; set; }

        public int Servings { get; set; }
        
        
        public Guid AppUserId { get; set; }

        
    }
}