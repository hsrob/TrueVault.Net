using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueVault.Net.Dto
{
	class DocumentSuccessResponseDto : TrueVaultResponseDto
	{
		public Guid document_id { get; set; }
	}
}
