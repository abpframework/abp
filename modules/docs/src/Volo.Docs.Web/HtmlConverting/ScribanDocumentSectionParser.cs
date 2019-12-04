using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Scriban;

namespace Volo.Docs.HtmlConverting
{
    public class ScribanDocumentSectionRenderer : IDocumentSectionRenderer
    {
        public async Task<string> Render(string document, DocumentRenderParameters parameters = null)
        {
            Template scribanTemplate;
            if (parameters == null)
            {
                scribanTemplate = Template.Parse(document);
                return scribanTemplate.Render();
            }

            var p2 = new Dictionary<string, string>();

            foreach (var item in parameters)
            {
                p2.Add(item.Key, item.Value);
            }

            scribanTemplate = Template.Parse(document);
            var result = scribanTemplate.Render(p2);

            try
            {
                return await RemoveOptionsJson(result);
            }
            catch (Exception)
            {
                return scribanTemplate.Render();
            }
        }

        public async Task<Dictionary<string, List<string>>> GetAvailableParametersAsync(string document)
        {
            try
            {
                var jsonOpener = "````json";
                var jsonCloser = "````";
                var docs_param = "//[doc-params]";

                if (!document.Contains(jsonOpener))
                {
                    return new Dictionary<string, List<string>>();
                }

                var searchedIndex = 0;
                while (searchedIndex < document.Length)
                {
                    var jsonBeginningIndex = document.Substring(searchedIndex).IndexOf(jsonOpener) + jsonOpener.Length + searchedIndex;
                    var JsonEndingIndex = document.Substring(jsonBeginningIndex).IndexOf(jsonCloser) + jsonBeginningIndex;
                    var insideJsonSection = document[jsonBeginningIndex..JsonEndingIndex];

                    if (insideJsonSection.IndexOf(docs_param) < 0)
                    {
                        searchedIndex = JsonEndingIndex + jsonCloser.Length;
                        continue;
                    }

                    var pureJson = insideJsonSection.Replace(docs_param, "").Trim();

                    return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(pureJson);
                }

                return new Dictionary<string, List<string>>();
            }
            catch (Exception e)
            {
                //log
                return new Dictionary<string, List<string>>();
            }
        }

        private async Task<string> RemoveOptionsJson(string document) 
        {
            var jsonOpener = "````json";
            var jsonCloser = "````";
            var docs_param = "//[doc-params]";

            var searchedIndex = 0;
            while (searchedIndex < document.Length)
            {
                var jsonBeginningIndex = document.Substring(searchedIndex).IndexOf(jsonOpener) + jsonOpener.Length + searchedIndex;

                if (jsonBeginningIndex < 0)
                {
                    return document;
                }

                var JsonEndingIndex = document.Substring(jsonBeginningIndex).IndexOf(jsonCloser) + jsonBeginningIndex;
                var insideJsonSection = document[jsonBeginningIndex..JsonEndingIndex];

                if (insideJsonSection.IndexOf(docs_param) < 0)
                {
                    searchedIndex = JsonEndingIndex + jsonCloser.Length;
                    continue;
                }

                return document.Remove(
                    jsonBeginningIndex - jsonOpener.Length, (JsonEndingIndex + jsonCloser.Length) - (jsonBeginningIndex - jsonOpener.Length)
                    );

            }

            return document;
        }
    }
}
