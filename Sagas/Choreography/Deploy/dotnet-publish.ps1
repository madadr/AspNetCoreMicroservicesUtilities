cd ../Services

if ($args.count -eq 0)
{
    $args = @('Clients', 'Orders', 'Payments', 'Store')
}

for ($i = 0; $i -lt $args.count; $i++)
{
    $service = "$($args[$i])"
    echo "=== === === === === === === === === === === === ==="
    echo "Building project for service: $service"
    echo "=== === === === === === === === === === === === ==="
    dotnet clean ./"$service".Api -c Release -o ./"$service".Api/bin/Docker
    dotnet publish --force ./"$service".Api -c Release -o ./"$service".Api/bin/Docker
} 

cd ../Deploy
