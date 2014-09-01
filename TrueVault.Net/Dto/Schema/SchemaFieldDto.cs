using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueVault.Net.Dto.Schema
{
    public class SchemaFieldDto
    {
        public string name { get; set; }
        public bool index { get; set; }
        public string type { get; set; }

        /// <summary>
        /// Convenience method to set the type of this Field to "string"
        /// </summary>
        /// <returns></returns>
        public SchemaFieldDto AsString()
        {
            type = "string";
            return this;
        }
        /// <summary>
        /// Convenience method to set the type of this Field to "integer"
        /// </summary>
        /// <returns></returns>
        public SchemaFieldDto AsInteger()
        {
            type = "integer";
            return this;
        }
        /// <summary>
        /// Convenience method to set the type of this Field to "long"
        /// </summary>
        /// <returns></returns>
        public SchemaFieldDto AsLong()
        {
            type = "long";
            return this;
        }
        /// <summary>
        /// Convenience method to set the type of this Field to "float"
        /// </summary>
        /// <returns></returns>
        public SchemaFieldDto AsFloat()
        {
            type = "float";
            return this;
        }
        /// <summary>
        /// Convenience method to set the type of this Field to "boolean"
        /// </summary>
        /// <returns></returns>
        public SchemaFieldDto AsBoolean()
        {
            type = "boolean";
            return this;
        }
        /// <summary>
        /// Convenience method to set the type of this Field to "date"
        /// </summary>
        /// <returns></returns>
        public SchemaFieldDto AsDate()
        {
            type = "date";
            return this;
        }
    }
}