using Security.ApiGateway.Yarp.Abstractions;
using Security.ApiGateway.Yarp.Contracts;
using System.Text.Json;

namespace Security.ApiGateway.Yarp.Configuration
{
    public sealed class FileGatewayConfigStore(IWebHostEnvironment env) : IGatewayConfigStore
    {
        private readonly string _filePath = Path.Combine(env.ContentRootPath, "yarp.json");

        public async Task<GatewayConfigSnapshot> GetAsync(CancellationToken cancellationToken)
        {
            if (!File.Exists(_filePath))
                return new GatewayConfigSnapshot([], []);

            var json = await File.ReadAllTextAsync(_filePath, cancellationToken);
            var root = JsonSerializer.Deserialize<GatewayFileRoot>(json)!;
            return root.ToSnapshot();
        }

        public async Task SaveAsync(GatewayConfigSnapshot snapshot, CancellationToken cancellationToken)
        {
            var root = GatewayFileRoot.FromSnapshot(snapshot);
            var json = JsonSerializer.Serialize(root, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json, cancellationToken);
        }
    }
}
