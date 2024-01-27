using Microsoft.Extensions.Azure;
using Microsoft.SemanticKernel.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace LLMWebApi.Services 
{
    public class DocumentService 
    {
        public string folderPath;

        public DocumentService(string folderPath)
        {
            this.folderPath = folderPath;
        }

        public List<Dictionary<string, object>> ExtractFromDocumentDir()
        {
            if (Directory.Exists(this.folderPath))
            {
                string[] files = Directory.GetFiles(this.folderPath, "*.pdf");
                List<Dictionary<string, object>> documents = [];

                foreach (var file in files)
                {
                    documents.Add(ExtractText(file));
                }

                return documents;
            }

            return [];
        }

        public void GetMetaData(PdfDocument document)
        {
                string producer = document.Information.Producer;
                string title = document.Information.Title;
                string keywords = document.Information.Keywords;

                if(title == null) 
                {
                    Console.WriteLine("No title found for the document");
                }
                else 
                {
                    Console.WriteLine($"title: {title}");
                }

                if(title == null) 
                {
                    Console.WriteLine("No keywords found for the document");
                }
                else 
                {
                    Console.WriteLine($"keywords: {keywords}");
                }
        }

        public Dictionary<string, object> ExtractText(string filePath)
        {
            Console.WriteLine($"Processing {filePath}...");

            using PdfDocument document = PdfDocument.Open(filePath);
            int pageCount = document.NumberOfPages;

            Dictionary<string, object> doc = [];
            doc.Add("pdfName", Path.GetFileName(filePath));
            doc.Add("noOfPages", pageCount);

            Console.WriteLine($"document: {Path.GetFileName(filePath)}");

            doc.Add("content", new List<Dictionary<string, object>>());

            foreach (var page in document.GetPages())
            {
                Console.WriteLine($"Processing page {page.Number} out of {pageCount}");

                var pageContent = new Dictionary<string, object>();

                var pageText = ContentOrderTextExtractor.GetText(page);
                var paragraphs = new List<string>();

                // Break down long page content to small chunks to ensure it's within Token Limit of the OPENAI model
                if (pageText.Length > 2048)
                {
#pragma warning disable SKEXP0055 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                    var lines = TextChunker
                    .SplitPlainTextLines(pageText, 128);
                    paragraphs = TextChunker.SplitPlainTextParagraphs(lines, 1024);
#pragma warning restore SKEXP0055 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                }
                else
                {
                    paragraphs.Add(pageText);
                }

                pageContent.Add("pageNo", page.Number);
                pageContent.Add("paragraphs", paragraphs);

                Console.WriteLine($"page {page.Number} has {paragraphs.Count} paragraphs");

                foreach (var paragraph in paragraphs)
                {
                    var paragraphId = paragraphs.IndexOf(paragraph);
                    Console.WriteLine($"Processing paragraph {paragraphId}");
                }
                List<Dictionary<string, object>> docContent = (List<Dictionary<string, object>>)doc["content"];
                docContent.Add(pageContent);
            }

            return doc;
        }
    }
}