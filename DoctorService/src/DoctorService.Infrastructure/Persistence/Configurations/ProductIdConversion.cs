
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductService.Domain.Brands.ValueObjects;
using ProductService.Domain.Categories.ValueObjects;
using ProductService.Domain.Products.ValueObjects;
using ProductService.Domain.ProductsTypes.ValueObjects;

namespace ProductService.Infrastructure.Persistence.Configurations
{
    public class ProductIdConversion : ValueConverter<ProductId, Guid>
    {
        public ProductIdConversion() : base(
            id => id.Value, // Convertir CustomerId a Guid para almacenar en la base de datos
            value => new ProductId(value)) // Convertir Guid de la base de datos a CustomerId al leer
        {
        }
    }

    public class CategoryIdConversion : ValueConverter<CategoryId, Guid>
    {
        public CategoryIdConversion() : base(
            id => id.Value, // Convertir CategoryId a Guid para almacenar en la base de datos
            value => new CategoryId(value)) // Convertir Guid de la base de datos a CategoryId al leer
        {
        }
    }

    public class BrandIdConversion : ValueConverter<BrandId, Guid>
    {
        public BrandIdConversion() : base(
            id => id.Value, // Convertir BrandId a Guid para almacenar en la base de datos
            value => new BrandId(value)) // Convertir Guid de la base de datos a BrandId al leer
        {
        }
    }

    public class ProductTypeIdConversion : ValueConverter<ProductTypeId, Guid>
    {
        public ProductTypeIdConversion() : base(
            id => id.Value, // Convertir BrandId a Guid para almacenar en la base de datos
            value => new ProductTypeId(value)) // Convertir Guid de la base de datos a BrandId al leer
        {
        }
    }

}
