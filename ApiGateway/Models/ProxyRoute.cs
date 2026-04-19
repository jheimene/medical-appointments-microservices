
namespace Security.ApiGateway.Yarp.Models
{
    public class ProxyRoute
    {
        public int Id { get; set; }
        public string RouteId { get; set; } = default!;
        public string ClusterId { get; set; } = default!;
        public int? Order { get; set; }
        public ICollection<Method> Methods { get; set; } = [];
        public string PathPattern { get; set; } = default!;
        public string? RemovePrefix { get; set; }
        public string? AuthorizationPolicy   { get; set; }
        public string? RateLimitPolicy { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public enum Method
    {
        GET,
        POST,
        PUT,
        DELETE,
        PATCH,
        HEAD,
        OPTIONS,
        TRACE
    }
}
