FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["AptBacs.Ui.Web/AptBacs.Ui.Web.csproj", "AptBacs.Ui.Web/"]
RUN dotnet restore "AptBacs.Ui.Web/AptBacs.Ui.Web.csproj"
COPY . .
WORKDIR "/src/AptBacs.Ui.Web"
RUN dotnet build "AptBacs.Ui.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AptBacs.Ui.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AptBacs.Ui.Web.dll"]