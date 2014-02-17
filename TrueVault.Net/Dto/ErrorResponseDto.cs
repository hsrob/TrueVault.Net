using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueVault.Net.Dto.Inner;

namespace TrueVault.Net.Dto
{
	class ErrorResponseDto : TrueVaultResponseDto
	{
		public InnerErrorDto error { get; set; }
	}
}
