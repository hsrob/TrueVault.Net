using TrueVault.Net.Dto.Inner;

namespace TrueVault.Net.Dto
{
    internal class ErrorResponseDto : TrueVaultResponseDto
    {
        public InnerErrorDto error { get; set; }
    }
}