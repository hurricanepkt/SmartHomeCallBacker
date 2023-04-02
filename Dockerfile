FROM mcr.microsoft.com/dotnet/sdk:7.0.202 as build-env 
WORKDIR /app

COPY *.csproj ./ 

COPY . ./
RUN dotnet publish -c Release -o out 

FROM mcr.microsoft.com/dotnet/aspnet:7.0.202
WORKDIR /app 
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "CommandService.dll"] 