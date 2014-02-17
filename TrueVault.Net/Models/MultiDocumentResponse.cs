using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Text;

namespace TrueVault.Net.Models
{
    public class MultiDocumentResponse : TrueVaultResponse
    {
        public IEnumerable<DocumentResponse> Documents { get; internal set; }

        public IEnumerable<T> DeserializeDocuments<T>() where T : class, new()
        {
            return Documents.Select(d => d.DeserializeDocument<T>());
        }
    }

    public class DocumentResponse
    {
        public Guid Id { get; internal set; }
        public string Document { get; internal set; }

        public T DeserializeDocument<T>() where T : class, new()
        {
            return JsonSerializer.DeserializeFromString<T>(Encoding.ASCII.GetString(Convert.FromBase64String(Document)));
        }
    }
}