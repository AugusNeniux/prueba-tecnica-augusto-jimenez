# Dockerfile básico para la API
# ⚠️ Este Dockerfile funciona pero tiene áreas de mejora (ver README sección Docker)

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "CleanArchitecture.PracticalTest.API.dll"]