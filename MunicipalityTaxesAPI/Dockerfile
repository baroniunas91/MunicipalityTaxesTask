FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
EXPOSE 8080

# Copy csproj and restore as distinct layers
COPY . ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish ./MunicipalityTaxesAPI.csproj -r linux-x64 --self-contained true -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

RUN apt-get update && apt-get install -y libgssapi-krb5-2

ARG env_name
ENV ENVIRONMENT=$env_name

WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["/bin/sh", "-c", "dotnet MunicipalityTaxesAPI.dll"]