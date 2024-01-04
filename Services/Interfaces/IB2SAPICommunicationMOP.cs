using B2S_API_Comm.Domain;

namespace B2S_API_Comm.Services.Interfaces
{
    /// <summary>
    /// <br/>
    ///  IB2SAPICommunicationB2S_MOP Obj = new B2SAPICommunicationModule("URI");<br/><br/>
    /// <code>Obj.GetProductViaString("What They Search");</code>
    /// <code>
    /// Obj.GetProductAsync(
    ///     new <see cref="ProductRequest"/>()
    ///         Brand = "",
    ///         ItemGroup = "",
    ///         EAN = "",
    ///         ProductNumber = "",
    ///         Index = 0,
    ///         Amount = 50
    ///     }
    /// );
    /// </code>
    /// </summary>
    [Obsolete]
    public interface IB2SAPICommunicationMOP
    {
        /// <summary>
        /// Gets preferably a single <see cref="Product"/> in a 
        /// <see cref="List{Product}"/>, otherwise the most likely in order of most likely
        /// </summary>
        /// <param name="searchString"><see cref="string"/> which is the search criteria</param>
        /// <returns><see cref="List{Product}"/> of <see cref="Product"/> (Decending on how likely it is to be the one)</returns>
        [Obsolete]
        Task<IEnumerable<Product>?> GetProductViaString(string searchString);
        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="Product"/> which fit with the request information
        /// </summary>
        /// <param name="request"><see cref="ProductRequest"/> containing the filter information</param>
        /// <returns><see cref="List{Product}"/> of <see cref="Product"/> based on the request</returns>
        [Obsolete]
        Task<IEnumerable<Product>?> GetProductsAsync(ProductRequest request);
    }
}
