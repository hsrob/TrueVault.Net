using System;

namespace TrueVault.Net.Dto.JsonStore
{
    internal class DocumentSuccessResponseDto : TrueVaultResponseDto
    {
        public Guid document_id { get; set; }
    }
}