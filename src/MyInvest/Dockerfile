FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["packages.lock.json", "MyInvest/"]
COPY ["MyInvest.csproj", "MyInvest/"]
RUN dotnet restore "MyInvest/MyInvest.csproj" --locked-mode

COPY . MyInvest/
WORKDIR "/src/MyInvest"
RUN dotnet build "MyInvest.csproj" --no-restore --configuration Release --output /app/build

FROM build AS publish
RUN dotnet publish "MyInvest.csproj" --configuration Release --output /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyInvest.dll"]
