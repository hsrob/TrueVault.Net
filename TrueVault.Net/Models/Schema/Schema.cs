using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueVault.Net.Models.Schema
{
    public class Schema
    {
        /// <summary>
        /// Create a new Schema
        /// </summary>
        public Schema(){}

        /// <summary>
        /// Create a new Schema with the given Name and Fields
        /// </summary>
        /// <param name="name">Required, the name of this Schema</param>
        /// <param name="fields">The Fields to include in this Schema</param>
        /// <exception cref="System.InvalidOperationException">A schema must have a valid, non-empty name</exception>
        /// <exception cref="System.InvalidOperationException">A Schema must include one or more fields</exception>
        public Schema(string name, params SchemaField[] fields)
        {
            if(string.IsNullOrWhiteSpace(name)) throw new InvalidOperationException("A schema must have a valid, non-empty name");
            if(fields == null || !fields.Any()) throw new InvalidOperationException("A Schema must include one or more fields");
            Name = name;
            Fields = fields;
        }
        public string Name { get; set; }
        public IEnumerable<SchemaField> Fields { get; set; }
    }
}
