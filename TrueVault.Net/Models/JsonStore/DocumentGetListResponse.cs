using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Text;

namespace TrueVault.Net.Models.JsonStore
{
    public class DocumentGetListResponse : TrueVaultResponse
    {
        public DocumentGetListData Data { get; set; }
    }

    public class DocumentGetListData
    {
        public bool FullDocument { get; set; }
        public List<DocumentGetListItem> Items { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }
        public List<T> DeserializeDocuments<T>() where T : class, new()
        {
            return Items.Select(d => d.DeserializeDocument<T>()).ToList();
        }
    }

    public class DocumentGetListItem : DocumentResponse
    {
        public Guid? SchemaId { get; set; }
        public Guid VaultId { get; set; }
    }
}