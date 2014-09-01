using System;
using System.Text;
using ServiceStack.Text;

namespace TrueVault.Net.Dto.JsonStore
{
    internal class DocumentRequestDto<T> where T : class, new()
    {
        /// <summary>
        ///     Creates a Document request DTO for POST or PUT operations, accepting an entity T with a parameterless constructor.
        ///     The entity will be serialized and converted to a Base64 Encoded JSON string.
        /// </summary>
        /// <param name="entity">The entity of type T to store as a TrueVault document</param>
        public DocumentRequestDto(T entity)
        {
            document = Convert.ToBase64String(Encoding.ASCII.GetBytes(new JsonSerializer<T>().SerializeToString(entity)));
        }

        /// <summary>
        ///     Creates a Document request DTO for POST or PUT operations, accepting an entity T with a parameterless constructor.
        ///     The entity will be serialized and converted to a Base64 Encoded JSON string.
        ///     This document will be associated with the provided Search Engine Schema ID.
        /// </summary>
        /// <param name="entity">The entity of type T to store as a TrueVault document</param>
        /// <param name="schemaId">The Search Engine Schema ID to associate this document with</param>
        public DocumentRequestDto(T entity, Guid schemaId)
            : this(entity)
        {
            schema_id = schemaId;
        }

        /// <summary>
        ///     Base64 ASCII Encoded JSON
        /// </summary>
        public string document { get; private set; }

        /// <summary>
        ///     Search Engine Schema ID
        /// </summary>
        public Guid? schema_id { get; private set; }
    }
}