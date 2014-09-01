using AutoMapper;
using FizzWare.NBuilder;
using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using TrueVault.Net.Models;
using TrueVault.Net.Models.JsonStore;
using TrueVault.Net.Models.Schema;
using TrueVault.Net.Test.TestModels;

namespace TrueVault.Net.Test
{
    [TestFixture]
    public class TrueVaultClientTest
    {
        private TrueVaultClient trueVaultClient;
        private Guid testVaultId;
        private ConcurrentStack<DocumentSaveSuccessResponse> documentSuccessResponses;
        private ConcurrentStack<SchemaSaveSuccessResponse> schemaSuccessResponses;

        [TestFixtureSetUp]
        public void Setup()
        {
            trueVaultClient = new TrueVaultClient(ConfigurationManager.AppSettings["TrueVaultApiKey"]);
            documentSuccessResponses = new ConcurrentStack<DocumentSaveSuccessResponse>();
            testVaultId = Guid.Parse(ConfigurationManager.AppSettings["TrueVaultTestVault"]);
            Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void NewDocumentCanBeCreated()
        {
            var person = CreatePerson();
            documentSuccessResponses.Push(AssertCreatePersonSuccess(person, testVaultId));
        }

        [Test]
        public void ExistingDocumentCanBeRetrieved()
        {
            var person = CreatePerson();
            var createResponse = AssertCreatePersonSuccess(person, testVaultId);
            documentSuccessResponses.Push(createResponse);

            var retrievedPerson = trueVaultClient.GetDocument<Person>(testVaultId, createResponse.DocumentId);

            Assert.AreEqual(person.ID, retrievedPerson.ID);
            Assert.AreEqual(person.Name, retrievedPerson.Name);
            Assert.AreEqual(person.Email, retrievedPerson.Email);
        }

        [Test]
        public void ExistingDocumentCanBeEdited()
        {
            var person = CreatePerson();
            var createResponse = AssertCreatePersonSuccess(person, testVaultId);
            documentSuccessResponses.Push(createResponse);

            person.Name += " Justgotmarried";
            person.Email = string.Format("{0}@truevaulttest.net", Guid.NewGuid());

            var editResponse = trueVaultClient.UpdateDocument(testVaultId, createResponse.DocumentId, person);
            Assert.AreEqual(editResponse.Result, "success", "Edit response should indicate success");
            Assert.AreNotEqual(editResponse.TransactionId, default(Guid),
                "Edit response Transaction ID should be a non-default GUID");

            var retrievedPerson = trueVaultClient.GetDocument<Person>(testVaultId, createResponse.DocumentId);

            Assert.AreEqual(person.ID, retrievedPerson.ID);
            Assert.AreEqual(person.Name, retrievedPerson.Name);
            Assert.AreEqual(person.Email, retrievedPerson.Email);
        }

        [Test]
        public void MultipleDocumentsCanBeRetrieved()
        {
            const int personCount = 3;
            var people = CreateMultiplePersons(personCount);

            var createResponses = new Dictionary<Guid, Person>();

            people.ForEach(person =>
            {
                var createResponse = AssertCreatePersonSuccess(person, testVaultId);
                documentSuccessResponses.Push(createResponse);
                createResponses.Add(createResponse.DocumentId, person);
            });

            var multipleRetrievedDocuments = trueVaultClient.MultiGetDocuments(testVaultId,
                createResponses.Select(c => c.Key).ToArray());
            Assert.AreEqual(personCount, multipleRetrievedDocuments.Documents.Count());

            var retrievedPeopleDocs = multipleRetrievedDocuments.Documents;

            foreach (var doc in retrievedPeopleDocs)
            {
                var deserializedPerson = doc.DeserializeDocument<Person>();
                Assert.IsTrue(createResponses.ContainsKey(doc.Id));
                Assert.AreEqual(createResponses[doc.Id].ID, deserializedPerson.ID);
                Assert.AreEqual(createResponses[doc.Id].Name, deserializedPerson.Name);
                Assert.AreEqual(createResponses[doc.Id].Email, deserializedPerson.Email);
            }
        }

        [Test]
        public void SchemaCanBeCreated()
        {
            var personSchema = CreateSchema("Person");
            schemaSuccessResponses.Push(AssertCreateSchemaSuccess(personSchema, testVaultId));
        }

        [Test]
        public void SchemaCanBeDeleted()
        {
            var personSchema = CreateSchema("DeleteMe");
        }

        [TestFixtureTearDown]
        public void Teardown()
        {
            DocumentSaveSuccessResponse documentSuccessResponse;
            while (documentSuccessResponses.TryPop(out documentSuccessResponse))
            {
                trueVaultClient.DeleteDocument(testVaultId, documentSuccessResponse.DocumentId);
            }
        }

        #region Helpers

        private DocumentSaveSuccessResponse AssertCreatePersonSuccess(Person person, Guid vaultId)
        {
            var response = trueVaultClient.CreateDocument(vaultId, person);
            Assert.IsNotNull(response, "Response should not be null");
            Assert.AreNotEqual(response.DocumentId, default(Guid), "Document ID should be a non-default GUID");
            Assert.AreNotEqual(response.TransactionId, default(Guid), "Transaction ID should be a non-default GUID");
            Assert.AreEqual(response.Result, "success", "Response should indicate success");

            return response;
        }

        private Person CreatePerson()
        {
            return Builder<Person>.CreateNew()
                .With(p => p.Email = string.Format("{0}@truevaulttest.com", Guid.NewGuid().ToString()))
                .Build();
        }

        private List<Person> CreateMultiplePersons(int listSize)
        {
            return Builder<Person>.CreateListOfSize(listSize)
                .All().With(p => p.Email = string.Format("{0}@truevaulttest.com", Guid.NewGuid().ToString()))
                .Build().ToList();
        }

        private Schema CreateSchema(string schemaName)
        {
            var personSchema = new Schema(schemaName, new SchemaField("ID", false).AsInteger(), new SchemaField("Name"),
                new SchemaField("Email"));
            return personSchema;
        }
        private SchemaSaveSuccessResponse AssertCreateSchemaSuccess(Schema schema, Guid vaultId)
        {
            var response = trueVaultClient.CreateSchema(vaultId, schema);
            Assert.IsNotNull(response, "Response should not be null");
            Assert.AreNotEqual(response.Schema.Id, default(Guid), "Schema ID should be a non-default GUID");
            Assert.AreNotEqual(response.TransactionId, default(Guid), "Transaction ID should be a non-default GUID");
            Assert.AreEqual(response.Result, "success", "Response should indicate success");
            return response;
        }

        #endregion
    }
}