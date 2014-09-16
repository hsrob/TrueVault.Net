using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using ServiceStack.Text;

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
            Fields = new List<SchemaField>();
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
            Fields = fields.ToList();
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
            Fields = fields.ToList();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<SchemaField> Fields { get; set; }
    }

    public class Schema<T> : Schema where T : class
    {
        public Schema() : base(){}

        public Schema(string name, params SchemaField[] fields) : base(name, fields){}

        internal Schema(Guid id, string name, params SchemaField[] fields) : base(id, name, fields){}

        /// <summary>
        /// Register a field definition in a nested type (ex. "Nested.NestedField")
        /// </summary>
        /// <typeparam name="TNested">The type of the property containing the nested field you wish to index</typeparam>
        /// <param name="fieldExpression">An expression providing an accessor for the field definition containing the target nested field from T</param>
        /// <param name="nestedFieldExpression">An expression providing an accessor for the nested field from TNested</param>
        /// <param name="fieldType">The TrueVault field type to index the nested field as. Valid types are: string, integer/long, float/double, boolean</param>
        /// <param name="index">Whether to index the nested field</param>
        /// <returns></returns>
        public Schema<T> RegisterNestedField<TNested>(Expression<Func<T, object>> fieldExpression,  
            Expression<Func<TNested, object>> nestedFieldExpression, 
            string fieldType = "string", 
            bool index = true) where TNested : class
        {
            //Try accessing via UnaryExpression Operand for value types
            //http://stackoverflow.com/questions/12975373/expression-for-type-members-results-in-different-expressions-memberexpression
            var sf = Utils.GetMemberExpression(fieldExpression);
            var nf = Utils.GetMemberExpression(nestedFieldExpression);

            if (sf != null && nf != null)
            {
                Fields.Add(new SchemaField("{0}.{1}".Fmt(sf.Member.Name, nf.Member.Name), fieldType, index));
            }
            return this;
        }
    }
}