using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueVault.Net.Dto
{
	class TrueVaultResponseDto
	{
		public Guid transaction_id { get; set; }
		public string result { get; set; }
	}
}
