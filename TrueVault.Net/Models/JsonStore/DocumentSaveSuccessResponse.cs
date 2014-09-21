using System;

namespace TrueVault.Net.Models.JsonStore
{
    public class DocumentSaveSuccessResponse : TrueVaultResponse
    {
        public Guid DocumentId { get; set; }
    }
}