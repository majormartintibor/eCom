# Use the .NET 9 SDK image to build the project 
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env WORKDIR /app 

# Copy the .csproj file and restore dependencies 
COPY *.csproj ./ RUN dotnet restore 

# Copy the rest of the project files and build COPY 
. ./ RUN dotnet publish -c Release -o out 

# Use the .NET runtime image for the final image 
FROM mcr.microsoft.com/dotnet/aspnet:9.0 WORKDIR /app COPY --from=build-env /app/out . ENTRYPOINT ["dotnet", "YourProjectName.dll"]

# Environment variables 
#ENV ASPNETCORE_ENVIRONMENT=Development 
#ENV API_BASE_URL=https://api.example.com 
#ENV AUTHORITY=https://identity.example.com 
#ENV CLIENT_ID=myclientid 
#ENV REDIRECT_URI=https://app.example.com/authentication/login-callback ENV 
#POST_LOGOUT_REDIRECT_URI=https://app.example.com/authentication/logout-callback 
#
#ENTRYPOINT ["dotnet", "eCom.UI.dll"]