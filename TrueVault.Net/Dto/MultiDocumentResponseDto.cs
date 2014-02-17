using System;
using System.Collections.Generic;

namespace TrueVault.Net.Dto
{
    internal class MultiDocumentResponseDto : TrueVaultResponseDto
    {
        public IEnumerable<DocumentResponseDto> documents { get; set; }
    }

    internal class DocumentResponseDto
    {
        public Guid id { get; set; }
        public string document { get; set; }
    }
}