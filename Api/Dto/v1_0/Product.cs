using System;
using System.Collections.Generic;
using ProductEntity = ProductApiExample.DataLayer.Entities.Product;

namespace ProductApiExample.Api.Dto.v1_0
{
    public class Product : IEquatable<Product?>, IEquatable<ProductEntity?>
    {
        public Product(int id, string name, Uri imgUri, decimal price, string? description)
        {
            Id = id;
            Name = name;
            ImgUri = imgUri;
            Price = price;
            Description = description;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Uri ImgUri { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }

        public override bool Equals(object? obj)
        {
            switch (obj)
            {
                case Product dto: return Equals(dto);
                case ProductEntity entity: return Equals(entity);
                default:
                    return false;
            }
        }

        public bool Equals(Product? other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   EqualityComparer<Uri>.Default.Equals(ImgUri, other.ImgUri) &&
                   Price == other.Price &&
                   Description == other.Description;
        }

        public bool Equals(ProductEntity? other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   EqualityComparer<Uri>.Default.Equals(ImgUri, other.ImgUri) &&
                   Price == other.Price &&
                   Description == other.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, ImgUri, Price, Description);
        }
    }
}
