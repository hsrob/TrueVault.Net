using System;
using System.Collections.Generic;

namespace TrueVault.Net.Dto.Schema
{
    /// <summary>
    /// Cannot be AutoMapped to Schema, use the extension method MapToSchema
    /// <see cref="TrueVault.Net.Extensions.MapToSchema" />
    /// </summary>
    internal class SchemaDto
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public IEnumerable<SchemaFieldDto> fields { get; set; }
    }
}