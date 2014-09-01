using System;

namespace TrueVault.Net.Dto.JsonStore
{
    internal class DocumentSaveSuccessResponseDto : TrueVaultResponseDto
    {
        public Guid document_id { get; set; }
    }
}