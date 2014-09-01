using System;

namespace TrueVault.Net.Dto.JsonStore
{
    internal class DocumentGetResponseDto
    {
        public Guid id { get; set; }
        public string document { get; set; }
    }
}