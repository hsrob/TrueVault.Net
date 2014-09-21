using System.Collections.Generic;
using System.Linq;

namespace TrueVault.Net.Models.JsonStore
{
    public class MultiDocumentResponse : TrueVaultResponse
    {
        public IEnumerable<DocumentResponse> Documents { get; set; }

        public IEnumerable<T> DeserializeDocuments<T>() where T : class, new()
        {
            return Documents.Select(d => d.DeserializeDocument<T>());
        }
    }
}