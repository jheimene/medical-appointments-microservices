using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Contracts.Requests;
using ProductService.Application.Common.Models;
using ProductService.Application.Products.Commands.UploadProductImage;
using ProductService.Application.Products.Queries.GetProductImageById;
using ProductService.Application.Products.Queries.GetProductImages;
using Error = ErrorOr.Error;

namespace ProductService.Api.Controllers
{
    [Route("api/products/{productId:guid}/images")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        [Consumes("multipart/form-data")]
        //[RequestFormLimits(MultipartBodyLengthLimit = 10_000_000)]
        //[RequestSizeLimit(10_000_000)]
        public async Task<IActionResult> Upload(
            [FromRoute] Guid productId,
            [FromForm] UploadProductImageRequest request,
            CancellationToken cancellationToken
            )
        {
            await using var stream = request.File.OpenReadStream();

            var command = new UploadProductImageCommand(
                productId,
                new FileUploadData(
                    request.File.FileName,
                    request.File.ContentType,
                    request.File.Length,
                    stream
                    ),
                request.IsMain,
                request.SortOrder
                );

            var result = await _mediator.Send( command, cancellationToken );
            return result.Match(
                response => CreatedAtAction(nameof(GetById), new { productId = response.ProductId, imageId = response.ProductImageId }, response),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromRoute] Guid productId,
            CancellationToken cancellationToken
            )
        {
            var result = await _mediator.Send(new GetProductImagesByProductIdQuery(productId), cancellationToken);

            return result.Match(
                productImageList => Ok(productImageList),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );
        }

        [HttpGet("{imageId:guid}")]
        public async Task<IActionResult> GetById(
           [FromRoute] Guid productId,
           [FromRoute] Guid imageId,
           CancellationToken cancellationToken
           )
        {
            var result = await _mediator.Send(new GetProductImageByIdQuery(productId, imageId), cancellationToken);

            return result.Match(
                productImage => Ok(productImage),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );
        }


        //    [HttpPost]
        //    [Consumes("multipart/form-data")]
        //    [RequestFormLimits(MultipartBodyLengthLimit = 10_000_000)]
        //    [RequestSizeLimit(10_000_000)]
        //    [ProducesResponseType(typeof(ProductImageResponse), StatusCodes.Status201Created)]
        //    [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //    [ProducesResponseType(StatusCodes.Status404NotFound)]
        //    public async Task<ActionResult<ProductImageResponse>> Upload(
        //    [FromRoute] Guid productId,
        //    [FromForm] UploadProductImageFormRequest request,
        //    [FromServices] ProductImageService service,
        //    CancellationToken cancellationToken)
        //    {
        //        var result = await service.UploadAsync(productId, request, cancellationToken);

        //        return CreatedAtAction(
        //            nameof(GetById),
        //            new { productId, imageId = result.Id },
        //            result);
        //    }

        //    [HttpGet]
        //    [ProducesResponseType(typeof(IEnumerable<ProductImageResponse>), StatusCodes.Status200OK)]
        //    public async Task<ActionResult<IReadOnlyList<ProductImageResponse>>> GetAll(
        //    [FromRoute] Guid productId,
        //    [FromServices] ProductImageService service,
        //    CancellationToken cancellationToken)
        //    {
        //        var result = await service.GetByProductAsync(productId, cancellationToken);
        //        return Ok(result);
        //    }

        //    [HttpGet("{imageId:guid}")]
        //    [ProducesResponseType(typeof(ProductImageResponse), StatusCodes.Status200OK)]
        //    [ProducesResponseType(StatusCodes.Status404NotFound)]
        //    public async Task<ActionResult<ProductImageResponse>> GetById(
        //    [FromRoute] Guid productId,
        //    [FromRoute] Guid imageId,
        //    [FromServices] ProductImageService service,
        //    CancellationToken cancellationToken)
        //    {
        //        var result = await service.GetByIdAsync(productId, imageId, cancellationToken);

        //        if (result is null)
        //            return NotFound();

        //        return Ok(result);
        //    }

        //    [HttpPatch("{imageId:guid}/main")]
        //    [ProducesResponseType(typeof(ProductImageResponse), StatusCodes.Status200OK)]
        //    [ProducesResponseType(StatusCodes.Status404NotFound)]
        //    public async Task<ActionResult<ProductImageResponse>> SetMain(
        //[FromRoute] Guid productId,
        //[FromRoute] Guid imageId,
        //[FromServices] ProductImageService service,
        //CancellationToken cancellationToken)
        //    {
        //        var result = await service.SetMainAsync(productId, imageId, cancellationToken);
        //        return Ok(result);
        //    }

        //    [HttpDelete("{imageId:guid}")]
        //    [ProducesResponseType(StatusCodes.Status204NoContent)]
        //    [ProducesResponseType(StatusCodes.Status404NotFound)]
        //    public async Task<IActionResult> Delete(
        //        [FromRoute] Guid productId,
        //        [FromRoute] Guid imageId,
        //        [FromServices] ProductImageService service,
        //        CancellationToken cancellationToken)
        //    {
        //        await service.DeleteAsync(productId, imageId, cancellationToken);
        //        return NoContent();
        //    }

    }
}
