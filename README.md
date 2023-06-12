# icd0021-22-23-s-exam


### create migration
dotnet ef migrations add Initial --project DAL.EF.App --startup-project WebApp --context ApplicationDbContext

### apply migration
dotnet ef database update --project DAL.EF.App --startup-project WebApp --context ApplicationDbContext

### remove migration

dotnet ef migrations remove --project DAL.EF.App --startup-project WebApp --context ApplicationDbContext

### drop db

dotnet ef database drop --project DAL.EF.App --startup-project WebApp


## MVC
### inside WebApp

dotnet aspnet-codegenerator controller -m Domain.App.Product -name ProductController -outDir Controllers -dc ApplicationDbContext  -udl --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -m Domain.App.Recipe -name RecipeController -outDir Controllers -dc ApplicationDbContext  -udl --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -m Domain.App.UserProduct -name UserProductController -outDir Controllers -dc ApplicationDbContext  -udl --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -m Domain.App.RecipeProduct -name RecipeProductController -outDir Controllers -dc ApplicationDbContext  -udl --referenceScriptLibraries -f


## API
### inside WebApp
dotnet aspnet-codegenerator controller -m Domain.App.Product -name ProductsController -outDir ApiControllers -api -dc ApplicationDbContext  -udl -f
dotnet aspnet-codegenerator controller -m Domain.App.Recipe -name RecipeController -outDir ApiControllers -api -dc ApplicationDbContext  -udl -f
dotnet aspnet-codegenerator controller -m Domain.App.UserProduct -name UserProductController -outDir ApiControllers -api -dc ApplicationDbContext  -udl -f
dotnet aspnet-codegenerator controller -m Domain.App.RecipeProduct -name RecipeProductController -outDir ApiControllers -api -dc ApplicationDbContext  -udl -f



## Identity
### inside WebApp
dotnet aspnet-codegenerator identity -dc DAL.EF.App.ApplicationDbContext --userClass AppUser -f