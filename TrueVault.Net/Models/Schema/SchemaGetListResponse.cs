using System.Collections.Generic;

namespace TrueVault.Net.Models.Schema
{
    /// <summary>
    ///     Returned from GET /{VaultId}/schemas
    /// </summary>
    public class SchemaGetListResponse : TrueVaultResponse
    {
        public IEnumerable<Schema> Schemas { get; set; }
    }
}