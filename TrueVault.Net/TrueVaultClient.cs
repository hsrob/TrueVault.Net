using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AutoMapper;
using ServiceStack.Text;
using TrueVault.Net.Dto;
using TrueVault.Net.Dto.JsonStore;
using TrueVault.Net.Dto.Schema;
using TrueVault.Net.Models;
using TrueVault.Net.Models.JsonStore;
using TrueVault.Net.Models.Schema;

namespace TrueVault.Net
{
    public class TrueVaultClient
    {
        public TrueVaultClient(string apiKey)
        {
            AutoMapperConfig.Configure();
            JsConfig<Guid>.SerializeFn = g => g.ToString();
            JsConfig<Guid?>.SerializeFn = g => g.ToString();
            ClientConfig.Instance.ApiKey = apiKey;
            ClientConfig.Instance.AuthHeader = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(apiKey + ":"));
        }

        #region JSON Store

        /// <summary>
        ///     Creates a document in the specified Vault
        /// </summary>
        /// <param name="vaultId">ID of the Vault to create this document in</param>
        /// <param name="document">Document to store in the Vault</param>
        /// <returns>DocumentSuccessResponse</returns>
        public DocumentSaveSuccessResponse CreateDocument<T>(Guid vaultId, T document) where T : class, new()
        {
            return CreateDocument(vaultId, new DocumentRequestDto<T>(document));
        }

        /// <summary>
        ///     Creates a document in the specified Vault, and instructs the TrueVault search engine to index the document
        ///     according to the given Schema definition
        /// </summary>
        /// <param name="vaultId">ID of the Vault to create this document in</param>
        /// <param name="document">Document to store in the Vault</param>
        /// <param name="schemaId">
        ///     ID of the Schema. The Document will be indexed by the search engine according to the Schema
        ///     definition
        /// </param>
        /// <returns>DocumentSuccessResponse includes TransactionId, Status, DocumentId</returns>
        public DocumentSaveSuccessResponse CreateDocument<T>(Guid vaultId, T document, Guid schemaId)
            where T : class, new()
        {
            return CreateDocument(vaultId, new DocumentRequestDto<T>(document, schemaId));
        }

        private DocumentSaveSuccessResponse CreateDocument<T>(Guid vaultId, DocumentRequestDto<T> documentRequestDto)
            where T : class, new()
        {
            try
            {
                return
                    VaultDocumentsUrl(vaultId)
                        .PostToUrl(documentRequestDto, requestFilter: AuthorizationHeader)
                        .MapResponseDto<DocumentSaveSuccessResponseDto, DocumentSaveSuccessResponse>();
            }
            catch (WebException wex)
            {
                throw ParseWebException(wex);
            }
        }

        /// <summary>
        ///     Updates an existing document
        /// </summary>
        /// <param name="vaultId">ID of the Vault where this document is stored</param>
        /// <param name="documentId">TrueVault Document ID of the existing document to update</param>
        /// <param name="document">Updated document to replace the existing document in the Vault</param>
        /// <returns>TrueVaultResponse includes TransactionId, Status</returns>
        public TrueVaultResponse UpdateDocument<T>(Guid vaultId, Guid documentId, T document) where T : class, new()
        {
            try
            {
                return
                    VaultDocumentUrl(vaultId, documentId)
                        .PutToUrl(new DocumentRequestDto<T>(document), requestFilter: AuthorizationHeader)
                        .MapResponseDto<TrueVaultResponseDto, TrueVaultResponse>();
            }
            catch (WebException wex)
            {
                throw ParseWebException(wex);
            }
        }

        /// <summary>
        ///     Deletes an existing document
        /// </summary>
        /// <param name="vaultId">ID of the Vault where this document is stored</param>
        /// <param name="documentId">TrueVault Document ID of the existing document to delete</param>
        /// <returns></returns>
        public TrueVaultResponse DeleteDocument(Guid vaultId, Guid documentId)
        {
            try
            {
                return
                    VaultDocumentUrl(vaultId, documentId)
                        .DeleteFromUrl(requestFilter: AuthorizationHeader)
                        .MapResponseDto<TrueVaultResponseDto, TrueVaultResponse>();
            }
            catch (WebException wex)
            {
                throw ParseWebException(wex);
            }
        }

        /// <summary>
        ///     Gets an existing document
        /// </summary>
        /// <param name="vaultId">ID of the Vault where this document is stored</param>
        /// <param name="documentId">TrueVault Document ID of the existing document to get</param>
        /// <returns>The requested document</returns>
        public T GetDocument<T>(Guid vaultId, Guid documentId) where T : class, new()
        {
            try
            {
                return
                    VaultDocumentUrl(vaultId, documentId)
                        .GetStringFromUrl(requestFilter: AuthorizationHeader)
                        .MapDocumentResponse<T>();
            }
            catch (WebException wex)
            {
                throw ParseWebException(wex);
            }
        }

        /// <summary>
        ///     Gets multiple existing documents
        /// </summary>
        /// <param name="vaultId">ID of the Vault where these documents are stored</param>
        /// <param name="documentIds">TrueVault Document IDs of the existing documents to get</param>
        /// <returns>The requested document</returns>
        public MultiDocumentResponse MultiGetDocuments(Guid vaultId, params Guid[] documentIds)
        {
            try
            {
                return
                    VaultMultiDocumentUrl(vaultId, documentIds)
                        .GetStringFromUrl(requestFilter: AuthorizationHeader)
                        .MapResponseDto<MultiDocumentGetResponseDto, MultiDocumentResponse>();
            }
            catch (WebException wex)
            {
                throw ParseWebException(wex);
            }
        }

        #endregion

        #region Schemas

        /// <summary>
        ///     Creates a new Schema in the specified Vault
        /// </summary>
        /// <param name="vaultId">ID of the Vault to create this schema in</param>
        /// <param name="schema">Schema to create in the Vault</param>
        /// <returns>SchemaSuccessResponse</returns>
        public SchemaSaveSuccessResponse CreateSchema(Guid vaultId, Schema schema)
        {
            var newSchemaRequestDto = new SchemaSaveRequestDto(Mapper.Map<SchemaDto>(schema));

            try
            {
                return
                    VaultSchemasUrl(vaultId)
                        .PostToUrl(newSchemaRequestDto, requestFilter: AuthorizationHeader)
                        .MapResponseDto<SchemaSaveSuccessResponseDto, SchemaSaveSuccessResponse>();
            }
            catch (WebException wex)
            {
                throw ParseWebException(wex);
            }
        }

        /// <summary>
        ///     Updates an existing document
        /// </summary>
        /// <param name="vaultId">ID of the Vault where this Schema exists</param>
        /// <param name="schemaId">TrueVault Schema ID of the existing Schema to update</param>
        /// <param name="schema">Updated Schema to replace the existing Schema in the Vault</param>
        /// <returns>TrueVaultResponse includes TransactionId, Status</returns>
        public TrueVaultResponse UpdateSchema(Guid vaultId, Guid schemaId, Schema schema)
        {
            try
            {
                return
                    VaultSchemaUrl(vaultId, schemaId)
                        .PutToUrl(new SchemaSaveRequestDto(Mapper.Map<SchemaDto>(schema)),
                            requestFilter: AuthorizationHeader)
                        .MapResponseDto<SchemaSaveSuccessResponseDto, SchemaSaveSuccessResponse>();
            }
            catch (WebException wex)
            {
                throw ParseWebException(wex);
            }
        }

        /// <summary>
        ///     Deletes an existing Schema
        /// </summary>
        /// <param name="vaultId">ID of the Vault where this Schema exists</param>
        /// <param name="schemaId">TrueVault Schema ID of the existing Schema to delete</param>
        /// <returns></returns>
        public TrueVaultResponse DeleteSchema(Guid vaultId, Guid schemaId)
        {
            try
            {
                return
                    VaultSchemaUrl(vaultId, schemaId)
                        .DeleteFromUrl(requestFilter: AuthorizationHeader)
                        .MapResponseDto<TrueVaultResponseDto, TrueVaultResponse>();
            }
            catch (WebException wex)
            {
                throw ParseWebException(wex);
            }
        }

        /// <summary>
        ///     Gets an existing Schema
        /// </summary>
        /// <param name="vaultId">ID of the Vault where this Schema Exists</param>
        /// <param name="schemaId">TrueVault Schema ID of the existing Schema to get</param>
        /// <returns>The requested document</returns>
        public Schema GetSchema(Guid vaultId, Guid schemaId)
        {
            try
            {
                return
                    VaultSchemaUrl(vaultId, schemaId)
                        .GetStringFromUrl(requestFilter: AuthorizationHeader)
                        .MapResponseDto<SchemaGetResponseDto, SchemaGetResponse>().Schema;
            }
            catch (WebException wex)
            {
                throw ParseWebException(wex);
            }
        }

        /// <summary>
        ///     <para>Gets a list of existing Schemas</para>
        ///     <para>
        ///         Note: Schemas listed in the response will not contain Fields. Use GetSchema to get an individual Schema with
        ///         its Fields.
        ///     </para>
        /// </summary>
        /// <param name="vaultId">ID of the Vault where this Schema exists</param>
        /// <returns>The requested document</returns>
        public IEnumerable<Schema> GetSchemaList(Guid vaultId)
        {
            try
            {
                return
                    VaultSchemasUrl(vaultId)
                        .GetStringFromUrl(requestFilter: AuthorizationHeader)
                        .MapResponseDto<SchemaGetListResponseDto, SchemaGetListResponse>().Schemas;
            }
            catch (WebException wex)
            {
                throw ParseWebException(wex);
            }
        }

        #endregion

        #region Helpers

        private WebException ParseWebException(WebException wex)
        {
            var errorResponse = JsonSerializer.DeserializeFromString<ErrorResponseDto>(wex.GetResponseBody());
            return new WebException("TrueVault Transaction ID {0} - {1} Error (Type: {2}) [Code: {3}]: {4}"
                .Fmt(errorResponse.transaction_id, (int) wex.GetStatus().GetValueOrDefault(), errorResponse.error.type,
                    errorResponse.error.code,
                    errorResponse.error.message), wex, wex.Status, wex.Response);
        }

        private void AuthorizationHeader(HttpWebRequest request)
        {
            request.Headers.Add(HttpRequestHeader.Authorization, ClientConfig.Instance.AuthHeader);
        }

        #region Document URLs

        private string VaultMultiDocumentUrl(Guid vaultId, IEnumerable<Guid> documentIds)
        {
            return "{0}{1}/documents/{2}".Fmt(ClientConfig.Instance.TrueVaultBaseUrl, vaultId.ToString(),
                documentIds.Select(d => d.ToString()).Join(","));
        }

        private string VaultDocumentUrl(Guid vaultId, Guid documentId)
        {
            return "{0}{1}/documents/{2}".Fmt(ClientConfig.Instance.TrueVaultBaseUrl, vaultId.ToString(),
                documentId.ToString());
        }

        private string VaultDocumentsUrl(Guid vaultId)
        {
            return "{0}{1}/documents".Fmt(ClientConfig.Instance.TrueVaultBaseUrl, vaultId.ToString());
        }

        #endregion

        #region Schema URLs

        private string VaultSchemaUrl(Guid vaultId, Guid schemaId)
        {
            return "{0}{1}/schemas/{2}".Fmt(ClientConfig.Instance.TrueVaultBaseUrl, vaultId.ToString(),
                schemaId.ToString());
        }

        private string VaultSchemasUrl(Guid vaultId)
        {
            return "{0}{1}/schemas".Fmt(ClientConfig.Instance.TrueVaultBaseUrl, vaultId.ToString());
        }

        #endregion

        #endregion
    }
}