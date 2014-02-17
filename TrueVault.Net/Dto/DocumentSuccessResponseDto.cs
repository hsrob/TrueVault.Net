using System;

namespace TrueVault.Net.Dto
{
    internal class DocumentSuccessResponseDto : TrueVaultResponseDto
    {
        public Guid document_id { get; set; }
    }
}