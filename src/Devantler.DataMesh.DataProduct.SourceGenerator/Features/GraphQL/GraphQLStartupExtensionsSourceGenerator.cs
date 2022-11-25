using System;
using System.Linq;
using System.Text;
using Devantler.DataMesh.Core.Helpers;
using Devantler.DataMesh.DataProduct.SourceGenerator.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Devantler.DataMesh.DataProduct.SourceGenerator.Features.GraphQL;

[Generator]
public class GraphQLStartupExtensionsSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context) { }
    public void Execute(GeneratorExecutionContext context)
    {
        var config = ConfigurationDeserializer.Deserialize(context.AdditionalFiles.First().GetText()?.ToString());

        if (!config.Features.GraphQL) return;

        var @namespace = context.Compilation.AssemblyName + ".Features.GraphQL";
        var @class = GetType().Name[..^"SourceGenerator".Length];

        var source = string.Join(Environment.NewLine,
            "// <auto-generated />",
            $"using {@namespace}.Queries;",
            $"namespace {@namespace};",
            $"public static partial class {@class}",
            "{",
            $"{GenerateAddFromSourceGeneratorMethod()}",
            "}"
        );

        context.AddSource($"{@class}.cs", SourceText.From(source, Encoding.UTF8));
    }

    private object GenerateAddFromSourceGeneratorMethod()
    {
        return string.Join(Environment.NewLine,
            $"{GenerationHelpers.IndentBy(4)}static partial void AddFromSourceGenerator(IServiceCollection services){{",
            $"{GenerationHelpers.IndentBy(8)}services.AddGraphQLServer().AddQueryType<Query>();",
            $"{GenerationHelpers.IndentBy(4)}}}"
        );
    }
}
