using System;
using System.Text;
using ServiceStack.Text;

namespace TrueVault.Net.Dto.Schema
{
    internal class SchemaSaveRequestDto
    {
        /// <summary>
        ///     Creates a Schema request DTO for POST or PUT operations.
        ///     The Schema will be serialized and converted to a Base64 Encoded JSON string.
        /// </summary>
        /// <param name="schemaDto">The schema to create</param>
        public SchemaSaveRequestDto(SchemaDto schemaDto)
        {
            schema =
                Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(new JsonSerializer<SchemaDto>().SerializeToString(schemaDto)));
        }

        /// <summary>
        ///     Base64 ASCII Encoded JSON
        /// </summary>
        public string schema { get; private set; }
    }
}