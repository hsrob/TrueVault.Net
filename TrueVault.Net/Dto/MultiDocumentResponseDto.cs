using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueVault.Net.Dto
{
	class MultiDocumentResponseDto : TrueVaultResponseDto
	{
		public IEnumerable<DocumentResponseDto> documents { get; set; }
	}

	class DocumentResponseDto
	{
		public Guid id { get; set; }
		public string document { get; set; }
	}
}
