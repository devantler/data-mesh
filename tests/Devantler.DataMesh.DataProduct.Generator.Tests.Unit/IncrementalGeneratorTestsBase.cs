using System.Collections.Immutable;
using Devantler.DataMesh.DataProduct.Generator.IncrementalGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Devantler.DataMesh.DataProduct.Generator.Tests.Unit;

public abstract class IncrementalGeneratorTestsBase<T> where T : GeneratorBase, new()
{
    readonly CSharpCompilation _compilation;
    readonly CSharpGeneratorDriver _driver;

    protected IncrementalGeneratorTestsBase()
    {
        List<PortableExecutableReference> references = LoadAssemblyReferences();
        _compilation = CSharpCompilation.Create(
            "Devantler.DataMesh.DataProduct.Generator.Tests.Unit",
            references: references
        );

        _driver = CSharpGeneratorDriver.Create(new T());
    }

    public SettingsTask Verify(CustomAdditionalText additionalText)
    {
        if (additionalText is null)
            throw new ArgumentNullException(nameof(additionalText));
        ImmutableArray<AdditionalText> additionalTexts = ImmutableArray.Create<AdditionalText>(additionalText);
        GeneratorDriver driver = _driver.AddAdditionalTexts(additionalTexts)
            .RunGenerators(_compilation);
        string directoryName = GetTestDirectoryName();
        return Verifier.Verify(driver).UseDirectory(directoryName).DisableRequireUniquePrefix();
    }

    string GetTestDirectoryName()
    {
        int indexOfDirectoryNameInNamespace = GetType()?.Namespace?.LastIndexOf('.') + 1 ?? 0;
        return GetType()?.Namespace?[indexOfDirectoryNameInNamespace..] ?? string.Empty;
    }

    static List<PortableExecutableReference> LoadAssemblyReferences()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .Select(a => a.Location)
            .Where(s => s.Contains("Devantler.DataMesh.DataProduct.dll", StringComparison.Ordinal))
            .Select(s => MetadataReference.CreateFromFile(s))
            .ToList();
    }
}

public class CustomAdditionalText : AdditionalText
{
    public override string Path { get; } = string.Empty;

    readonly string _text;

    public CustomAdditionalText(string path, string text)
    {
        Path = path;
        _text = text;
    }

    public override SourceText? GetText(CancellationToken cancellationToken = default) => SourceText.From(_text);
}
