FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine3.18 as build
WORKDIR /src
COPY VaultSample/*.csproj .
RUN dotnet restore
COPY VaultSample .
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine3.18 as runtime
WORKDIR /app
COPY --from=build /publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "VaultSample.dll"]