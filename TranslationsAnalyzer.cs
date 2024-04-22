using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class TranslationsAnalyzer
{
    private readonly string _rootDirectory;

    public TranslationsAnalyzer(string rootDirectory)
    {
        _rootDirectory = rootDirectory;
    }

    public IEnumerable<TranslationId> GetAllTranslationIds()
    {
        var files = Directory.GetFiles(_rootDirectory, "*.cs", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(file));
            var root = syntaxTree.GetRoot();

            foreach (var translation in root.DescendantNodes().OfType<InvocationExpressionSyntax>())
            {
                var memberAccess = translation.Expression as MemberAccessExpressionSyntax;
                if (memberAccess == null || memberAccess.Name.Identifier.Text != "GetTranslation")
                    continue;

                var identifier = memberAccess.Expression as IdentifierNameSyntax;
                if (identifier == null || identifier.Identifier.Text != "_translationService")
                    continue;

                var argument = translation.ArgumentList.Arguments.FirstOrDefault();
                if (argument != null)
                {
                    var text = argument.Expression.ToString();
                    var line = syntaxTree.GetLineSpan(translation.Span).StartLinePosition.Line + 1;
                    string relativeFilePath = file.Replace(_rootDirectory, "");

                    yield return new TranslationId
                    {
                        RelativeFilePath = relativeFilePath,
                        Line = line,
                        Text = text
                    };
                }
            }
        }
    }
}