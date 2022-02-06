#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Issue-Service/src/Spirebyte.Services.Issues.API/Spirebyte.Services.Issues.API.csproj", "src/Issue-Service/src/Spirebyte.Services.Issues.API/"]
COPY ["src/Issue-Service/src/Spirebyte.Services.Issues.Infrastructure/Spirebyte.Services.Issues.Infrastructure.csproj", "src/Issue-Service/src/Spirebyte.Services.Issues.Infrastructure/"]
COPY ["src/Issue-Service/src/Spirebyte.Services.Issues.Application/Spirebyte.Services.Issues.Application.csproj", "src/Issue-Service/src/Spirebyte.Services.Issues.Application/"]
COPY ["src/Issue-Service/src/Spirebyte.Services.Issues.Core/Spirebyte.Services.Issues.Core.csproj", "src/Issue-Service/src/Spirebyte.Services.Issues.Core/"]
RUN dotnet restore "src/Issue-Service/src/Spirebyte.Services.Issues.API/Spirebyte.Services.Issues.API.csproj"
COPY . .
WORKDIR "/src/src/Issue-Service/src/Spirebyte.Services.Issues.API"
RUN dotnet build "Spirebyte.Services.Issues.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Spirebyte.Services.Issues.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Spirebyte.Services.Issues.API.dll"]
