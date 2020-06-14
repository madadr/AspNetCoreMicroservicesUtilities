FROM mcr.microsoft.com/dotnet/core/aspnet
WORKDIR /app
COPY ./bin/Docker .
EXPOSE 80/tcp
EXPOSE 81/tcp
ENV ASPNETCORE_ENVIRONMENT Release
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
