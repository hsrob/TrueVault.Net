using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueVault.Net.Models
{
	public class TrueVaultResponse
	{
		public Guid TransactionId { get; internal set; }
		public string Result { get; internal set; }
	}
}
