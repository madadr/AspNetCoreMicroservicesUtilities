cd ../Services

if ($args.count -eq 0)
{
    $args = @('Clients', 'Orders', 'Payments', 'Store')
}

for ($i = 0; $i -lt $args.count; $i++)
{
    $service = "$($args[$i])"
    echo "=== === === === === === === === === === === === ==="
    echo "Building docker image for service: $service"
    echo "=== === === === === === === === === === === === ==="

    $serviceLowerCase = $service.ToLower()
    docker image rm -f choreography."$serviceLowerCase".api
    docker build --no-cache -f ./"$service".Api/Dockerfile -t choreography."$serviceLowerCase".api ./"$service".Api
}

cd ../Deploy