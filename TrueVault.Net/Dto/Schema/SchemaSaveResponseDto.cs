using System;

namespace TrueVault.Net.Dto.Schema
{
    internal class SchemaSaveResponseDto : SchemaDto
    {
        public Guid vault_id { get; set; }
    }
}