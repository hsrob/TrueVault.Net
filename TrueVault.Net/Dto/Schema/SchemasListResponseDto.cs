using System.Collections.Generic;

namespace TrueVault.Net.Dto.Schema
{
    internal class SchemasListResponseDto : TrueVaultResponseDto
    {
        public IEnumerable<SchemaListSchemaDto> schemas { get; set; }
    }
}