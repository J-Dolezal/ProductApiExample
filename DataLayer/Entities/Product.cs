using System;

namespace ProductApiExample.DataLayer.Entities
{
    public class Product
    {
        /// <summary>
        /// Mas size of <see cref="Name"/> property
        /// </summary>
        public const int NameMaxLength = 100;

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
