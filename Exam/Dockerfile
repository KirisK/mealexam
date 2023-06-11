FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY *.props .
COPY *.sln .

COPY BLL.App/*.csproj ./BLL.App/
COPY BLL.DTO.App/*.csproj ./BLL.DTO.App/
COPY BLL.Base/*.csproj ./BLL.Base/
COPY BLL.Contracts.App/*.csproj ./BLL.Contracts.App/
COPY BLL.Contracts.Base/*.csproj ./BLL.Contracts.Base/

COPY DAL.Contracts.App/*.csproj ./DAL.Contracts.App/
COPY DAL.Contracts.Base/*.csproj ./DAL.Contracts.Base/
COPY DAL.EF.App/*.csproj ./DAL.EF.App/
COPY DAL.EF.Base/*.csproj ./DAL.EF.Base/

COPY Domain.Contracts.Base/*.csproj ./Domain.Contracts.Base/
COPY Domain.App/*.csproj ./Domain.App/
COPY Domain.Base/*.csproj ./Domain.Base/

COPY Shared.App/*.csproj ./Shared.App/
COPY Shared.Base/*.csproj ./Shared.Base/
COPY Shared.Contracts.Base/*.csproj ./Shared.Contracts.Base/

COPY Public.DTO.v1/*.csproj ./Public.DTO.v1/
COPY Mapper.Profiles.App/*.csproj ./Mapper.Profiles.App/
COPY Tests/*.csproj ./Tests/

COPY WebApp/*.csproj ./WebApp/

RUN dotnet restore

# copy everything else and build app
COPY BLL.App/. ./BLL.App/
COPY BLL.DTO.App/. ./BLL.DTO.App/
COPY BLL.Base/. ./BLL.Base/
COPY BLL.Contracts.App/. ./BLL.Contracts.App/
COPY BLL.Contracts.Base/. ./BLL.Contracts.Base/

COPY DAL.Contracts.App/. ./DAL.Contracts.App/
COPY DAL.Contracts.Base/. ./DAL.Contracts.Base/
COPY DAL.EF.App/. ./DAL.EF.App/
COPY DAL.EF.Base/. ./DAL.EF.Base/

COPY Domain.Contracts.Base/. ./Domain.Contracts.Base/
COPY Domain.App/. ./Domain.App/
COPY Domain.Base/. ./Domain.Base/

COPY Shared.App/. ./Shared.App/
COPY Shared.Base/. ./Shared.Base/
COPY Shared.Contracts.Base/. ./Shared.Contracts.Base/

COPY Public.DTO.v1/. ./Public.DTO.v1/
COPY Mapper.Profiles.App/. ./Mapper.Profiles.App/
COPY Tests/. ./Tests/

COPY WebApp/. ./WebApp/


WORKDIR /src/WebApp
RUN dotnet publish -c Release -o out




FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
EXPOSE 80

COPY --from=build /src/WebApp/out ./
ENTRYPOINT ["dotnet", "WebApp.dll"]