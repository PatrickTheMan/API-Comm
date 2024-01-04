using B2S_API_Comm.Domain;

namespace B2S_API_Comm.Services.Interfaces
{
    /// <summary>
    /// <br/>
    /// IB2SAPICommunicationOCR Obj = new B2SAPICommunicationModule("URI");<br/><br/>
    /// KeyValuePair of (Words / Occurrence)<br/>
    /// <code>Obj.GetProductViaOCR(<see cref="IEnumerable{KeyValuePair{string, int}}"/>);</code>
    /// </summary>
    [Obsolete]
    public interface IB2SAPICommunicationOCR
    {
        /// <summary>
        /// Gets preferably a single <see cref="Product"/> in a 
        /// <see cref="List{Product}"/>, otherwise the most likely in order of most likely
        /// </summary>
        /// <param name="ocrKeyValuePairs"><see cref="List{T}"/> with all the words and their occurrencess</param>
        /// <returns><see cref="List{Product}"/> of <see cref="Product"/> (Decending on how likely it is to be the one)</returns>
        [Obsolete]
        Task<IEnumerable<Product>?> GetProductViaOCR(IEnumerable<KeyValuePair<string, int>> ocrKeyValuePairs);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ocrKeyValuePairs"></param>
        /// <param name="formatted"></param>
        /// <returns></returns>
        [Obsolete]
        Task<string> GetProductsJsonString(IEnumerable<KeyValuePair<string, int>> ocrKeyValuePairs, bool formatted = true);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="formatted"></param>
        /// <returns></returns>
        [Obsolete]
        Task<string> GetProductsJsonString(ProductRequest request, bool formatted = true);
    }
}
