using System;
using System.Collections.Generic;

namespace TrueVault.Net.Models.Schema
{
    /// <summary>
    ///     Returned from GET /{VaultId}/schemas
    /// </summary>
    public class SchemaGetListResponse : TrueVaultResponse
    {
        internal SchemaGetListResponse(string result, Guid transactionId, IEnumerable<Schema> schemas)
        {
            Result = result;
            TransactionId = transactionId;
            Schemas = schemas;
        }
        public IEnumerable<Schema> Schemas { get; private set; }
    }
}