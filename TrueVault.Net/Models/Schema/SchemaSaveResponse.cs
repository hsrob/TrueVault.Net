using System;

namespace TrueVault.Net.Models.Schema
{
    /// <summary>
    ///     Schema, returned from POST or PUT /{VaultId}/schemas/{SchemaId}
    /// </summary>
    public class SchemaSaveResponse : Schema
    {
        public Guid VaultId { get; set; }
    }
}