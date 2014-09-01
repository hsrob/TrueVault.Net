using System;
using System.Collections.Generic;
using System.Linq;

namespace TrueVault.Net.Models.Schema
{
    /// <summary>
    ///     <para>
    ///         A Schema specifies the fields in a Document to index by the search engine. Schemas are specific to a Vault so
    ///         you may update schemas in one Vault without affecting the schemas in another Vault.
    ///     </para>
    ///     <para>
    ///         When a Schema is updated, all Documents associated with the updated Schema will be automatically re-indexed
    ///         by our search engine.
    ///     </para>
    ///     <remarks>A Schema with Documents associated with it can not be deleted. You must delete all documents first.</remarks>
    /// </summary>
    public class Schema
    {
        /// <summary>
        ///     Create a new Schema
        /// </summary>
        public Schema()
        {
        }

        /// <summary>
        ///     Create a new Schema with the given Name and Fields
        /// </summary>
        /// <param name="name">Required, the name of this Schema</param>
        /// <param name="fields">The Fields to include in this Schema</param>
        /// <exception cref="System.InvalidOperationException">A schema must have a valid, non-empty name</exception>
        /// <exception cref="System.InvalidOperationException">A Schema must include one or more fields</exception>
        public Schema(string name, params SchemaField[] fields)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("A Schema must have a valid, non-empty name");
            if (fields == null || !fields.Any())
                throw new InvalidOperationException("A Schema must include one or more fields");
            Name = name;
            Fields = fields;
        }

        /// <summary>
        ///     Create a new Schema with the given Name and Fields
        /// </summary>
        /// <param name="id">The Id of this Schema (only set from a response)</param>
        /// <param name="name">Required, the name of this Schema</param>
        /// <param name="fields">The Fields to include in this Schema</param>
        /// <exception cref="System.InvalidOperationException">A schema must have a valid, non-empty name</exception>
        /// <exception cref="System.InvalidOperationException">A Schema must include one or more fields</exception>
        internal Schema(Guid id, string name, params SchemaField[] fields)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("A Schema must have a valid, non-empty name");
            if (fields == null || !fields.Any())
                throw new InvalidOperationException("A Schema must include one or more fields");
            if(id == default(Guid))
                throw new InvalidOperationException("A Schema Id must be a valid (non-default) Guid");
            Id = id;
            Name = name;
            Fields = fields;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<SchemaField> Fields { get; set; }
    }
}