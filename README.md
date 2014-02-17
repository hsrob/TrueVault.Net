truevault-net
=============

.NET wrapper for the TrueVault API

###What is TrueVault, and what is this library?

TrueVault is a Secure API to store health data. See their website at https://www.truevault.com for more information.

This library is a simple, strongly-typed .NET wrapper around TrueVault's RESTful API.

###Features (Completed and Planned)

- [x] JSON Document Store API
- [ ] BLOB Store API
- [ ] Search Schema API

###External Dependencies

> **Client Library**

> - ServiceStack.Text 3.9.71
> - AutoMapper 3.1.1

> **Test Project**

> - NBuilder 3.0.1.1
> - NUnit 2.6.3

#Usage

##Instantiation

Create a new instance of the TrueVault client, passing in your TrueVault API key, and you're ready to go!

```csharp
var trueVaultClient = new TrueVaultClient("{TrueVault API Key}");
```

##JSON Document Store API

###Creating a  Document

This library automatically handles Base64 conversion and serialization. You only need to pass an instance of Type `T` to save it to TrueVault. `T` can be any class with a parameterless constructor.

A document can be created with or without the optional `schemaId` parameter. While this library (currently) does not support creating search Schemas, you can still create one yourself, then pass its ID into the appropriate overloaded `CreateDocument<T>` method.

###Getting Document(s)

`GetDocument<T>` is used to retrieve a single document from TrueVault, and will directly return an instance of Type `T`. Base64 conversion and deserialization is handled for you.

`MultiGetDocuments<T>` retrieves a `MultiDocumentResponse`, which contains a list of `DocumentResponse` in its `Documents` property. You can use the `DeserializeDocuments<T>` method to extract and return the wrapped document instances as Type `T`.

Each `DocumentResponse` exposes a `DeserializeDocument<T>` method, which extracts and returns the individual document as Type `T`. `DocumentResponse` contains the TrueVault document ID in its `Id` property, as well as the raw serialized Base64 encoded JSON string in its `Document` property.

#Contributing

Pull requests are welcome. If you're planning on adding a major feature, please contact me first to make sure it fits with the direction of the library, and that I'm not already working on it!