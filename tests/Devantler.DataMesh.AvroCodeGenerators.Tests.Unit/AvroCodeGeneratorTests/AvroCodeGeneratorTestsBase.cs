using Avro;

namespace Devantler.DataMesh.AvroCodeGenerators.Tests.Unit.AvroCodeGeneratorTests;

public class AvroCodeGeneratorTestsBase
{
    internal readonly AvroCodeGenerator _avroCodeGenerator;

    public AvroCodeGeneratorTestsBase() => _avroCodeGenerator = new();

    internal Task Verify(string schemaPath)
    {
        //Arrange
        string schemaText = File.ReadAllText(schemaPath);
        Schema schema = Schema.Parse(schemaText);

        string @namespace = GetType().Name;

        //Act
        string code = _avroCodeGenerator.Generate(@namespace, schema);

        //Assert
        return Verifier.Verify(code).UseMethodName(schema.Name);
    }
}
