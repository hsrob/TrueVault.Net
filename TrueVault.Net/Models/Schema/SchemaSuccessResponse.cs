using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueVault.Net.Models.Schema
{
    public class SchemaSuccessResponse : TrueVaultResponse
    {
        public SchemaResponse Schema { get; set; }
    }
}
