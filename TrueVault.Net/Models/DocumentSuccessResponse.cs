using System;

namespace TrueVault.Net.Models
{
    public class DocumentSuccessResponse : TrueVaultResponse
    {
        public Guid DocumentId { get; internal set; }
    }
}