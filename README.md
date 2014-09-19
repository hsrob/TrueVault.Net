TrueVault.Net
=============

.NET wrapper for the TrueVault API

###What is TrueVault, and what is this library?

TrueVault is a Secure API to store health data. See their website at https://www.truevault.com for more information.

This library is a simple, strongly-typed .NET wrapper around TrueVault's RESTful API.

###Features (:white_check_mark: Completed and :heavy_minus_sign: Planned)

- :white_check_mark: JSON Document Store API
- :heavy_minus_sign: BLOB Store API
- :white_check_mark: Search Schema management API (Get/Create/Update/Delete Schemas)
  - :heavy_minus_sign: Search API

###External Dependencies

> - ServiceStack.Text 3.9.71
> - AutoMapper 3.2.1
> - NBuilder 3.0.1.1
> - NUnit 2.6.3

##Release Notes

###Version 1.0.2
- Implement Search Schema management API

#Usage

##Instantiation

Create a new instance of the TrueVault client, passing in your TrueVault API key, and you're ready to go!

```csharp
var trueVaultClient = new TrueVaultClient("{TrueVault API Key}");
```

##JSON Document Store API

###Creating a  Document

This library automatically handles Base64 conversion and serialization. You only need to pass an instance of Type `T` to save it to TrueVault. `T` can be any class with a parameterless constructor.

A document can be created with or without the optional `schemaId` parameter via the appropriate overloaded `CreateDocument<T>` method.

###Getting Document(s)

`GetDocument<T>` is used to retrieve a single document from TrueVault, and will directly return an instance of Type `T`. Base64 conversion and deserialization is handled for you.

`MultiGetDocuments<T>` retrieves a `MultiDocumentResponse`, which contains a list of `DocumentResponse` in its `Documents` property. You can use the `DeserializeDocuments<T>` method to extract and return the wrapped document instances as Type `T`.

Each `DocumentResponse` exposes a `DeserializeDocument<T>` method, which extracts and returns the individual document as Type `T`. `DocumentResponse` contains the TrueVault document ID in its `Id` property, as well as the raw serialized Base64 encoded JSON string in its `Document` property.

###Creating a Schema

Create a new instance of `Schema` or `Schema<T>`. Note that even though it has a public setter, **you should never modify the `Id` property of a Schema yourself**.

You can pass new instances of `SchemaField` via the `Schema`/`Schema<T>` constructor, or simply add them to the `Fields` List after construction. Check the Intellisense notes for further details about the various properties.

`Schema<T>` is derived from `Schema`, but it isn't necessary to use this generic version. It simply adds a convenience method, `RegisterNestedField<TNested>`, which accepts `fieldExpression`, which is a `MemberExpression` pointing to a complex property on `T`, and `nestedFieldExpression`, which is a `MemberExpression` pointing to a primitive property on `TNested`. This method will also attempt to determine the TrueVault Schema compatible type of the property on `TNested`. See the Intellisense notes and Unit Tests for further details.

When your `Schema` is ready, call `TrueVaultClient.CreateSchema` to save it, which will return a `SchemaSaveSuccessResponse`, containing a `Schema` property that should be the same as the `Schema` you saved.

###Getting Schema(s)

Call `TrueVaultClient.GetSchema` and `TrueVaultClient.GetSchemaList` to get one or all `Schema`s, respectively. The responses will contain a `Schema` or `IEnumerable<Schema>`, respectively.

###Exception Handling

When the library catches a `WebException`, it will make an attempt to parse and unwrap the `error` object in an error response from TrueVault. It will throw a new `WebException` with the original exception in the `InnerException` property, and a message formatted as follows, where `response` is the error response body returned by TrueVault.

`TrueVault Transaction ID {response.transaction_id} - {HTTP Status Code} Error (Type: {response.error.type}) [Code: {response.error.code}]: {response.error.message}`

##Running the Tests

Update App.config in the TrueVault.Net.Test project with the values you wish to use for testing. Please note that the tests use the TrueVault API, so you may incur a small amount of usage by running them. Per the license, you agree that I cannot be held responsible for any usage charges, fees, overages, etc. you may incur by running the tests.

#Contributing

Pull requests are welcome. If you're planning on adding a major feature, please contact me first to make sure it fits with the direction of the library, and that I'm not already working on it!

#Legal

This library is made available under The MIT License. By using this library, you agree to the terms of that license.
