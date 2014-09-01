namespace TrueVault.Net.Dto.Schema
{
    /// <summary>
    ///     Returned from GET /{VaultId}/schemas/{SchemaId}
    /// </summary>
    internal class SchemaGetResponseDto : TrueVaultResponseDto
    {
        public SchemaDto schema { get; set; }
    }
}