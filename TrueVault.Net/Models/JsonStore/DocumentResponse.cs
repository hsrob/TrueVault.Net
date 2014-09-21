using System;
using System.Text;
using ServiceStack.Text;

namespace TrueVault.Net.Models.JsonStore
{
    public class DocumentResponse
    {
        public Guid Id { get; set; }
        public string Document { get; set; }

        public T DeserializeDocument<T>() where T : class, new()
        {
            return JsonSerializer.DeserializeFromString<T>(Encoding.ASCII.GetString(Convert.FromBase64String(Document)));
        }
    }
}