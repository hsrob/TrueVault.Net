using System;

namespace TrueVault.Net.Models.Schema
{
    /// <summary>
    ///     Response wrapper containing a Schema, returned from POST or PUT /{VaultId}/schemas/{SchemaId}
    /// </summary>
    public class SchemaSaveSuccessResponse : TrueVaultResponse
    {
        internal SchemaSaveSuccessResponse(string result, Guid transactionId, SchemaSaveResponse schemaSaveResponse)
        {
            Result = result;
            TransactionId = transactionId;
            Schema = schemaSaveResponse;
        }
        public SchemaSaveResponse Schema { get; private set; }
    }
}