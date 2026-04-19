namespace ProductService.Application.Products.Commands.AddProductTags
{
    public sealed record AddTagsRequestDto(List<string> Tags) { }
}
