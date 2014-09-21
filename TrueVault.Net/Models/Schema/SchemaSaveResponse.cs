using System;

namespace TrueVault.Net.Models.Schema
{
    /// <summary>
    ///     Schema, returned from POST or PUT /{VaultId}/schemas/{SchemaId}
    /// </summary>
    public class SchemaSaveResponse : Schema
    {
        internal SchemaSaveResponse(Guid vaultId, string name, params SchemaField[] fields) : base(name, fields)
        {
            VaultId = vaultId;
        }

        internal SchemaSaveResponse(Guid vaultId, Guid id, string name, params SchemaField[] fields)
            : base(id, name, fields)
        {
            VaultId = vaultId;
        }

        public Guid VaultId { get; private set; }
    }
}