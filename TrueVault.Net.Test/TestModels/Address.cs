namespace TrueVault.Net.Test.TestModels
{
    public class Address
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int PostalCode { get; set; }
        public string Country { get; set; }
        public AddressType AddressType { get; set; }
    }

    public enum AddressType
    {
        Residential,
        Commercial
    }
}