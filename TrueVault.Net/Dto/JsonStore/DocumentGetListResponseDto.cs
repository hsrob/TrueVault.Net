using System;
using System.Collections.Generic;

namespace TrueVault.Net.Dto.JsonStore
{
    internal class DocumentGetListResponseDto : TrueVaultResponseDto
    {
        public DocumentGetListDataDto data { get; set; }
    }

    internal class DocumentGetListDataDto
    {
        public bool full_document { get; set; }
        public IEnumerable<DocumentGetListItemDto> items { get; set; }
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
    }

    internal class DocumentGetListItemDto : DocumentGetResponseDto
    {
        public Guid? schema_id { get; set; }
        public Guid vault_id { get; set; }
    }
}