namespace TrueVault.Net.Models.Schema
{
    /// <summary>
    ///     Response wrapper containing a Schema, returned from GET /{VaultId}/schemas/{SchemaId}
    /// </summary>
    public class SchemaGetResponse : TrueVaultResponse
    {
        public Schema Schema { get; set; }
    }
}