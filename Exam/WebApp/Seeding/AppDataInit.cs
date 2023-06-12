using DAL.EF.App;
using Domain.App.Identity;
using Domain.Contracts.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Public.DTO.v1;
using Product = Domain.App.Product;
using Recipe = Domain.App.Recipe;
using RecipeProduct = Domain.App.RecipeProduct;

#pragma warning disable 1591

namespace WebApp.Seeding;

/// <summary>
/// Class for initial data seeding
/// </summary>
public class AppDataInit
{
    private static Guid userId;
    private static Guid userRegularId;
    public static void MigrateDatabase(ApplicationDbContext context)
    {
        context.Database.Migrate();
    }

    public static void DropDatabase(ApplicationDbContext context)
    {
        context.Database.EnsureDeleted();
    }
    
    private readonly IApplicationBuilder _app;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="app"></param>
    /// <param name="configuration"></param>
    /// <param name="environment"></param>
    public AppDataInit(IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment environment)
    {
        _app = app;
        _configuration = configuration;
        _environment = environment;
    }

    /// <summary>
    /// Run configured database operations  
    /// </summary>
    public void Run()
    {
        using var serviceScope = _app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();
        using var ctx = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

        if (ctx != null)
        {
            if (_configuration.GetValue<bool>("AppData:DropDatabase"))
            {
                Console.Write("Drop database");
                ctx.Database.EnsureDeleted();
                Console.WriteLine(" - done");
            }

            if (_configuration.GetValue<bool>("AppData:Migrate"))
            {
                Console.Write("Migrate database");
                if (ctx.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                {
                    ctx.Database.Migrate();
                }

                Console.WriteLine(" - done");
            }

            using var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();

            if (_configuration.GetValue<bool>("AppData:SeedIdentity"))
            {
                using var roleManager = serviceScope.ServiceProvider.GetService<
                    RoleManager<AppRole>
                >();

                if (userManager != null && roleManager != null)
                {
                    SeedIdentity(userManager, roleManager,ctx).Wait();
                }
                else
                {
                    Console.Write(
                        $"No user manager {(userManager == null ? "null" : "ok")} or "
                            + $"role manager {(roleManager == null ? "null" : "ok")}!"
                    );
                }
            }

            if (_configuration.GetValue<bool>("AppData:SeedData"))
            {
                Console.Write("Seed database test cleanings and stuff");
                SeedTestData(ctx);
                Console.WriteLine(" - done");
            }
        }
    }


    public static async Task SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,ApplicationDbContext context)
    {
        IdentityResult? result;
            
        var roleNames = new[] {"Admin", "User"};
        foreach (var roleName in roleNames)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                role = new AppRole { Name = roleName };
                    
                result = await roleManager.CreateAsync(role);
                    
                if (!result.Succeeded) HandleBadResult(result, $"create role {roleName}");
            }
        }

        // ADMIN
        var user = new AppUser();
        user.Email = "admin@mealplan.com";
        user.FirstName = "Admin";
        user.LastName = "Adminович";
        user.UserName = user.Email;

        result = await userManager.CreateAsync(user, "Pass123.");
        if (!result.Succeeded) HandleBadResult(result, "create user");
            
        result = await userManager.AddToRoleAsync(user, "Admin");
        if (!result.Succeeded) HandleBadResult(result, "add user to role");

        userId = user.Id;
        
        //User
        var userRegular = new AppUser();
        userRegular.Email = "regular@meal.com";
        userRegular.FirstName = "Anne";
        userRegular.LastName = "Wells";
        userRegular.UserName = userRegular.Email;

        result = await userManager.CreateAsync(userRegular, "Pass123.");
        if (!result.Succeeded) HandleBadResult(result, "create user");
            
        result = await userManager.AddToRoleAsync(userRegular, "User");
        if (!result.Succeeded) HandleBadResult(result, "add user to role");

        userRegularId = userRegular.Id;
        
        await context.SaveChangesAsync();
    }

    private static void HandleBadResult(IdentityResult result, string context)
    {
        foreach (var identityError in result.Errors)
        {
            Console.WriteLine($"Can't {context}! Error: {identityError.Description}");
        }
    }
    

    private void SeedTestData(ApplicationDbContext context)
    {
        // Product
        var product = NewWithMeta<Product>();
        product.ProductName = "Apple";

        context.Products.Add(product);
        
        // Product1
        var product1 = NewWithMeta<Product>();
        product1.ProductName = "Potato";

        context.Products.Add(product1);
        
        // Product2
        var product2 = NewWithMeta<Product>();
        product2.ProductName = "Milk";

        context.Products.Add(product2);
        
        /*// Recipe owner
        var person = context.Users.Include(p => p.User)
            .FirstOrDefault(p => p.User!.Email!.Equals("manager1@cleaningservice.com"));
        if (person == null)
        {
            resproduct = NewWithMeta<RecipeProduct>();
            resproduct.User = context.Users.FirstOrDefault(u => u.Email!.Equals("manager1@cleaningservice.com"));
            context.Persons.Add(person);
        }


        var pic = NewWithMeta<Recipe>();
        pic.AppUser = person;
        pic.RecipeProducts = ;

        context.Recipes.Add(pic);
        
        */
        
        //Recipe 1
        var recipe = NewWithMeta<Recipe>();
        recipe.RecipeName = "Mashed Potato";
        recipe.Description = "Boil potatoes, add milk and butter";
        recipe.RecipeTimeNeeded = 30;
        recipe.AppUserId = userId;


        var recipeProduct = NewWithMeta<RecipeProduct>();
        recipeProduct.Product = product1;
        recipeProduct.RequiredAmount = 3;
        recipeProduct.Units = "pic";
        recipe.RecipeProducts = new List<RecipeProduct> { recipeProduct };
        
        context.Recipes.Add(recipe);

        //Recipe 2
        recipe = NewWithMeta<Recipe>();
        recipe.RecipeName = "Pizza";
        recipe.Description = "Kill youself";
        recipe.RecipeTimeNeeded = 25;
        recipe.AppUserId = userId;
        
        context.Recipes.Add(recipe);

        //Recipe 3
        recipe = NewWithMeta<Recipe>();
        recipe.RecipeName = "Cake";
        recipe.Description = "Boil potatoes, add milk and butter";
        recipe.RecipeTimeNeeded = 30;
        recipe.AppUserId = userRegularId;

        context.Recipes.Add(recipe);

        //Recipe 4
        recipe = NewWithMeta<Recipe>();
        recipe.RecipeName = "Salad";
        recipe.Description = "Kill youself";
        recipe.RecipeTimeNeeded = 25;
        recipe.AppUserId = userRegularId;
        
        context.Recipes.Add(recipe);
        context.SaveChanges();
        
    }

    private TEntity NewWithMeta<TEntity>() where TEntity : IDomainEntityMeta, new()
    {
        return new TEntity
        {
            CreatedBy = "SystemInit"
        };
    }
    
}