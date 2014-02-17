using System;
using System.Text;

namespace TrueVault.Net.Dto
{
    internal class DocumentRequestDto
    {
        /// <summary>
        ///     Creates a Document request DTO for POST or PUT operations, accepting a serialized JSON string, which will
        ///     automatically be converted to Base64.
        /// </summary>
        /// <param name="jsonDocument">
        ///     A serialized JSON string representing the document to be stored, which will automatically be
        ///     converted to Base64.
        /// </param>
        public DocumentRequestDto(string jsonDocument)
        {
            document = Convert.ToBase64String(Encoding.ASCII.GetBytes(jsonDocument));
        }

        /// <summary>
        ///     Creates a Document request DTO for POST or PUT operations, accepting a serialized JSON string, which will
        ///     automatically be converted to Base64.
        ///     This document will be associated with the provided Search Engine Schema ID.
        /// </summary>
        /// <param name="jsonDocument">
        ///     A serialized JSON string representing the document to be stored, which will automatically be
        ///     converted to Base64.
        /// </param>
        /// <param name="schemaId">The Search Engine Schema ID to associate this document with</param>
        public DocumentRequestDto(string jsonDocument, Guid schemaId)
            : this(jsonDocument)
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