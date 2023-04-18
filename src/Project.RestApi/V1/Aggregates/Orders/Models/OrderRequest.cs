namespace Project.RestApi.V1.Aggregates.Orders.Models
{
    public class OrderRequest
    {
        public OrderGoodRequest[] Goods { get; set; }

        public string Description { get; set; }
    }
}