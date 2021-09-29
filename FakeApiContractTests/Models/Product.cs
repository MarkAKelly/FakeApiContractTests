using System;
using System.ComponentModel.DataAnnotations;

namespace FakeApiContractTests.Models
{
    public class Product
    {
        [Required]
        public string id { get; set; }
        [Required]
        public string name { get; set; }
        public string category { get; set; }
        [Required]
        public Price price { get; set; }
    }
}
