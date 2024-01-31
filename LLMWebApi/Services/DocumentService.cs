using System.ComponentModel.DataAnnotations;
using LLMWebApi.Models;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.SemanticKernel.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace LLMWebApi.Services
{
    public class DocumentService
    {
        public Document? document;

        public string DataDir { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "data");
        public int DocumentLineSplitMaxTokens { get; set; } = 128;
        public int DocumentChunkMaxTokens { get; set; } = 1024;

        public DocumentService(Document document)
        {
            this.document = document;
        }

        public DocumentText? ExtractFromDocument()
        {
            if (this.document != null)
            {
                var documentDirs = Directory.GetDirectories(DataDir);

                foreach (var dir in documentDirs)
                {
                    Console.WriteLine($"searching {dir} directory in data");

                    var filePath = Path.Combine(DataDir, dir, this.document.FileName);

                    Console.WriteLine($"filepath: {filePath}");

                    if (File.Exists(filePath))
                    {
                        Console.WriteLine($"{this.document.FileName} exists in {dir} directory");
                        return ExtractText(filePath);
                    }

                    return null;
                }
            }

            return null;
        }

        public void GetMetaData(PdfDocument document)
        {
            string producer = document.Information.Producer;
            string title = document.Information.Title;
            string keywords = document.Information.Keywords;

            if (title == null)
            {
                Console.WriteLine("No title found for the document");
            }
            else
            {
                Console.WriteLine($"title: {title}");
            }

            if (title == null)
            {
                Console.WriteLine("No keywords found for the document");
            }
            else
            {
                Console.WriteLine($"keywords: {keywords}");
            }
        }

        public DocumentText ExtractText(string filePath)
        {
            Console.WriteLine($"Processing {filePath}...");

            using PdfDocument document = PdfDocument.Open(filePath);

            var documentText = new DocumentText
            {
                NoOfPages = document.NumberOfPages
            };

            Console.WriteLine($"document: {Path.GetFileName(filePath)}");

            // doc.Add("content", new List<Dictionary<string, object>>());

            foreach (var page in document.GetPages())
            {
                Console.WriteLine($"Processing page {page.Number} out of {document.NumberOfPages}");

                var pageContent = new Page
                {
                    PageNo = page.Number
                };

                var pageText = ContentOrderTextExtractor.GetText(page);
                var paragraphs = new List<string>();

                // Break down long page content to small chunks to ensure it's within Token Limit of the OPENAI model
                if (pageText.Length > 2048)
                {
#pragma warning disable SKEXP0055 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

                    var lines = TextChunker
                    .SplitPlainTextLines(pageText, DocumentLineSplitMaxTokens);
                    pageContent.Paragraphs = TextChunker.SplitPlainTextParagraphs(lines, DocumentChunkMaxTokens);

#pragma warning restore SKEXP0055 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                }
                else
                {
                    pageContent.Paragraphs.Add(pageText);
                }

                Console.WriteLine($"page {page.Number} has {pageContent.Paragraphs} paragraphs");

                foreach (var paragraph in pageContent.Paragraphs)
                {
                    var paragraphId = pageContent.Paragraphs.IndexOf(paragraph);
                    Console.WriteLine($"Processing paragraph {paragraphId}");
                }
                documentText.Content.Add(pageContent);
            }

            return documentText;
        }
    }
}