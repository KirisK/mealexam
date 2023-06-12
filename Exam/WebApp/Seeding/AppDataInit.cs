using DAL.EF.App;
using Domain.App.Identity;
using Domain.Contracts.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Public.DTO.v1;
using Product = Domain.App.Product;
using Recipe = Domain.App.Recipe;
using UserProduct = Domain.App.UserProduct;
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
                    SeedIdentity(userManager, roleManager, ctx).Wait();
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


    public static async Task SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
        ApplicationDbContext context)
    {
        IdentityResult? result;

        var roleNames = new[] { "Admin", "User" };
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
        userRegular.Email = "regular@mealplan.com";
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

        // Product3
        var product3 = NewWithMeta<Product>();
        product3.ProductName = "Tomato";

        context.Products.Add(product3);

        // Product4
        var product4 = NewWithMeta<Product>();
        product4.ProductName = "Cucumber";

        context.Products.Add(product4);

        // Product5
        var product5 = NewWithMeta<Product>();
        product5.ProductName = "Salt";

        context.Products.Add(product5);

        // Product6
        var product6 = NewWithMeta<Product>();
        product6.ProductName = "Water";

        context.Products.Add(product6);

        // Product7
        var product7 = NewWithMeta<Product>();
        product7.ProductName = "Bread";

        context.Products.Add(product7);

        // Product8
        var product8 = NewWithMeta<Product>();
        product8.ProductName = "Flour";

        context.Products.Add(product8);


        //Recipe 1
        var recipe = NewWithMeta<Recipe>();
        recipe.RecipeName = "Mashed Potato";
        recipe.Description = "Boil potatoes, add milk and butter";
        recipe.RecipeTimeNeeded = 30;
        recipe.Servings = 1;
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
        recipe.Servings = 1;
        recipe.AppUserId = userId;

        context.Recipes.Add(recipe);

        //Recipe 3
        recipe = NewWithMeta<Recipe>();
        recipe.RecipeName = "Cake";
        recipe.Description = "Mix all ingredients together, put into baking form and then put it to oven";
        recipe.RecipeTimeNeeded = 45;
        recipe.Servings = 4;
        recipe.AppUserId = userRegularId;

        recipeProduct = NewWithMeta<RecipeProduct>();
        recipeProduct.Product = product2;
        recipeProduct.RequiredAmount = 1;
        recipeProduct.Units = "l";
        recipe.RecipeProducts = new List<RecipeProduct> { recipeProduct };


        context.Recipes.Add(recipe);


        //Recipe 4
        recipe = NewWithMeta<Recipe>();
        recipe.RecipeName = "Salad";
        recipe.Description = "Chop all vegetables, put into the bowl, add salt and oli. Mix the salad.";
        recipe.RecipeTimeNeeded = 15;
        recipe.Servings = 2;
        recipe.AppUserId = userRegularId;

        context.Recipes.Add(recipe);

        /*//User products
        
        var uproduct = NewWithMeta<UserProduct>();
        uproduct.Product = product1;
        uproduct.AvailableAmount = 4;
        uproduct.Units = "pic";
        uproduct.AppUserId = userRegularId;

        context.UserProducts.Add(uproduct);*/


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