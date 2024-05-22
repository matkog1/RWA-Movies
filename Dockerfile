# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
# Optionally set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Use the official ASP.NET Core SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["WebMVC/WebMVC.csproj", "WebMVC/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["BLayer/BLayer.csproj", "BLayer/"]
COPY ["WebCoreAPI/WebCoreAPI.csproj", "WebCoreAPI/"]
COPY . .

WORKDIR "/src/WebMVC"
RUN dotnet restore "WebMVC.csproj"
RUN dotnet build "WebMVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebMVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebMVC.dll"]

# Optional Health Check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl --fail http://localhost:80/health || exit 1
