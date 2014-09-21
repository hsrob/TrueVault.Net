using System;

namespace TrueVault.Net.Models.Schema
{
    /// <summary>
    ///     Response wrapper containing a Schema, returned from GET /{VaultId}/schemas/{SchemaId}
    /// </summary>
    public class SchemaGetResponse : TrueVaultResponse
    {
        internal SchemaGetResponse(string result, Guid transactionId, Schema schema)
        {
            Result = result;
            TransactionId = transactionId;
            Schema = schema;
        }
        public Schema Schema { get; private set; }
    }
}