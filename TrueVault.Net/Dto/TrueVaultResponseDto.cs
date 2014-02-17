using System;

namespace TrueVault.Net.Dto
{
    internal class TrueVaultResponseDto
    {
        public Guid transaction_id { get; set; }
        public string result { get; set; }
    }
}