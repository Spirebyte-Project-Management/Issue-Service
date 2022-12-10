FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

ARG Nuget_CustomFeedUserName
ARG Nuget_CustomFeedPassword

WORKDIR /src
COPY ["src/Spirebyte.Services.Issues.API/Spirebyte.Services.Issues.API.csproj", "src/Spirebyte.Services.Issues.API/"]
COPY ["src/Spirebyte.Services.Issues.Infrastructure/Spirebyte.Services.Issues.Infrastructure.csproj", "src/Spirebyte.Services.Issues.Infrastructure/"]
COPY ["src/Spirebyte.Services.Issues.Application/Spirebyte.Services.Issues.Application.csproj", "src/Spirebyte.Services.Issues.Application/"]
COPY ["src/Spirebyte.Services.Issues.Core/Spirebyte.Services.Issues.Core.csproj", "src/Spirebyte.Services.Issues.Core/"]
RUN dotnet nuget add source --username ${Nuget_CustomFeedUserName} --password ${Nuget_CustomFeedPassword} --store-password-in-clear-text --name github "https://nuget.pkg.github.com/Spirebyte-Project-Management/index.json"
RUN dotnet restore "src/Spirebyte.Services.Issues.API/Spirebyte.Services.Issues.API.csproj"
COPY . .
WORKDIR "/src/src/Spirebyte.Services.Issues.API"
RUN dotnet build "Spirebyte.Services.Issues.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Spirebyte.Services.Issues.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT Docker
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED true
ENTRYPOINT ["dotnet", "Spirebyte.Services.Issues.API.dll"]
