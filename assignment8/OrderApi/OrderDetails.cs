namespace OrderApi
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int OrderId { get; set; }  // 外键关系
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int TotalAmount { get; set; }
    }
}
