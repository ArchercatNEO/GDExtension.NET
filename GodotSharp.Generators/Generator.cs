using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

[Generator]
public partial class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {

        });
        
        var syntax = context.SyntaxProvider.CreateSyntaxProvider(SyntaxFilter, SyntaxMap);

        context.RegisterSourceOutput(syntax, (ctx, source) =>
        {

        });
    }

    public bool SyntaxFilter(SyntaxNode node, CancellationToken token)
    {
        if (node is not ClassDeclarationSyntax)
        {
            return false;
        }

        return true;
    }

    public bool SyntaxMap(GeneratorSyntaxContext context, CancellationToken token)
    {
        return true;
    }
}