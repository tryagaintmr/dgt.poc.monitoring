#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy your certificate to a location inside the container
COPY /dgt.poc.web/devdocker.crt /usr/local/share/ca-certificates/

# Update the CA certificates inside the container
RUN update-ca-certificates

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["dgt.poc.web/dgt.poc.web.csproj", "dgt.poc.web/"]
RUN dotnet restore "dgt.poc.web/dgt.poc.web.csproj"
COPY . .
WORKDIR "/src/dgt.poc.web"
RUN dotnet build "dgt.poc.web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dgt.poc.web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dgt.poc.web.dll"]