using System.Collections.Generic;

namespace TrueVault.Net.Dto.JsonStore
{
    internal class MultiDocumentGetResponseDto : TrueVaultResponseDto
    {
        public IEnumerable<DocumentGetResponseDto> documents { get; set; }
    }
}