using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.App.Identity;
using Domain.Base;

namespace Domain.App
{
    public class Recipe : DomainEntityMetaId
    {
        [MaxLength(64)]
        public string RecipeName { get; set; }  = default!;

        public int RecipeTimeNeeded { get; set; }

        public ICollection<RecipeProduct>?  RecipeProducts { get; set; }
        
        public string Description { get; set; } = default!;

        public Boolean IsPublic { get; set; }

        public int Servings { get; set; }
        
        [ForeignKey(nameof(AppUser))]
        public Guid AppUserId { get; set; }

        public AppUser? AppUser { get; set; }

        
    }
}