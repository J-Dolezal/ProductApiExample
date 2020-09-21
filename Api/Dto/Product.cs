using System;

namespace ProductApiExample.Api.Dto
{
    public class Product
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
    }
}
