using Avro;
using Devantler.Commons.StringHelpers;
using Devantler.DataMesh.DataProduct.Configuration.SchemaRegistry;

namespace Devantler.DataMesh.SchemaRegistry;

/// <summary>
/// A Local schema registry service.
/// </summary>
public class LocalSchemaRegistryService : ISchemaRegistryService
{
    readonly LocalSchemaRegistryOptions _schemaRegistryOptions;

    /// <summary>
    /// A constructor to construct a Local schema registry service.
    /// </summary>
    /// <param name="schemaRegistryOptions"></param>
    public LocalSchemaRegistryService(LocalSchemaRegistryOptions schemaRegistryOptions) => _schemaRegistryOptions = schemaRegistryOptions;

    /// <inheritdoc/>
    public async Task<Schema> GetSchemaAsync(string subject, int version)
    {
        if (_schemaRegistryOptions.Path == null)
            throw new InvalidOperationException("Schema registry path not set");

        string schemaFileName = $"{subject.ToKebabCase()}-v{version}.avsc";

        string[] schemaFile = Directory.GetFiles(_schemaRegistryOptions.Path, schemaFileName);

        if (schemaFile.Length == 0)
            throw new FileNotFoundException($"Schema file not found for {Directory.GetCurrentDirectory()}/{schemaFileName}");

        string schemaString = await Task.Run(() => File.ReadAllText(schemaFile[0]));

        return Schema.Parse(schemaString);
    }
}
