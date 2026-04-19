using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Products.Commands.ActivateProduct;
using ProductService.Application.Products.Commands.AddProductTags;
using ProductService.Application.Products.Commands.CreateCustomer;
using ProductService.Application.Products.Commands.PatchProduct;
using ProductService.Application.Products.Commands.RemoveProductTags;
using ProductService.Application.Products.Commands.UpdateProductBasics;
using ProductService.Application.Products.Dtos;
using ProductService.Application.Products.Queries.GetByIdProduct;


namespace ProductService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return result.Match(
                productId => CreatedAtAction(nameof(GetById), new { productId }, new { productId }),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );
        }

        [HttpGet("{productId:guid}", Name = "Product_GetById")]
        public async Task<IActionResult> GetById(Guid productId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetByIdCategoryQuery(productId), cancellationToken);

            return result.Match(
                product => Ok(product),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );
        }

        [HttpPut("{productId:guid}")]
        public async Task<IActionResult> UpdateBasics(Guid productId, [FromBody] UpdateProductBasicsCommand command, CancellationToken cancellationToken)
        {
            if (productId != command.ProductId) return BadRequest();
            var result = await _mediator.Send(command, cancellationToken);
            return result.Match(
                _ => NoContent(),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );
        }

        [HttpPatch("{productId:guid}")]
        public async Task<IActionResult> Patch(Guid ProductId, [FromBody] JsonPatchDocument<ProductPatchDto> patchDoc, CancellationToken cancellation)
        {
            if (patchDoc == null) return BadRequest("El document patch es requerido.");

            if (!ModelState.IsValid) return BadRequest("El modeo es invalido");


            var command = new PatchProductCommand(ProductId, patchDoc);
            var result = await _mediator.Send(command, cancellation);

            return result.Match(
                _ => NoContent(),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );
        }

        [HttpPatch("{productId:guid}/tags")]
        public async Task<IActionResult> AddTags(Guid productId, [FromBody] List<string> tags, CancellationToken cancellation)
        {
            var command = new AddProductTagsCommand(productId, tags);
            var result = await _mediator.Send(command, cancellation);
            return result.Match(
                _ => NoContent(),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );
        }

        [HttpDelete("{productId:guid}/tags")]
        public async Task<IActionResult> RemoveTags(Guid productId, [FromBody] List<string> tags, CancellationToken cancellation)
        {
            var command = new RemoveProductTagsCommand(productId, tags);
            var result = await _mediator.Send(command, cancellation);
            return result.Match(
                _ => NoContent(),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );

        }

        [HttpPut("{productId:guid}/activate")]
        public async Task<IActionResult> Activate(Guid productId, CancellationToken cancellation)
        {
            var command = new ActivateProductCommand(productId);
            var result = await _mediator.Send(command, cancellation);
            return result.Match(
                _ => NoContent(),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );

        }
    }
}
