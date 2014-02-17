using System;

namespace TrueVault.Net.Models
{
    public class TrueVaultResponse
    {
        public Guid TransactionId { get; internal set; }
        public string Result { get; internal set; }
    }
}