using System;

namespace TrueVault.Net.Models.JsonStore
{
    public class DocumentSuccessResponse : TrueVaultResponse
    {
        public Guid DocumentId { get; internal set; }
    }
}