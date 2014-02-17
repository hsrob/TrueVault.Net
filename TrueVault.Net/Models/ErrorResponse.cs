using TrueVault.Net.Models.Inner;

namespace TrueVault.Net.Models
{
    public class ErrorResponse : TrueVaultResponse
    {
        public InnerError Error { get; internal set; }
    }
}