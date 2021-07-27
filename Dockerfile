FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["JetbrainsFeaturedImageGenerator.csproj", "./"]
RUN dotnet restore "JetbrainsFeaturedImageGenerator.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "JetbrainsFeaturedImageGenerator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JetbrainsFeaturedImageGenerator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JetbrainsFeaturedImageGenerator.dll"]