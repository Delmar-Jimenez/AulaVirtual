using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CommentRemoverApp
{
    class CommentRemover : CSharpSyntaxRewriter
    {
        public override SyntaxTrivia VisitTrivia(SyntaxTrivia trivia)
        {
            if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) ||
                trivia.IsKind(SyntaxKind.MultiLineCommentTrivia) ||
                trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) ||
                trivia.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia))
            {
                return default;
            }
            return base.VisitTrivia(trivia);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string rootDir = @"C:\AulaVirtual";
            Console.WriteLine("Iniciando escaneo en: " + rootDir);
            
            var files = Directory.GetFiles(rootDir, "*.cs", SearchOption.AllDirectories)
                .Where(f => !f.Contains(@"\obj\") && !f.Contains(@"\bin\") && !f.Contains(@"\.git\") && !f.Contains(@"\CommentRemover\"));

            int count = 0;
            var remover = new CommentRemover();

            foreach (var file in files)
            {
                try
                {
                    string code = File.ReadAllText(file);
                    SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
                    SyntaxNode root = tree.GetRoot();
                    
                    SyntaxNode newRoot = remover.Visit(root);
                    
                    if (root != newRoot)
                    {
                        File.WriteAllText(file, newRoot.ToFullString());
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en archivo {file}: {ex.Message}");
                }
            }

            Console.WriteLine($"Se limpiaron los comentarios de {count} archivos.");
        }
    }
}
