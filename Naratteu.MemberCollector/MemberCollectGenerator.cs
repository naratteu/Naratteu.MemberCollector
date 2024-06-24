using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using Microsoft.CodeAnalysis.Shared.Extensions;

namespace Naratteu.MemberCollector;

[Generator]
public class MemberCollectGenerator : IIncrementalGenerator
{
    void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            ctx.AddSource("MemberCollectAttribute.g.cs", SourceText.From("""
                namespace Naratteu.MemberCollector
                {
                    [System.AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
                    public class MemberCollectAttribute : System.Attribute { }
                }
                """, Encoding.UTF8));
        });

        var attr = context.SyntaxProvider.ForAttributeWithMetadataName("Naratteu.MemberCollector.MemberCollectAttribute", (_, _) => true, (c, _) => c);
        context.RegisterSourceOutput(attr.Collect(),
            (spc, sources) =>
            {
                foreach (var g in sources.GroupBy(s => s.TargetSymbol.ContainingType.ToDisplayString()))
                {
                    var type = g.First().TargetSymbol.ContainingType;
                    spc.AddSource($"{g.Key}.g.cs", SourceText.From($$"""
                        namespace {{type.ContainingNamespace}}
                        {
                            {{TypeBlock(type, $$"""System.Collections.IEnumerable GetMemberCollection() { {{string.Join(" ", g.Select(s => $"yield return {s.TargetSymbol.Name};"))}} }""")}}
                        }
                        """, Encoding.UTF8));
                }
            });

        static string TypeBlock(INamedTypeSymbol type, string inner)
        {
            var curr = $$"""partial class {{type.Name}} { {{inner}} }""";
            return type.ContainingType is { } t ? TypeBlock(t, curr) : curr;
        }
    }
}