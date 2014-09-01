using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueVault.Net.Dto.Schema
{
    internal class SchemaListSchemaDto
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public Guid vault_id { get; set; }
    }
}
