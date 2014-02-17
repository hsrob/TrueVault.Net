using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
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
		private TrueVaultClient trueVaultPersonClient;
		private ConcurrentQueue<DocumentSuccessResponse> successResponses;

		[TestFixtureSetUp]
		public void Setup()
		{
			trueVaultPersonClient = new TrueVaultClient(ConfigurationManager.AppSettings["TrueVaultApiKey"]);
			successResponses = new ConcurrentQueue<DocumentSuccessResponse>();
		}

		[Test]
		public void NewDocumentCanBeCreated()
		{
			var person = Builder<Person>.CreateNew()
				.With(p => p.Email = string.Format("{0}@truevaulttest.com", Guid.NewGuid().ToString()))
				.Build();

			var response = trueVaultPersonClient.CreateDocument(Guid.Parse(ConfigurationManager.AppSettings["TrueVaultTestVault"]), person);
			Debug.WriteLine("CreateDocument Response: {0}", response);
			Assert.IsNotNull(response, "Response should not be null");
			Assert.AreNotEqual(response.DocumentId, default(Guid), "Document ID should be a non-default GUID");
			Assert.AreNotEqual(response.TransactionId, default(Guid), "Transaction ID should be a non-default GUID");
			Assert.AreEqual(response.Result, "success", "Response should indicate success");

			successResponses.Enqueue(response);
		}

		[TestFixtureTearDown]
		public void Teardown()
		{
			DocumentSuccessResponse documentSuccessResponse;
			while (successResponses.TryDequeue(out documentSuccessResponse))
			{
				
			}
		}
    }
}
