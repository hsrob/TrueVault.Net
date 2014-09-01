using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueVault.Net.Models.Schema
{
    public class SchemaField
    {
        public SchemaField(){}

        /// <summary>
        /// Create a default "string" Type Schema Field
        /// </summary>
        /// <param name="name">Field Name</param>
        /// <param name="index">Whether to index this field in the TrueVault search engine (optional, default: true)</param>
        public SchemaField(string name, bool index = true)
        {
            Name = name;
            Index = index;
            Type = "string";
        }

        /// <summary>
        /// Create a Schema Field of the given Type
        /// </summary>
        /// <param name="name">Field Name</param>
        /// <param name="type">The Type of this Field</param>
        /// <param name="index">Whether to index this field in the TrueVault search engine (optional, default: true)</param>
        public SchemaField(string name, string type, bool index = true)
        {
            Name = name;
            Index = index;
            Type = type;
        }
        /// <summary>
        /// The name of this Field
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Whether to index this Field in the TrueVault search engine
        /// </summary>
        public bool Index { get; set; }
        /// <summary>
        /// A field type is required for all non-string fields. Valid types are: string, integer/long, float/double, boolean and date (e.g. "2009-11-15T14:12:12")
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Convenience method to set the Type of this Field to "string"
        /// </summary>
        /// <returns></returns>
        public SchemaField AsString()
        {
            Type = "string";
            return this;
        }
        /// <summary>
        /// Convenience method to set the Type of this Field to "integer"
        /// </summary>
        /// <returns></returns>
        public SchemaField AsInteger()
        {
            Type = "integer";
            return this;
        }
        /// <summary>
        /// Convenience method to set the Type of this Field to "long"
        /// </summary>
        /// <returns></returns>
        public SchemaField AsLong()
        {
            Type = "long";
            return this;
        }
        /// <summary>
        /// Convenience method to set the Type of this Field to "float"
        /// </summary>
        /// <returns></returns>
        public SchemaField AsFloat()
        {
            Type = "float";
            return this;
        }
        /// <summary>
        /// Convenience method to set the Type of this Field to "boolean"
        /// </summary>
        /// <returns></returns>
        public SchemaField AsBoolean()
        {
            Type = "boolean";
            return this;
        }
        /// <summary>
        /// Convenience method to set the Type of this Field to "date"
        /// </summary>
        /// <returns></returns>
        public SchemaField AsDate()
        {
            Type = "date";
            return this;
        }
    }
}
