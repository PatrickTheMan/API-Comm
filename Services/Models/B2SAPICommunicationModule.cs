using B2S_API_Comm.Domain;
using B2S_API_Comm.Services.Interfaces;
using Models.Handlers;

namespace B2S_API_Comm.Services.Models
{
    public class B2SAPICommunicationModule : IB2SAPICommunicationOCR, IB2SAPICommunicationMOP
    {
        B2SHttpClientHandler Handler { get; set; }

        [Obsolete]
        public B2SAPICommunicationModule(string serverURI) 
        {
            Handler = new B2SHttpClientHandler(serverURI);
        }

        [Obsolete]
        public async Task<IEnumerable<Product>?> GetProductsAsync(ProductRequest request)
        {
             return await Handler.GetProductsAsync(request);
        }

        [Obsolete]
        public async Task<IEnumerable<Product>?> GetProductViaOCR(IEnumerable<KeyValuePair<string, int>> ocrKeyValuePairs)
        {
			List<string> words = new();
			foreach (var item in ocrKeyValuePairs)
			{
				words.Add(item.Key);
			}
			return await Handler.GetProductsAsync(words);
        }

        [Obsolete]
        public async Task<IEnumerable<Product>?> GetProductViaString(string searchString)
        {
            return await Handler.GetProductsAsync(searchString.Split(' '));
        }

        [Obsolete]
        public async Task<string> GetProductsJsonString(IEnumerable<KeyValuePair<string, int>> ocrKeyValuePairs, bool formatted)
        {
            List<string> words = new();
            foreach (var item in ocrKeyValuePairs)
            {
                words.Add(item.Key);
            }
            return await Handler.GetProductsJsonString(words, formatted);
        }

        [Obsolete]
        public async Task<string> GetProductsJsonString(ProductRequest request, bool formatted)
        {
            return await Handler.GetProductsJsonString(request, formatted);
        }
    }
}
