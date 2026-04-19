namespace ProductService.Application.ProductsTypes.Queries.GetByIdProductType
{
    public sealed record GetByIdProductTypeQueryResponse(
        Guid ProductTypeId,
        string Code,
        string Name,
        bool IsActive
    );
}
