using System.Text;
using Devantler.DataMesh.DataProduct.Configuration;
using Devantler.DataMesh.DataProduct.SourceGenerator.Core;
using Devantler.DataMesh.DataProduct.SourceGenerator.Core.Extensions;
using Devantler.DataMesh.DataProduct.SourceGenerator.Core.Parsers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Configuration;

namespace Devantler.DataMesh.DataProduct.SourceGenerator.Generators;

[Generator]
public class ModelsGenerator : AppSettingsGenerator
{
    protected override void Generate(SourceProductionContext context, Compilation compilation, IConfiguration configuration)
    {
        var schemas = configuration.GetSection("Schemas").Get<Schema[]>();

        var @namespace = NamespaceResolver.Resolve(compilation.GlobalNamespace, "IModel");

        foreach (var schema in schemas)
        {
            var @class = schema.Name.ToPascalCase();
            var source =
            $$"""
            namespace {{@namespace}};

            public class {{@class}} : IModel
            {
                public Guid Id { get; set; }
                {{PropertyParser.Parse(schema.Properties).IndentBy(4)}}    
            }

            """;

            context.AddSource($"{@class}.cs", SourceText.From(source.AddMetadata(), Encoding.UTF8));
        }
    }
}