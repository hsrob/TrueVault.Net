using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueVault.Net.Dto.Schema
{
    public class SchemaCreateResponseDto : SchemaDto
    {
        public Guid id { get; set; }
        public Guid vault_id { get; set; }
    }
}
