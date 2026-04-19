using ProductService.Domain.Brands.ValueObjects;
using ProductService.Domain.Categories.ValueObjects;
using ProductService.Domain.Products.Enums;
using ProductService.Domain.Products.Events;
using ProductService.Domain.Products.ValueObjects;
using ProductService.Domain.ProductsTypes.ValueObjects;

namespace ProductService.Domain.Products
{
    public sealed class Product : AggregateRoot<ProductId, string>
    {
        // Core data
        public ProductName Name { get; private set; } = default!;
        public Slug Slug { get; private set; } = default!;
        public Sku Sku { get; private set; } = default!;
        public string? Description { get; private set; } = default!;
        public ProductStatus Status { get; private set; }
        public Money Price { get; private set; } = default!;

        public ProductTypeId ProductTypeId { get; private set; }
        public BrandId BrandId { get; private set; } = default!;
        public ModelName? Model { get; private set; }

        private readonly HashSet<ProductCategoryLink> _categoryIds = [];
        public IReadOnlyCollection<ProductCategoryLink> CategoryIds => _categoryIds.AsReadOnly();

        private readonly HashSet<Tag> _tags = [];
        public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();

        private readonly List<ProductImage> _images = [];
        public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();

        private readonly List<ProductAttribute> _attributes = [];
        public IReadOnlyCollection<ProductAttribute> Attributes => _attributes.AsReadOnly();

        private Product() { }

        public static Product Create(
            ProductName name,
            Slug slug,
            Sku sku,
            Money price,
            ProductTypeId productTypeId,
            BrandId brandId,
            string? model = null //,
            //string? description = null
        )
        {

            var product = new Product
            {
                Id = ProductId.NewId(),
                Name = name,
                Slug = slug,
                Sku = sku,
                Price = price,
                ProductTypeId = productTypeId,
                BrandId = brandId,
                Model = string.IsNullOrWhiteSpace(model) ? null : ModelName.Create(model),
                //Description = description,
                Status = ProductStatus.Draf
            };

            product.SetCreated("system", DateTime.Now);

            return product;
        }



        public static Product Create(
            string name,
            string slug,
            string sku,
            string currency,
            decimal price,
            ProductTypeId productTypeId,
            BrandId brandId,
            string? model = null,
            string? description = null
            //IEnumerable<ProductCategoryLink>? categories = null,
            //IEnumerable<Tag>? tags = null,
            //IEnumerable<ProductAttribute>? attributes = null,
            //IEnumerable<ProductImage>? images = null
        )
        {

            var product = new Product
            {
                Id = ProductId.NewId(),
                Name = ProductName.Create(name),
                Slug = Slug.Create(slug),
                Sku = Sku.Create(slug),
                Price = Money.Create(price, Currency.Create(currency)),
                ProductTypeId = productTypeId,
                BrandId = brandId,
                Model = string.IsNullOrWhiteSpace(model) ? null : ModelName.Create(model),
                Description = description,
                Status = ProductStatus.Draf              
            };

            product.SetCreated("system", DateTime.Now);            

            //if (categories is not null)
            //    foreach (var c in categories) product._categoryIds.Add(c);

            //if (tags is not null)
            //    foreach (var t in tags) product._tags.Add(t);

            //if (attributes is not null)
            //    foreach (var a in attributes) product.AddOrUpdateAttribute(a);

            //if (images is not null)
            //    product.SetImages(images);


            //// Validar campos obligatorios
            //var errors_fields = ValidateFieldsRequired(name, lastName, docNumber);
            //if (errors_fields.Count > 0)
            //{
            //    throw new DomainValidationException(
            //        code: "cutomer.required_fields",
            //        message: "Reglas de negocio no cumplidas.",
            //        errors: errors_fields);
            //}


            return product;
        }

        public void Confirm()
        {
            // Agregar un evento de dominio
            var productNewEvent = new ProductCreatedDomainEvent(
                ProductId: Id.Value,
                Name: Name.Value,
                Slug: Slug.Value,
                Sku: Sku.Value,
                Status: Status.ToString(),
                Description: Description,
                Currency: Price.Currency.Code,
                Price: Price.Amount,
                ProductTypeId: ProductTypeId.Value,
                BrandId: BrandId.Value,
                Model: Model?.Value,
                Tags: string.Join(", ", Tags.Select(t => t.Value)),
                CategoryIds: CategoryIds.Select(c => c.CategoryId.Value),
                null
            );

            AddDomainEvent(productNewEvent);
        }

        //private static Dictionary<string, string[]> ValidateFieldsRequired(string name, string lastName, string documentNumber)
        //{
        //    var errors = new Dictionary<string, string[]>();

        //    // Validar campos obligatorios
        //    if (string.IsNullOrWhiteSpace(name))
        //        errors["name"] = new[] { "El nombre es requerido." };

        //    if (string.IsNullOrWhiteSpace(lastName))
        //        errors["lastName"] = new[] { "El apellido es requerido." };

        //    if (string.IsNullOrWhiteSpace(documentNumber))
        //        errors["documentNumber"] = new[] { "El número de documento es requerido." };

        //    return errors;
        //}


        // ---------------------------
        // Behavior / Invariants
        // ---------------------------
        public void Rename(string newName)
        {
            Name = ProductName.Create(newName);
            Touch();
        }

        public void ChangeSlug(string newSlug)
        {
            Slug = Slug.Create(newSlug);
            Touch();
        }

        public void SetDescription(string? description)
        {
            Description = description;
            Touch();
        }

        public void SetBrand(BrandId brandId, string? model = null)
        {
            if (brandId.Equals(BrandId)) return;            
            BrandId = brandId;
            Model = string.IsNullOrWhiteSpace(model) ? null : ModelName.Create(model);
            Touch();
        }

        public void ChangeModel(string? model)
        {
            Model = string.IsNullOrWhiteSpace(model) ? null : ModelName.Create(model);
            Touch();
        }

        public void AssignCategory(CategoryId categoryId)
        {
            _categoryIds.Add(new ProductCategoryLink(categoryId));
            Touch();
        }

        public void RemoveCategory(CategoryId categoryId)
        {
            _categoryIds.Remove(new ProductCategoryLink(categoryId));
            Touch();
        }

        public void ReplaceCategories(IEnumerable<CategoryId> categoryIds)
        {
            var incomming = categoryIds.Select(id => new ProductCategoryLink(id)).Distinct().ToList();
            _categoryIds.Clear();
            foreach (var c in incomming) _categoryIds.Add(c);
            Touch();
        }

        public void ReplaceTags(IEnumerable<string> tags)
        {
            var incomming = tags.Select(t => Tag.Create(t)).Distinct().ToList();
            _tags.Clear();
            foreach (var t in incomming) _tags.Add(t);
            Touch();
        }

        public void AddTag(string tag)
        {
            _tags.Add(Tag.Create(tag));
            Touch();
        }

        public void RemoveTag(string tag)
        {
            _tags.Remove(Tag.Create(tag));
            Touch();
        }

        public void AddOrUpdateAttribute(ProductAttribute attr)
        {
            var idx = _attributes.FindIndex(a => a.Key.Equals(attr.Key));
            if (idx >= 0) _attributes[idx] = attr;
            else _attributes.Add(attr);

            Touch();
        }

        public void RemoveAttribute(AttributeKey key)
        {
            _attributes.RemoveAll(a => a.Key.Equals(key));
            Touch();
        }

        public void SetImages(IEnumerable<ProductImage> images)
        {
            var list = images.ToList();
            if (list.Count(i => i.IsMain) > 1)
                throw new BusinessRuleViolationException("", "Solo puede existir una imagen principal por producto.");

            _images.Clear();
            _images.AddRange(list.OrderBy(i => i.SortOrder));
            Touch();
        }

        //public ProductVariant AddVariant(ProductVariant variant)
        //{
        //    // Invariante: no repetir combinación (Size+Color) dentro del mismo producto
        //    if (_variants.Any(v => v.SameOptionsAs(variant)))
        //        throw new BusinessRuleViolationException("Ya existe una variante con la misma combinación (talla/color).");

        //    _variants.Add(variant);
        //    Touch();
        //    return variant;
        //}

        //public void RemoveVariant(VariantId variantId)
        //{
        //    _variants.RemoveAll(v => v.Id.Equals(variantId));
        //    Touch();
        //}

        public void Activate()
        {
            Guard.Against(_categoryIds.Count == 0, "", "Para activar el producto debe tener al menos una categoría.");
            //Guard.Against(_variants.Count == 0, "", "Para activar el producto debe tener al menos una variante.");

            // Regla: cada variante activa debe tener al menos 1 precio
            //Guard.Against(_variants.Any(v => v.IsActive && v.Prices.Count == 0),
            //    "No se puede activar: existe una variante activa sin precio.");

            Status = ProductStatus.Active;
            AddDomainEvent(new ProductActivateDomainEvent(Id.Value, Name.Value, Slug.Value));
            Touch();
        }

        public void Discontinue(string reason)
        {
            Guard.AgainstNullOrWhiteSpace(reason, "", "La razón de descontinuación es requerida.");
            Status = ProductStatus.Discontinued;
            Touch();
        }

        private void Touch() => LastModifiedAt = DateTime.Now;
    }
}
