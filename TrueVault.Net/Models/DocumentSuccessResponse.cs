using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueVault.Net.Models
{
	public class DocumentSuccessResponse : TrueVaultResponse
	{
		public Guid DocumentId { get; internal set; }
	}
}
