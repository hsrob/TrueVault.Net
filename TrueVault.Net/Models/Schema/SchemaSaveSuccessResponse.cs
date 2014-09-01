namespace TrueVault.Net.Models.Schema
{
    /// <summary>
    ///     Response wrapper containing a Schema, returned from POST or PUT /{VaultId}/schemas/{SchemaId}
    /// </summary>
    public class SchemaSaveSuccessResponse : TrueVaultResponse
    {
        public SchemaSaveResponse Schema { get; set; }
    }
}