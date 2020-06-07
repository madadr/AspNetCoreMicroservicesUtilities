cd ../Services

if ($args.count -eq 0)
{
    $args = @('Tickets.Api', 'MailNotifications.Api', 'Users.Api', 'ServiceRegistry', 'Router')
}

for ($i = 0; $i -lt $args.count; $i++)
{
    $service = "$($args[$i])"
    echo "=== === === === === === === === === === === === ==="
    echo "Building docker image for service: $service"
    echo "=== === === === === === === === === === === === ==="

    $serviceLowerCase = $service.ToLower()
    docker image rm -f sd."$serviceLowerCase"
    docker build --no-cache -f ./"$service"/Dockerfile -t sd."$serviceLowerCase" ./"$service"
}

cd ../Deploy