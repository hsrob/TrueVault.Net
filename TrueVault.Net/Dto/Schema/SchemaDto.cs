using System.Collections.Generic;

namespace TrueVault.Net.Dto.Schema
{
    public class SchemaDto
    {
        public string name { get; set; }
        public IEnumerable<SchemaFieldDto> fields { get; set; }
    }
}