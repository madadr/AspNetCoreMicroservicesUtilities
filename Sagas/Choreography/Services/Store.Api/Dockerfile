FROM mcr.microsoft.com/dotnet/core/aspnet
WORKDIR /app
COPY ./bin/Docker .
EXPOSE 80/tcp
ENV ASPNETCORE_ENVIRONMENT Release
ENTRYPOINT ["dotnet", "Store.Api.dll"]
