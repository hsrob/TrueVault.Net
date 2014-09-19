using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using FizzWare.NBuilder;
using NUnit.Framework;
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
            trueVaultClient = new TrueVaultClient(TestConfig.Instance.TrueVaultApiKey);
            documentSuccessResponses = new ConcurrentStack<DocumentSaveSuccessResponse>();
            schemaSuccessResponses = new ConcurrentStack<SchemaSaveSuccessResponse>();
            testVaultId = Guid.Parse(TestConfig.Instance.TrueVaultTestVault);
            Mapper.AssertConfigurationIsValid();
        }

        [TestFixtureTearDown]
        public void Teardown()
        {
            DocumentSaveSuccessResponse documentSuccessResponse;
            while (documentSuccessResponses.TryPop(out documentSuccessResponse))
            {
                trueVaultClient.DeleteDocument(testVaultId, documentSuccessResponse.DocumentId);
            }

            SchemaSaveSuccessResponse schemaSuccessResponse;
            while (schemaSuccessResponses.TryPop(out schemaSuccessResponse))
            {
                trueVaultClient.DeleteSchema(testVaultId, schemaSuccessResponse.Schema.Id);
            }
        }

        private DocumentSaveSuccessResponse AssertCreatePersonSuccess(Person person, Guid vaultId)
        {
            DocumentSaveSuccessResponse response = trueVaultClient.CreateDocument(vaultId, person);
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
            List<Person> people = Builder<Person>.CreateListOfSize(listSize)
                .All()
                .With(p => p.Email = string.Format("{0}@truevaulttest.com", Guid.NewGuid().ToString()))
                .Build().ToList();

            people.ForEach(p =>
                p.Address = Builder<Address>
                    .CreateNew()
                    .With(a => a.PersonId = p.Id)
                    .Build());

            return people;
        }

        private Schema CreatePersonSchema(string schemaName)
        {
            var personSchema = new Schema<Person>(schemaName, new SchemaField("Id", false).AsInteger(),
                new SchemaField("Name"),
                new SchemaField("Email"));
            personSchema
                .RegisterNestedField<Address>(p => p.Address, a => a.Id, null, false)
                .RegisterNestedField<Address>(p => p.Address, a => a.PersonId)
                .RegisterNestedField<Address>(p => p.Address, a => a.Street)
                .RegisterNestedField<Address>(p => p.Address, a => a.City)
                .RegisterNestedField<Address>(p => p.Address, a => a.State)
                .RegisterNestedField<Address>(p => p.Address, a => a.PostalCode)
                .RegisterNestedField<Address>(p => p.Address, a => a.Country)
                .RegisterNestedField<Address>(p => p.Address, a => a.AddressType);
            return personSchema;
        }

        private SchemaSaveSuccessResponse AssertCreateSchemaSuccess(Schema schema, Guid vaultId)
        {
            SchemaSaveSuccessResponse response = trueVaultClient.CreateSchema(vaultId, schema);
            Assert.IsNotNull(response, "Response should not be null");
            Assert.AreNotEqual(response.Schema.Id, default(Guid), "Schema ID should be a non-default GUID");
            Assert.AreNotEqual(response.TransactionId, default(Guid), "Transaction ID should be a non-default GUID");
            Assert.AreEqual(response.Result, "success", "Response should indicate success");
            return response;
        }

        [Test]
        public void ExistingDocumentCanBeEdited()
        {
            Person person = CreatePerson();
            DocumentSaveSuccessResponse createResponse = AssertCreatePersonSuccess(person, testVaultId);
            documentSuccessResponses.Push(createResponse);

            person.Name += " Justgotmarried";
            person.Email = string.Format("{0}@truevaulttest.net", Guid.NewGuid());

            TrueVaultResponse editResponse = trueVaultClient.UpdateDocument(testVaultId, createResponse.DocumentId,
                person);
            Assert.AreEqual(editResponse.Result, "success", "Edit response should indicate success");
            Assert.AreNotEqual(editResponse.TransactionId, default(Guid),
                "Edit response Transaction ID should be a non-default GUID");

            var retrievedPerson = trueVaultClient.GetDocument<Person>(testVaultId, createResponse.DocumentId);

            Assert.AreEqual(person.Id, retrievedPerson.Id);
            Assert.AreEqual(person.Name, retrievedPerson.Name);
            Assert.AreEqual(person.Email, retrievedPerson.Email);
        }

        [Test]
        public void ExistingDocumentCanBeRetrieved()
        {
            Person person = CreatePerson();
            DocumentSaveSuccessResponse createResponse = AssertCreatePersonSuccess(person, testVaultId);
            documentSuccessResponses.Push(createResponse);

            var retrievedPerson = trueVaultClient.GetDocument<Person>(testVaultId, createResponse.DocumentId);

            Assert.AreEqual(person.Id, retrievedPerson.Id);
            Assert.AreEqual(person.Name, retrievedPerson.Name);
            Assert.AreEqual(person.Email, retrievedPerson.Email);
        }

        [Test]
        public void MultipleDocumentsCanBeRetrieved()
        {
            const int personCount = 3;
            List<Person> people = CreateMultiplePersons(personCount);

            var createResponses = new Dictionary<Guid, Person>();

            people.ForEach(person =>
            {
                DocumentSaveSuccessResponse createResponse = AssertCreatePersonSuccess(person, testVaultId);
                documentSuccessResponses.Push(createResponse);
                createResponses.Add(createResponse.DocumentId, person);
            });

            MultiDocumentResponse multipleRetrievedDocuments = trueVaultClient.MultiGetDocuments(testVaultId,
                createResponses.Select(c => c.Key).ToArray());
            Assert.AreEqual(personCount, multipleRetrievedDocuments.Documents.Count());

            IEnumerable<DocumentResponse> retrievedPeopleDocs = multipleRetrievedDocuments.Documents;

            foreach (DocumentResponse doc in retrievedPeopleDocs)
            {
                var deserializedPerson = doc.DeserializeDocument<Person>();
                Assert.IsTrue(createResponses.ContainsKey(doc.Id));
                Assert.AreEqual(createResponses[doc.Id].Id, deserializedPerson.Id);
                Assert.AreEqual(createResponses[doc.Id].Name, deserializedPerson.Name);
                Assert.AreEqual(createResponses[doc.Id].Email, deserializedPerson.Email);
            }
        }

        [Test]
        public void NewDocumentCanBeCreated()
        {
            Person person = CreatePerson();
            documentSuccessResponses.Push(AssertCreatePersonSuccess(person, testVaultId));
        }

        [Test]
        public void SchemaCanBeCreated()
        {
            Schema personSchema = CreatePersonSchema("Person" + Guid.NewGuid());
            schemaSuccessResponses.Push(AssertCreateSchemaSuccess(personSchema, testVaultId));
        }

        [Test]
        public void SchemaCanBeDeleted()
        {
            string schemaName = "DeleteMe" + Guid.NewGuid();
            Schema deleteSchema = CreatePersonSchema(schemaName);
            SchemaSaveSuccessResponse response = trueVaultClient.CreateSchema(testVaultId, deleteSchema);

            IEnumerable<Schema> allSchemas = trueVaultClient.GetSchemaList(testVaultId);
            Assert.IsTrue(allSchemas.Any(s => s.Name == schemaName));

            trueVaultClient.DeleteSchema(testVaultId, response.Schema.Id);

            allSchemas = trueVaultClient.GetSchemaList(testVaultId);
            Assert.IsFalse(allSchemas.Any(s => s.Name == schemaName));
        }
    }
}