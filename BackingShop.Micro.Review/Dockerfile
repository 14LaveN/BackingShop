FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BackingShop.Micro.Review/BackingShop.Micro.Review.csproj", "BackingShop.Micro.Review/"]
RUN dotnet restore "BackingShop.Micro.Review/BackingShop.Micro.Review.csproj"
COPY . .
WORKDIR "/src/BackingShop.Micro.Review"
RUN dotnet build "BackingShop.Micro.Review.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackingShop.Micro.Review.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackingShop.Micro.Review.dll"]
