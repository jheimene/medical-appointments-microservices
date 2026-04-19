using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ProductService.Application.Abstractions.Persistence;
using ProductService.Application.Abstractions.Queries;
using ProductService.Application.Products.Queries.SearchProducts;
using ProductService.Domain.Products.Enums;
using ProductService.Infrastructure.Persistence.Contexts;
using ProductService.Infrastructure.Persistence.Factories;
using System.Data;
using System.Text;
using System.Text.Json;

namespace ProductService.Infrastructure.Persistence.Repositories
{
    public class ProductSearchReadRepository : IProductSearchReadRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDbConnection _dbConnection;
        private readonly IDistributedCache _cache;

        public ProductSearchReadRepository(
            ApplicationDbContext applicationDbContext,
            ISqlConnectionFactory sqlConnectionFactory,
            IDistributedCache cache
        )
        {
            _dbContext = applicationDbContext;
            _dbConnection = sqlConnectionFactory.CreateConnection();
            _cache = cache;
        }

        public async Task<PagedResult<ProductSearchItemDto>> SearchAsync(SearchProductsQuery productSearch, CancellationToken cancellationToken)
        {
            var key = MakeCacheKey(productSearch);
            var cache = await _cache.GetStringAsync(key, cancellationToken);
            if (cache is not null)
                return JsonSerializer.Deserialize<PagedResult<ProductSearchItemDto>>(cache)!;

            PagedResult<ProductSearchItemDto> result;

            if (!string.IsNullOrWhiteSpace(productSearch.Text) && productSearch.Text.Length >= 3)
            {
                result = SearchFullTextAsync(productSearch, cancellationToken).GetAwaiter().GetResult();
            }
            else
            {
                result = SearchRegularAsync(productSearch, cancellationToken).GetAwaiter().GetResult();
            }

            // Cache: TTL corto
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(120)
            };
            await _cache.SetStringAsync(key, JsonSerializer.Serialize(result), options, cancellationToken);

            return result;
        }

        private async Task<PagedResult<ProductSearchItemDto>> SearchFullTextAsync(SearchProductsQuery filter, CancellationToken cancellationToken)
        {
            var take = Math.Clamp(filter.Take, 1, 100);

            var sql = """                
            SELECT TOP(@take)
                P.ProductId, P.Name, P.Slug, P.Sku, P.ProductType, P.Brand, P.Model, P.Description, P.CreatedAt, 1 [IsActive]
            FROM CONTAINSTABLE([Product].[ProductSearch], (Name, ProductType, Brand, Model, Description), @query) FT 
            INNER JOIN [Product].[ProductSearch] P ON P.ProductId = FT.[KEY]
            WHERE P.IsDeleted = 0
            AND (@createdAt IS NULL OR P.CreatedAt > @createdAt)
            ORDER BY FT.[RANK] DESC, P.CreatedAt ASC, P.ProductId ASC;             
            """;

            var rows = await _dbConnection.QueryAsync<ProductSearchItemDto>(new CommandDefinition(
                sql,
                new
                {
                    take,
                    query = filter.Text,
                    productTypeId = filter.ProductTypeId,
                    brandId = filter.BrandId,
                    model = filter.Model,
                    createdAt = filter.LastStart
                },
                cancellationToken: cancellationToken,
                commandType: CommandType.Text)
             );

            return new PagedResult<ProductSearchItemDto>
            {
                Items = [.. rows],
                NextCursor = null
            };
        }

        private async Task<PagedResult<ProductSearchItemDto>> SearchRegularAsync(SearchProductsQuery filter, CancellationToken cancellationToken)
        {
            var take = Math.Clamp(filter.Take, 1, 100);

            var query = _dbContext.ProductSearches.AsNoTracking().Where(e => !e.IsDeleted);
            
            if (!string.IsNullOrEmpty(filter.ProductTypeId)) query = query.Where(e => e.ProductTypeId == Guid.Parse(filter.ProductTypeId));
            if (!string.IsNullOrEmpty(filter.BrandId)) query = query.Where(e => e.BrandId == Guid.Parse(filter.BrandId));
            if (!string.IsNullOrEmpty(filter.Model)) query = query.Where(e => e.Model == filter.Model);

            if (filter.LastStart is not null || filter.LastId is not null)
            {
                var last = filter.LastStart ?? DateTime.MinValue;
                var lastId = filter.LastId ?? Guid.Empty;

                query = query.Where(e => e.CreatedAt > last || (e.CreatedAt == last && e.ProductId.CompareTo(lastId) > 0));
            }

            var items = await query
                .OrderBy(e => e.CreatedAt)
                .ThenBy(e => e.ProductId)
                .Take(take)
                .Select(e => new ProductSearchItemDto
                (
                    e.ProductId,
                    e.Name,
                    e.Sku,
                    e.Slug,
                    e.Brand,
                    e.Model,
                    e.CreatedAt,
                    (e.Status == ProductStatus.Active.ToString())
                ))
                .ToListAsync(cancellationToken);

            var next = items.Count == take
                ? new Cursor(items[^1].CreatedAt.ToString("O"), items[^1].ProductId.ToString())
                : null;

            return new PagedResult<ProductSearchItemDto> { Items = items, NextCursor = next };
        }

        private static string MakeCacheKey(SearchProductsQuery query)
        {
            var minified = JsonSerializer.Serialize(query);
            return $"search:products:v1{Convert.ToBase64String(Encoding.UTF8.GetBytes(minified))}";
        }
    }
}
