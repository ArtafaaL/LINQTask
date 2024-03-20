namespace ShoppingTable
{
    public class Shopping
    {
        public uint Id { get; }
        public decimal Summa { get; }
        public uint BuyersId { get; }

        public Shopping(uint id, decimal summa, uint buyersId)
        {
            Id = id;
            Summa = summa;
            BuyersId = buyersId;
        }
    }
}
