using B2S_API_Comm.Domain;
using B2S_API_Comm.Models;
using System.Text.Json;

namespace Models.Handlers
{
    public partial class B2SSpecificationHandler
    {
        #region Get
        /// <summary>
        /// Gets the specifications of a provided <see cref="Product"/>
        /// </summary>
        /// <param name="product">The provided Product</param>
        /// <returns>A list of specification <see cref="KeyValuePair"/></returns>
        public List<KeyValuePair<string, string>> GetSpecifications(Product product)
        {
            List<KeyValuePair<string, string>>? tempSpecList = new();
            if (product.SpecJson != null && product.SpecJson.Length > 0)
            {
                tempSpecList = JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(product.SpecJson);
            }
            return tempSpecList;
        }
        #endregion
        #region Checks
        /// <summary>
        /// Checks wether a provided <see cref="Product"/> has a specification with the provided name
        /// </summary>
        /// <param name="product">The provided Product</param>
        /// <param name="specificaitonName">The provided name</param>
        /// <returns>Wether it did or not</returns>
        public bool HasSpecification(Product product, string specificaitonName)
        {
            List<KeyValuePair<string, string>>? tempSpecList = new();
            if (product.SpecJson != null && product.SpecJson.Length > 0)
            {
                tempSpecList = JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(product.SpecJson);// Get current Spec
            }

            return tempSpecList.Any(pair =>
            {
                Console.WriteLine(pair.Key.ToLower() + " / " + specificaitonName.ToLower());

                return pair.Key.ToLower().Equals(specificaitonName.ToLower());
            });
        }
        #endregion
        #region Edit Specifications
        /// <summary>
        /// Adds a specification to the provided <see cref="Product"/> current specification list
        /// </summary>
        /// <param name="product">The provided Product</param>
        /// <param name="newPair">The new specification to add, provided as a <see cref="KeyValuePair"/></param>
        /// <returns>Wether or not adding the specification was a succes</returns>
        public bool AddSpecification(ref Product product, KeyValuePair<string, string> newPair)
        {
            List<KeyValuePair<string, string>>? tempSpecList = new();
            try
            {
                if (product.SpecJson != null && product.SpecJson.Length > 0)
                {
                    tempSpecList = JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(product.SpecJson);// Get current Spec
                }
                tempSpecList ??= new();
                tempSpecList.Add(newPair);
                product.SpecJson = JsonSerializer.Serialize(tempSpecList); // Actual Update
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Removes a specification from the provided <see cref="Product"/>'s specifications via a provided name
        /// </summary>
        /// <param name="product">The provided Product</param>
        /// <param name="name">The provided name</param>
        /// <returns>Wether or not removing the specification was a succes</returns>
        public bool RemoveSpecification(ref Product product, string name)
        {
            List<KeyValuePair<string, string>>? tempSpecList = new();
            try
            {
                if (product.SpecJson != null)
                {
                    tempSpecList = JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(product.SpecJson); // Get current Spec
                }
                foreach (KeyValuePair<string, string> pair in tempSpecList)
                {
                    if (pair.Key.Equals(name))
                    {
                        tempSpecList.Remove(pair);
                        product.SpecJson = JsonSerializer.Serialize(tempSpecList); // Actual Update
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Sets the specifications of a provided <see cref="Product"/> to the provided list of <see cref="KeyValuePair"/>s
        /// </summary>
        /// <param name="product">The provided Product</param>
        /// <param name="pairs">The provided list of KeyValuePairs</param>
        /// <returns>Wether or not setting the specifications was a succes</returns>
        public bool SetSpecifications(ref Product product, List<KeyValuePair<string, string>> pairs)
        {
            try
            {
                product.SpecJson = JsonSerializer.Serialize(pairs);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
