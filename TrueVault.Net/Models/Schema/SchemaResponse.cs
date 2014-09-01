using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueVault.Net.Models.Schema
{
    public class SchemaResponse : Schema
    {
        public Guid Id { get; set; }
        public Guid VaultId { get; set; }
    }
}
