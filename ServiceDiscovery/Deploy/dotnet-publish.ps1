cd ../Services

if ($args.count -eq 0)
{
    $args = @('Tickets.Api', 'MailNotifications.Api', 'Users.Api', 'ServiceRegistry', 'Router')
}

for ($i = 0; $i -lt $args.count; $i++)
{
    $service = "$($args[$i])"
    echo "=== === === === === === === === === === === === ==="
    echo "Building project for: $service"
    echo "=== === === === === === === === === === === === ==="
    dotnet clean ./"$service" -c Release -o ./"$service"/bin/Docker
    dotnet publish --force ./"$service" -c Release -o ./"$service"/bin/Docker
} 

cd ../Deploy
