using Security.ApiGateway.Yarp.Abstractions;
using Security.ApiGateway.Yarp.Contracts;

namespace Security.ApiGateway.Yarp.Configuration
{
    public sealed class InMemoryGatewayConfigStore(GatewayConfigSnapshot initial) : IGatewayConfigStore
    {
        private GatewayConfigSnapshot _snapshot = initial;


        public Task<GatewayConfigSnapshot> GetAsync(CancellationToken cancellationToken) => Task.FromResult(_snapshot);

        public Task SaveAsync(GatewayConfigSnapshot snapshot, CancellationToken cancellationToken)
        {
            _snapshot = snapshot;
            return Task.CompletedTask;
        }
    }
}
