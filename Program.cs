using System;

class Program
{
    static void Main(string[] args)
    {
        
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: dotnet run <Program Root Directory>");
            return;
        }

        string rootDirectory = args[0];
        
     //   string  rootDirectory = @"/Users/jairomora/Downloads/Translator/";

        var analyzer = new TranslationsAnalyzer(rootDirectory);
        var translationIds = analyzer.GetAllTranslationIds();

        foreach (var translationId in translationIds)
        {
            Console.WriteLine($"File: {translationId.RelativeFilePath}, Line: {translationId.Line}, Text: {translationId.Text}");
        }
        //TODO:SEND TO API
        //TODO:Receive from API and create translation file
        
    }
}