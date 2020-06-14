namespace ApiGateway
{
    public class RouteOption
    {
        public const string SectionName = "Routes";
        
        public string Method { get; set; }
        public string OriginRoute { get; set; }
        public string DestinationRoute { get; set; }
        public string DestinationHost { get; set; }
    }
}