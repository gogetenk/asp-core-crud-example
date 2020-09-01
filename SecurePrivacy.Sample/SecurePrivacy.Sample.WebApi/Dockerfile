#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
ENV ASPNETCORE_URLS=http://+:5000
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SecurePrivacy.Sample.WebApi/WebApi.csproj", "SecurePrivacy.Sample.WebApi/"]
COPY ["SecurePrivacy.Sample.Dto/Dto.csproj", "SecurePrivacy.Sample.Dto/"]
RUN dotnet restore "SecurePrivacy.Sample.WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/SecurePrivacy.Sample.WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SecurePrivacy.Sample.WebApi.dll"]