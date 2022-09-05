FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["lab3.csproj", "./"]
RUN dotnet restore "lab3.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "lab3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "lab3.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "lab3.dll"]
