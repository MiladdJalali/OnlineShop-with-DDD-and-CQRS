using System.ComponentModel.DataAnnotations;

namespace Project.RestApi.V1.Aggregates.Goods.Models
{
    public class GoodRequest
    {
        [Required] public string Name { get; set; }

        [Required] public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public bool IsFragile { get; set; }

        public string Description { get; set; }
    }
}