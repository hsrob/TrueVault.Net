using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using NUnit.Framework;
using TrueVault.Net.Models;
using TrueVault.Net.Test.TestModels;

namespace TrueVault.Net.Test
{
    [TestFixture]
    public class TrueVaultClientTest
    {
        private TrueVaultClient trueVaultClient;
        private Guid testVaultId;
        private ConcurrentStack<DocumentSuccessResponse> successResponses;

        [TestFixtureSetUp]
        public void Setup()
        {
            trueVaultClient = new TrueVaultClient(ConfigurationManager.AppSettings["TrueVaultApiKey"]);
            successResponses = new ConcurrentStack<DocumentSuccessResponse>();
            testVaultId = Guid.Parse(ConfigurationManager.AppSettings["TrueVaultTestVault"]);
        }

        [Test]
        public void NewDocumentCanBeCreated()
        {
            var person = CreatePerson();
            successResponses.Push(AssertCreatePersonSuccess(person, testVaultId));
        }

        [Test]
        public void ExistingDocumentCanBeRetrieved()
        {
            var person = CreatePerson();
            var createResponse = AssertCreatePersonSuccess(person, testVaultId);
            successResponses.Push(createResponse);

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
            successResponses.Push(createResponse);

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
                successResponses.Push(createResponse);
                createResponses.Add(createResponse.DocumentId, person);
            });

            var multipleRetrievedDocuments = trueVaultClient.MultiGetDocuments<Person>(testVaultId,
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

        [TestFixtureTearDown]
        public void Teardown()
        {
            DocumentSuccessResponse documentSuccessResponse;
            while (successResponses.TryPop(out documentSuccessResponse))
            {
                trueVaultClient.DeleteDocument(testVaultId, documentSuccessResponse.DocumentId);
            }
        }

        #region Helpers

        private DocumentSuccessResponse AssertCreatePersonSuccess(Person person, Guid vaultId)
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

        #endregion
    }
}