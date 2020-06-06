.\dotnet-publish.ps1
.\docker-build.ps1
docker stop $(docker ps -a -q)
docker rm $(docker ps -a -q)
docker-compose rm -f
docker-compose build --no-cache --force-rm
docker-compose up