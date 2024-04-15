using BackingShop.Tests.Common.Abstractions;

namespace BackingShop.Tests.Product.Abstractions;

[CollectionDefinition(nameof(IntegrationTestCollection))]
public sealed class IntegrationTestCollection : ICollectionFixture<IntegrationTestWebAppFactory>;
