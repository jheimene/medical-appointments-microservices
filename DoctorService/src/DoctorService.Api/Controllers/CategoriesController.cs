using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Categories.Commands.CreateCategory;
using ProductService.Application.Categories.Queries;

namespace ProductService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return result.Match(
                categoryId => CreatedAtAction(nameof(GetById), new { categoryId }, new { categoryId }),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );
        }

        [HttpGet("{categoryId:guid}", Name = "Category_GetById")]
        public async Task<IActionResult> GetById(Guid categoryId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetByIdCategoryQuery(categoryId), cancellationToken);

            return result.Match(
                categoryId => Ok(categoryId),
                errors => ErrorOrHttp.MapToProblem(this, errors)
            );
        }
    }
}
