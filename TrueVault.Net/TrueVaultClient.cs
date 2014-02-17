using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ServiceStack.Text;
using TrueVault.Net.Dto;
using TrueVault.Net.Models;

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

        /// <summary>
        ///     Creates a document in the specified Vault
        /// </summary>
        /// <param name="vaultId">ID of the Vault to create this document in</param>
        /// <param name="document">Document to store in the Vault</param>
        /// <returns>DocumentSuccessResponse</returns>
        public DocumentSuccessResponse CreateDocument<T>(Guid vaultId, T document) where T : class, new()
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
        public DocumentSuccessResponse CreateDocument<T>(Guid vaultId, T document, Guid schemaId) where T : class, new()
        {
            return CreateDocument(vaultId, new DocumentRequestDto<T>(document, schemaId));
        }

        private DocumentSuccessResponse CreateDocument<T>(Guid vaultId, DocumentRequestDto<T> documentRequestDto)
            where T : class, new()
        {
            try
            {
                return
                    VaultDocumentsUrl(vaultId)
                        .PostToUrl(documentRequestDto, requestFilter: AuthorizationHeader)
                        .MapResponseDto<DocumentSuccessResponseDto, DocumentSuccessResponse>();
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
        /// <param name="vaultId">ID of the Vault where these document are stored</param>
        /// <param name="documentIds">TrueVault Document IDs of the existing documents to get</param>
        /// <returns>The requested document</returns>
        public MultiDocumentResponse MultiGetDocuments<T>(Guid vaultId, params Guid[] documentIds)
            where T : class, new()
        {
            try
            {
                return
                    VaultMultiDocumentUrl(vaultId, documentIds)
                        .GetStringFromUrl(requestFilter: AuthorizationHeader)
                        .MapResponseDto<MultiDocumentResponseDto, MultiDocumentResponse>();
            }
            catch (WebException wex)
            {
                throw ParseWebException(wex);
            }
        }

        private WebException ParseWebException(WebException wex)
        {
            var errorResponse = JsonSerializer.DeserializeFromString<ErrorResponseDto>(wex.GetResponseBody());
            return new WebException("TrueVault Transaction ID {0} Error (Type: {1}) [Code: {2}]: {3}"
                .Fmt(errorResponse.transaction_id, errorResponse.error.type, errorResponse.error.code,
                    errorResponse.error.message), wex, WebExceptionStatus.ProtocolError, wex.Response);
        }

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

        private void AuthorizationHeader(HttpWebRequest request)
        {
            request.Headers.Add(HttpRequestHeader.Authorization, ClientConfig.Instance.AuthHeader);
        }
    }
}