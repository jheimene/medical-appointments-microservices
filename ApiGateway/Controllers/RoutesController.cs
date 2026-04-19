
using Microsoft.AspNetCore.Mvc;
using Security.ApiGateway.Yarp.Abstractions;
using Security.ApiGateway.Yarp.Contracts;
using Security.ApiGateway.Yarp.Contracts.Requests;
using Security.ApiGateway.Yarp.Models;


namespace Security.ApiGateway.Yarp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IGatewayConfigService _service;

        public RoutesController(IGatewayConfigService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoutes(CancellationToken cancellationToken) => Ok(await _service.GetRoutes(cancellationToken));

        [HttpGet("{routeId}")]
        public async Task<IActionResult> GetRoute(string routeId, CancellationToken cancellationToken)
        {
            var route = await _service.GetRoute(routeId, cancellationToken);
            return Ok(route);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoute([FromBody] UpsertRouteRequest request, CancellationToken cancellationToken)
        {
            if (request is null) return BadRequest();

            var route = new GatewayRouteDefinition
            (
                RouteId: request.RouteId,
                ClusterId: request.ClusterId,
                Order: request.OrderIndex,
                Methods: [.. request.Methods.Select(m => (Method)Enum.Parse(typeof(Method), m))],
                Path: request.PathPattern,
                RemovePrefix: null,
                AuthorizationPolicy: null
            );
            //_dbContext.Routes.Add(model);
            //_dbContext.SaveChanges();
            //_yarpProvider.Reload();
            await _service.CreateRouteAsync(route, cancellationToken);
            return CreatedAtAction(nameof(GetRoute), new { routeId = request.RouteId }, new { routeId = request.RouteId });
        }

        [HttpPut("{routeId}")]
        public async Task<IActionResult> UpdateRoute(string routeId, [FromBody] UpsertRouteRequest request, CancellationToken cancellationToken)
        {
            if (request is null) return BadRequest();
            if (request.RouteId != routeId) return BadRequest();

            var route = new GatewayRouteDefinition
            (
                RouteId: request.RouteId,
                ClusterId: request.ClusterId,
                Order: request.OrderIndex,
                Methods: [.. request.Methods.Select(m => (Method)Enum.Parse(typeof(Method), m))],
                Path: request.PathPattern,
                RemovePrefix: null,
                AuthorizationPolicy: null
            );

            //var route = _dbContext.Routes.Find(id);
            //if (route is null) return NotFound();

            //route.RouteId = model.RouteId;
            //route.ClusterId = model.ClusterId;
            //route.Path = model.Path;
            //route.Destination = model.Destination;

            //_dbContext.SaveChanges();
            //_yarpProvider.Reload();

            await _service.UpdateRouteAsync(route, cancellationToken);
            return NoContent();
        }

        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    var route = _dbContext.Routes.Find(id);
        //    if (route is null) return NotFound();

        //    _dbContext.Routes.Remove(route);
        //    _dbContext.SaveChanges();
        //    _yarpProvider.Reload();
        //    return NoContent();
        //}

        //[HttpPost("reload")]
        //public IActionResult Reload()
        //{
        //    _yarpProvider.Reload();
        //    return Ok("Rutas recargadas dinámicamente.");
        //}
    }
}
