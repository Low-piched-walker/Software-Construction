using System;

namespace OM
{
    class OrderDetail
    {
        public string ItemName { get; set; }
        public int ItemQuantity { get; set; }
        public double ItemPrice { get; set; }

        public OrderDetail() { }

        public OrderDetail(string itemName, int itemQuantity, double itemPrice)
        {
            ItemName = itemName;
            ItemQuantity = itemQuantity;
            ItemPrice = itemPrice;
        }

        public override bool Equals(object obj)
        {
            return obj is OrderDetail detail &&
                   ItemName == detail.ItemName &&
                   ItemQuantity == detail.ItemQuantity &&
                   ItemPrice == detail.ItemPrice;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ItemName, ItemQuantity, ItemPrice);
        }

        public override string ToString()
        {
            return $"ItemName:{ItemName}, ItemQuantity:{ItemQuantity}, ItemPrice:{ItemPrice}";
        }
    }
}
