namespace Project.RestApi.V1.Aggregates.Orders.Models
{
    public class OrderRequest
    {
        public string[] GoodsName { get; set; }

        public string Description { get; set; }
    }
}