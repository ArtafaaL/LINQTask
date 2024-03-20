namespace BuyerTable
{
    public class Buyer
    {
        public uint Id { get; }
        public string Name { get; }

        public Buyer (uint id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
