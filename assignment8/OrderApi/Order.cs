namespace OrderApi
{
    public class Order
    {
        public int ID { get; set; }
        public string Customer { get; set; }
        public int Money { get; set; }
        public List<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();

        public void CalculateTotalAmount()
        {
            Money = 0;
            foreach (var detail in OrderDetails)
            {
                Money += detail.TotalAmount;
            }
        }
    }
}
