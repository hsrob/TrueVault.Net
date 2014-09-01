using System.Collections.Generic;

namespace TrueVault.Net.Dto.Schema
{
    /// <summary>
    /// Returned from GET /{VaultId}/schemas
    /// </summary>
    internal class SchemaGetListResponseDto : TrueVaultResponseDto
    {
        public IEnumerable<SchemaDto> schemas { get; set; }
    }
}