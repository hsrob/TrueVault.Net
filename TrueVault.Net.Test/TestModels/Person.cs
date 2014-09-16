using System.Collections.Generic;

namespace TrueVault.Net.Test.TestModels
{
    public class Person
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
    }
}