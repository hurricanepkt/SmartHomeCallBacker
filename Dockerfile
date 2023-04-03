FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-env
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /publish
COPY --from=build-env /publish .
ENV ASPNETCORE_URLS=http://+:80
HEALTHCHECK CMD curl --fail http://localhost:80/health || exit
EXPOSE 80
ENTRYPOINT ["dotnet", "SmartHomeCallBacker.dll"]