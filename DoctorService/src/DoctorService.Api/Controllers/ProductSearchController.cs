using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Abstractions.Queries;
using ProductService.Application.Products.Queries.SearchProducts;

namespace ProductService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSearchController : ControllerBase
    {

        private readonly ILogger<ProductSearchController> _logger;
        private readonly IMediator _mediator;
        public ProductSearchController(ILogger<ProductSearchController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }


        [HttpGet("search")]
        [ProducesResponseType(typeof(PagedResult<ProductSearchItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchProducts(
            [FromQuery] string? q,
            [FromQuery] string? productTypeId,
            [FromQuery] string? brandId,
            [FromQuery] string? model,
            [FromQuery] int take = 50,
            [FromQuery] DateTime? lastStart = null,
            [FromQuery] Guid? lastId = null,
            //[FromQuery] int page = 1,
            //[FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default
        )
        {

            _logger.LogInformation("Searching for products with query: {Query}", q);
            var query = new SearchProductsQuery(
                Text: q,
                ProductTypeId: productTypeId,
                BrandId: brandId,
                Model: model,
                Take: take,
                LastStart: lastStart,
                LastId: lastId
                //Page: page,
                //PageSize: pageSize
            );

            var result = await _mediator.Send( query );

            return result.Match(
                products => Ok(products),
                errors => Problem(statusCode: StatusCodes.Status400BadRequest, detail: string.Join(", ", errors.Select(e => e.Description)))
            );
        }

    }
}
