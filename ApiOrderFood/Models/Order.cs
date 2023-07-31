namespace ApiOrderFood.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public string TableNumber { get; set; }
        public decimal TotalAmount { get; set; }    
        public List<OrderItem> Items { get; set; }
    }

    public class OrderItem
    {
        public string OrderItemId { get; set; }
        public string OrderId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
    }

}
