using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueVault.Net.Models.Inner;

namespace TrueVault.Net.Models
{
	public class ErrorResponse : TrueVaultResponse
	{
		public InnerError Error { get; internal set; }
	}
}
