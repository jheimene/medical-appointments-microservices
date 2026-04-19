using Microsoft.AspNetCore.Mvc;
using Security.ApiGateway.Yarp.Abstractions;
using Security.ApiGateway.Yarp.Contracts;
using Security.ApiGateway.Yarp.Contracts.Requests;

namespace Security.ApiGateway.Yarp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClustersController : ControllerBase
    {
        private readonly IGatewayConfigService _service;

        public ClustersController(IGatewayConfigService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetClusters(CancellationToken cancellationToken)
        {
            var clusters = await _service.GetClusters(cancellationToken);
            return Ok(clusters);
        }

        [HttpGet("{clusterId}")]
        public async Task<IActionResult> GetCluster(string clusterId, CancellationToken cancellationToken)
        {
            var cluster = await _service.GetCluster(clusterId, cancellationToken);
            if (cluster is null) return NotFound();
            return Ok(cluster);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoute([FromBody] UpsertClusterRequest request, CancellationToken cancellationToken)
        {
            if (request is null) return BadRequest();

            var cluster = new GatewayClusterDefinition
            (
                ClusterId: request.ClusterId,
                LoadBalancingPolicy: request.LoadBalancingPolicy,
                HealthCheckPath: request.HealthCheckPath,
                Destinations: [.. request.Destinations.Select(s => new GatewayDestinationDefinition(s.DestinationId, s.Address))]
            );
            await _service.CreateClusterAsync(cluster, cancellationToken);
            return CreatedAtAction(nameof(GetCluster), new { clusterId = request.ClusterId }, new { clusterId = request.ClusterId });
        }

        [HttpPut("{clusterId}")]
        public async Task<IActionResult> UpdateRoute(string clusterId, [FromBody] UpsertClusterRequest request, CancellationToken cancellationToken)
        {
            if (request is null) return BadRequest();
            if (request.ClusterId != clusterId) return BadRequest();

            var cluster = new GatewayClusterDefinition
            (
                ClusterId: request.ClusterId,
                LoadBalancingPolicy: request.LoadBalancingPolicy,
                HealthCheckPath: request.HealthCheckPath,
                Destinations: [.. request.Destinations.Select(s => new GatewayDestinationDefinition(s.DestinationId, s.Address))]
            );
            await _service.UpdateClusterAsync(cluster, cancellationToken);
            return NoContent();
        }
    }
}
