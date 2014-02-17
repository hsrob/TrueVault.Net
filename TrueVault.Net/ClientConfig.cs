using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueVault.Net
{
	public sealed class ClientConfig
	{
		private static readonly ClientConfig instance = new ClientConfig();

		private ClientConfig(){}

		public static ClientConfig Instance
		{
			get { return instance; }
		}

		public string TrueVaultBaseUrl
		{
			get { return _trueVaultBaseUrl; }
			set { _trueVaultBaseUrl = value; }
		}

		private string _trueVaultBaseUrl = "https://api.truevault.com/v1/vaults/";

		public string ApiKey { get; internal set; }
		public string AuthHeader { get; internal set; }
	}
}
