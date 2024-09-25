FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

ENV APP_ENV=host

COPY --from=build-env /app/out .
COPY --from=build-env /app/.env .
RUN mkdir Storage Storage/Certs

EXPOSE 8080

ENTRYPOINT ["dotnet", "api.dll"]
