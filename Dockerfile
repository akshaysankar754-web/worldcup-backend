# Stage 1: Build the .NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["Backend.csproj", "./"]
RUN dotnet restore "./Backend.csproj"

# Copy the remaining source files and build
COPY . .
RUN dotnet publish "Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# The default ASP.NET Core 8.0 port is 8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Backend.dll"]
