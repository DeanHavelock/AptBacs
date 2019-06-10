FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["AptBacs.PaymentProcessor.Application.Api/AptBacs.PaymentProcessor.Application.Api.csproj", "AptBacs.PaymentProcessor.Application.Api/"]
RUN dotnet restore "AptBacs.PaymentProcessor.Application.Api/AptBacs.PaymentProcessor.Application.Api.csproj"
COPY . .
WORKDIR "/src/AptBacs.PaymentProcessor.Application.Api"
RUN dotnet build "AptBacs.PaymentProcessor.Application.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AptBacs.PaymentProcessor.Application.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AptBacs.PaymentProcessor.Application.Api.dll"]