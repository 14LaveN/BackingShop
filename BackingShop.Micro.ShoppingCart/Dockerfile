FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BackingShop.Micro.ShoppingCart/BackingShop.Micro.ShoppingCart.csproj", "BackingShop.Micro.ShoppingCart/"]
RUN dotnet restore "BackingShop.Micro.ShoppingCart/BackingShop.Micro.ShoppingCart.csproj"
COPY . .
WORKDIR "/src/BackingShop.Micro.ShoppingCart"
RUN dotnet build "BackingShop.Micro.ShoppingCart.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackingShop.Micro.ShoppingCart.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackingShop.Micro.ShoppingCart.dll"]
