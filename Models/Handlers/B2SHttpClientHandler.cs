using System.Collections.Generic;
using System.Net.Http.Json;
using B2S_API_Comm.Domain;
using B2S_API_Comm.Models;
using Newtonsoft.Json;
#if DEBUG
using static LoggingTool.Logger;
#endif
namespace Models.Handlers
{
    public class B2SHttpClientHandler
    {
        #region Properties
        HttpClient HttpClient { get; set; }
        #endregion
        #region Constructor
        /// <summary>
        /// Handles all communication with the API
        /// </summary>
        /// <param name="apiURI">The API URI</param>
        public B2SHttpClientHandler(string apiURI)
        {
            HttpClientHandler Handler = new() {
                ClientCertificateOptions = ClientCertificateOption.Automatic,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
            };
            HttpClient = new HttpClient(Handler) { BaseAddress = new Uri(apiURI) };
        }
        #endregion
        #region Products
        #region Get
        /// <summary>
        /// Gets all products in the DB
        /// </summary>
        /// <returns>A list of all products</returns>
        public async Task<IEnumerable<Product>?> GetProductsAsync()
        {
            using HttpResponseMessage response = await HttpClient.GetAsync("api/Products");
#if DEBUG
            
            Log(await response.Content.ReadAsStringAsync());
#endif
            try
            {
                return JsonConvert.DeserializeObject<List<Product>>(await response.Content.ReadAsStringAsync());
			}
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Gets a product with the provided id
        /// </summary>
        /// <param name="id">The provided id</param>
        /// <returns>The product</returns>
        public async Task<Product?> GetProductAsync(int id)
        {
            using HttpResponseMessage response = await HttpClient.GetAsync($"api/Products/{id}");
#if DEBUG
            Log(await response.Content.ReadAsStringAsync());
#endif
            try
            {
				return JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync());
			}
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Gets a product based on an object and the search term
        /// </summary>
        /// <param name="productGetOptions">Type of object</param>
        /// <param name="searchString">The name of the object</param>
        /// <returns>A list of products</returns>
        public async Task<IEnumerable<Product>?> GetProductsAsync(ProductGetOptions productGetOptions, string searchString)
        {
            using HttpResponseMessage response = await HttpClient.GetAsync($"api/Products/{productGetOptions}/{searchString}");
#if DEBUG
            Log(await response.Content.ReadAsStringAsync());
#endif
            try
            {
				return JsonConvert.DeserializeObject<List<Product>>(await response.Content.ReadAsStringAsync());
			}
            catch
            {
                return null;
            }
        }
		/// <summary>
		/// Gets the result of a search via KeyValuePairs
		/// </summary>
		/// <param name="ocrWords">A list of words that occur</param>
		/// <returns>A list of products</returns>
		public async Task<IEnumerable<Product>?> GetProductsAsync(IEnumerable<string> ocrWords)
        {
            string ocrString = "";

            if (ocrWords.Count() == 0)
            {
                return null;
            }

            // Create ocr search string, with...
            // Key = OCR read word
            // Value = Occurrences of words
            foreach (string s in ocrWords)
            {
                ocrString += $"{HTMLSafeStringEncode(s)}¤";
            }
            ocrString = ocrString.Substring(0, ocrString.Length - 1); // Remove last '^'
#if DEBUG
            Log($"OCR string: ({ocrString})");
#endif
            if (ocrString.Length > 260)
                return new List<Product>() { new Product() { PrdId = -1, PrdProductText = $"Length of ocrstring is to long (proceeds 260 char)\n{ocrString}" } };

			using HttpResponseMessage response = await HttpClient.GetAsync($"api/Products/OCR/{ocrString}");
#if DEBUG
            Log(await response.Content.ReadAsStringAsync());
#endif
            try
            {
				return JsonConvert.DeserializeObject<List<Product>>(await response.Content.ReadAsStringAsync());
			}
            catch
            {
                try
                {
					return new List<Product>() { new Product() { PrdId = -1, PrdProductText = await response.Content.ReadAsStringAsync() + $"api/Products/OCR/{ocrString}" } };
				}
                catch
                {
					return null;
				}
            }
        }
        /// <summary>
        /// Gets the result of a search via KeyValuePairs
        /// </summary>
        /// <param name="ocrWords">The search words that occur</param>
        /// <param name="formatted">If the string should be formatted</param>
        /// <returns>A json string</returns>
        public async Task<string?> GetProductsJsonString(IEnumerable<string> ocrWords, bool formatted)
        {
            if (ocrWords == null)
            {
                return "No Search Words Found";
            }

            IEnumerable<Product> products = await GetProductsAsync(ocrWords);

            if (products == null)
				return "CRITICAL ERROR HAS OCCURED";
			if (products.Count() == 1 && products.First().PrdId == -1)
				return products.First().PrdProductText; // Return Error Text

			try
            {
				return formatted ? MakeJSNOReadable(JsonConvert.SerializeObject(products)) : JsonConvert.SerializeObject(products);
			}
            catch
            {
                return null;
            }
        }
		/// <summary>
		/// Gets the result of a request
		/// </summary>
		/// <param name="request">The search request</param>
		/// <returns>A list of products</returns>
		public async Task<IEnumerable<Product>?> GetProductsAsync(ProductRequest request)
		{
			string requestJson = JsonConvert.SerializeObject(request);
			using HttpResponseMessage response = await HttpClient.GetAsync($"api/Products/Request/{requestJson}");
#if DEBUG
            Log(await response.Content.ReadAsStringAsync());
#endif
			try
			{
				return JsonConvert.DeserializeObject<List<Product>>(await response.Content.ReadAsStringAsync());
			}
			catch
			{
				return null;
			}
		}
		/// <summary>
		/// Gets the result of a request in json
		/// </summary>
		/// <param name="request">The search request</param>
		/// <param name="formatted">If the json should be formatted or not</param>
		/// <returns>A json string</returns>
		public async Task<string?> GetProductsJsonString(ProductRequest request, bool formatted)
        {
            List<Product> products = (await GetProductsAsync(request)).ToList();

            try
            {
				return formatted ? MakeJSNOReadable(JsonConvert.SerializeObject(products)) : JsonConvert.SerializeObject(products);
			}
            catch
            {
                return null;
            }
        }

        #endregion
        #endregion
        #region Brand
        #region Get
        /// <summary>
        /// Gets all brands in the DB
        /// </summary>
        /// <returns>A list of all brands</returns>
        public async Task<IEnumerable<Brand>?> GetBrandsAsync()
        {
            using HttpResponseMessage response = await HttpClient.GetAsync("api/Brands");
#if DEBUG
            Log(await response.Content.ReadAsStringAsync());
#endif
            try
            {
                return JsonConvert.DeserializeObject<List<Brand>>(await response.Content.ReadAsStringAsync());
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Gets a brand with the provided id
        /// </summary>
        /// <param name="id">The provided id</param>
        /// <returns>The brand</returns>
        public async Task<Brand?> GetBrandAsync(int id)
        {
            using HttpResponseMessage response = await HttpClient.GetAsync($"api/Brands/{id}");
#if DEBUG
            Log(await response.Content.ReadAsStringAsync());
#endif
            try
            {
                return JsonConvert.DeserializeObject<Brand>(await response.Content.ReadAsStringAsync());
            }
            catch
            {
                return null;
            }
        }
		/// <summary>
		/// Gets the brands associated with the object provided and a name
		/// </summary>
		/// <param name="option">The type of object</param>
		/// <param name="name">The name of the object</param>
		/// <returns>The Brand</returns>
		public async Task<Brand?> GetBrandAsync(BrandGetOptions option, string name)
		{
			using HttpResponseMessage response = await HttpClient.GetAsync($"api/Brands/{option}/{name}");
#if DEBUG
			Log(await response.Content.ReadAsStringAsync());
#endif
			try
			{
				return JsonConvert.DeserializeObject<Brand>(await response.Content.ReadAsStringAsync());
			}
			catch
			{
				return null;
			}
		}
		#endregion
		#endregion
		#region ItemGroup
		#region Get
		/// <summary>
		/// Gets all itemgroups in the DB
		/// </summary>
		/// <returns>A list of all itemgroups</returns>
		public async Task<IEnumerable<ItemGroup>?> GetItemGroupsAsync()
        {
            using HttpResponseMessage response = await HttpClient.GetAsync("api/ItemGroups");
#if DEBUG
            Log(await response.Content.ReadAsStringAsync());
#endif
            try
            {
                return JsonConvert.DeserializeObject<List<ItemGroup>>(await response.Content.ReadAsStringAsync());
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Gets an itemgroup with the provided id
        /// </summary>
        /// <param name="id">The provided id</param>
        /// <returns>The itemgroup now in the DB</returns>
        public async Task<ItemGroup?> GetItemGroupAsync(int id)
        {
            using HttpResponseMessage response = await HttpClient.GetAsync($"api/ItemGroups/{id}");
#if DEBUG
            Log(await response.Content.ReadAsStringAsync());
#endif
            try
            {
                return JsonConvert.DeserializeObject<ItemGroup>(await response.Content.ReadAsStringAsync());
            }
            catch
            {
                return null;
            }
        }
		/// <summary>
		/// Gets the itemgroup associated with the object provided and a name
		/// </summary>
		/// <param name="option">The type of object</param>
		/// <param name="name">The name of the object</param>
		/// <returns>The Itemgroup</returns>
		public async Task<ItemGroup?> GetItemGroupAsync(ItemGroupGetOptions option, string name)
		{
			using HttpResponseMessage response = await HttpClient.GetAsync($"api/ItemGroups/{option}/{name}");
#if DEBUG
			Log(await response.Content.ReadAsStringAsync());
#endif
			try
			{
				return JsonConvert.DeserializeObject<ItemGroup>(await response.Content.ReadAsStringAsync());
			}
			catch
			{
				return null;
			}
		}
		#endregion
		#endregion
		#region Alias
		#region Get
		/// <summary>
		/// Gets a list of all aliases
		/// </summary>
		/// <returns>A list of all aliases</returns>
		public async Task<IEnumerable<BrandAlias>?> GetAliasesAsync()
        {
            using HttpResponseMessage response = await HttpClient.GetAsync("api/BrandAlias");
#if DEBUG
            Log(await response.Content.ReadAsStringAsync());
#endif
            try
            {
                return JsonConvert.DeserializeObject<List<BrandAlias>>(await response.Content.ReadAsStringAsync());
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Gets an alias with the provided Id
        /// </summary>
        /// <param name="id">The provided Id</param>
        /// <returns>The Alias</returns>
        public async Task<BrandAlias?> GetAliasAsync(int id)
        {
            using HttpResponseMessage response = await HttpClient.GetAsync($"api/BrandAlias/{id}");
#if DEBUG
            Log(await response.Content.ReadAsStringAsync());
#endif
            try
            {
                return JsonConvert.DeserializeObject<BrandAlias>(await response.Content.ReadAsStringAsync());
            }
            catch
            {
                return null;
            }
        }
		/// <summary>
		/// Gets the alias associated with the object provided and a name
		/// </summary>
		/// <param name="option">The type of object</param>
		/// <param name="name">The name of the object</param>
		/// <returns>The Alias</returns>
		public async Task<IEnumerable<BrandAlias>?> GetAliasAsync(AliasGetOptions option, string name)
		{
			using HttpResponseMessage response = await HttpClient.GetAsync($"api/BrandAlias/{option}/{name}");
#if DEBUG
			Log(await response.Content.ReadAsStringAsync());
#endif
			try
			{
				switch (option)
				{
					case AliasGetOptions.Brand: return JsonConvert.DeserializeObject<IEnumerable<BrandAlias>?>(await response.Content.ReadAsStringAsync());
					case AliasGetOptions.Alias: return new List<BrandAlias>() { JsonConvert.DeserializeObject<BrandAlias?>(await response.Content.ReadAsStringAsync()) };
					default: throw new Exception("Could not determine option");
				}
			}
			catch
			{
				return null;
			}
		}
		#endregion
		#endregion
		#region Counter
		#region Get
		/// <summary>
		/// Gets the count of products associated with the object
		/// </summary>
		/// <param name="options">The type of object</param>
		/// <param name="name">The name of the object</param>
		/// <returns>The count</returns>
		public async Task<int?> GetCountAsync(CounterGetOptions options, string name)
		{
			using HttpResponseMessage response = await HttpClient.GetAsync($"api/Count/{options}/{name}");
#if DEBUG
			Log(await response.Content.ReadAsStringAsync());
#endif
			try
			{
				return int.Parse(await response.Content.ReadAsStringAsync());
			}
			catch
			{
				return null;
			}
		}
		#endregion
		#endregion
		#region Common [DELETE, PUT, POST]
		/// <summary>
		/// Deletes an object in the DB
		/// </summary>
		/// <param name="option">The type of object</param>
		/// <param name="id">The id of the object in the DB</param>
		/// <returns>The statuscode</returns>
		public async Task<bool> DeleteAsync(DeleteOptions option, int id)
		{
			using HttpResponseMessage response = await HttpClient.DeleteAsync($"api/{option}/{id}");
#if DEBUG
			Log("StatusCode: " + response.IsSuccessStatusCode);
#endif
			return response.IsSuccessStatusCode;
		}
		/// <summary>
		/// Puts an object into the DB
		/// </summary>
		/// <param name="option">The type of object</param>
		/// <param name="obj">The item itself</param>
		/// <param name="id">The id of the object</param>
		/// <returns>The statuscode</returns>
		public async Task<bool> PutAsync(PutOptions option, object obj, int id)
		{
			using HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"api/{option}/{id}", obj);
#if DEBUG
			Log("StatusCode: " + response.IsSuccessStatusCode);
#endif
			return response.IsSuccessStatusCode;
		}
		/// <summary>
		/// Posts an object to the DB
		/// </summary>
		/// <param name="option">The type of the object</param>
		/// <param name="obj">The object to be posted</param>
		public async Task<bool> PostAsync(PostOptions option, object obj)
		{
			using HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"api/{option}", obj);
#if DEBUG
			Log(await response.Content.ReadAsStringAsync() + " && StatusCode: " + response.IsSuccessStatusCode);
#endif
			return response.IsSuccessStatusCode;
		}
		#endregion

		#region Obsolete
		#region Products
		#region Post
		[Obsolete]
		public async Task<bool> PostProductAsync(Product product)
		{
			using HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"api/Products", product);
#if DEBUG
			Log(await response.Content.ReadAsStringAsync() + " && StatusCode: " + response.IsSuccessStatusCode);
#endif
			return response.IsSuccessStatusCode;
		}
		#endregion
		#region Put
		[Obsolete]
		public async Task<bool> PutProductAsync(Product product)
		{
			using HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"api/Products/{product.PrdId}", product);
#if DEBUG
			Log(await response.Content.ReadAsStringAsync() + " && StatusCode: " + response.IsSuccessStatusCode);
#endif
			return response.IsSuccessStatusCode;
		}
		#endregion
		#region Delete
		[Obsolete]
		public async Task<bool> DeleteProductAsync(int id)
		{
			using HttpResponseMessage response = await HttpClient.DeleteAsync($"api/Products/{id}");
#if DEBUG
			Log(await response.Content.ReadAsStringAsync() + " && StatusCode: " + response.IsSuccessStatusCode);
#endif
			return response.IsSuccessStatusCode;
		}
		#endregion
		#endregion
		#region Brand
		#region Get
		[Obsolete]
		public async Task<Brand?> GetBrandBasedAliasAsync(string aliasName)
		{
			using HttpResponseMessage response = await HttpClient.GetAsync($"api/Brands/Alias/{aliasName}");
#if DEBUG
			Log(await response.Content.ReadAsStringAsync());
#endif
			try
			{
				return JsonConvert.DeserializeObject<Brand>(await response.Content.ReadAsStringAsync());
			}
			catch
			{
				return null;
			}
		}
		[Obsolete]
		public async Task<Brand?> GetBrandBasedNameAsync(string brandName)
		{
			using HttpResponseMessage response = await HttpClient.GetAsync($"api/Brands/Brand/{brandName}");
#if DEBUG
			Log(await response.Content.ReadAsStringAsync());
#endif
			try
			{
				return JsonConvert.DeserializeObject<Brand>(await response.Content.ReadAsStringAsync());
			}
			catch
			{
				return null;
			}
		}
		#endregion
		#region Post
		[Obsolete]
		public async Task<Brand?> PostBrandAsync(Brand brand)
		{
			using HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"api/Brands", brand);
#if DEBUG
			Log(await response.Content.ReadAsStringAsync() + " && StatusCode: " + response.IsSuccessStatusCode);
#endif
			try
			{
				return JsonConvert.DeserializeObject<Brand>(await response.Content.ReadAsStringAsync());
			}
			catch
			{
				return null;
			}
		}
		#endregion
		#region Put
		[Obsolete]
		public async Task<bool> PutBrandAsync(Brand brand)
		{
			using HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"api/Brands/{brand.BrdId}", brand);
#if DEBUG
			Log(await response.Content.ReadAsStringAsync() + " && StatusCode: " + response.IsSuccessStatusCode);
#endif
			return response.IsSuccessStatusCode;
		}
		#endregion
		#region Delete
		[Obsolete]
		public async Task<bool> DeleteBrandAsync(int id)
		{
			using HttpResponseMessage response = await HttpClient.DeleteAsync($"api/Brands/{id}");
#if DEBUG
			Log(await response.Content.ReadAsStringAsync() + " && StatusCode: " + response.IsSuccessStatusCode);
#endif
			return response.IsSuccessStatusCode;
		}
		#endregion
		#endregion
		#region ItemGroup
		#region Get
		[Obsolete]
		public async Task<ItemGroup?> GetItemGroupBasedNameAsync(string groupName)
		{
			using HttpResponseMessage response = await HttpClient.GetAsync($"api/ItemGroups/ItemGroup/{groupName}");
#if DEBUG
			Log(await response.Content.ReadAsStringAsync());
#endif
			try
			{
				return JsonConvert.DeserializeObject<ItemGroup>(await response.Content.ReadAsStringAsync());
			}
			catch
			{
				return null;
			}
		}
		#endregion
		#region Post
		[Obsolete]
		public async Task<ItemGroup?> PostItemGroupAsync(ItemGroup itemGroup)
		{
			using HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"api/ItemGroups", itemGroup);
#if DEBUG
			Log(await response.Content.ReadAsStringAsync() + " && StatusCode: " + response.IsSuccessStatusCode);
#endif
			try
			{
				return JsonConvert.DeserializeObject<ItemGroup>(await response.Content.ReadAsStringAsync());
			}
			catch
			{
				return null;
			}
		}
		#endregion
		#region Put
		[Obsolete]
		public async Task<bool> PutItemGroupAsync(ItemGroup itemGroup)
		{
			using HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"api/ItemGroups/{itemGroup.GrpId}", itemGroup);
#if DEBUG
			Log(await response.Content.ReadAsStringAsync() + " && StatusCode: " + response.IsSuccessStatusCode);
#endif
			return response.IsSuccessStatusCode;
		}
		#endregion
		#region Delete
		[Obsolete]
		public async Task<bool> DeleteItemGroupAsync(int id)
		{
			using HttpResponseMessage response = await HttpClient.DeleteAsync($"api/ItemGroups/{id}");
#if DEBUG
			Log(await response.Content.ReadAsStringAsync() + " && StatusCode: " + response.IsSuccessStatusCode);
#endif
			return response.IsSuccessStatusCode;
		}
		#endregion
		#endregion
		#region Alias
		#region Get
		[Obsolete]
		public async Task<IEnumerable<BrandAlias>?> GetAliasBasedBrandNameAsync(string brandName)
		{
			using HttpResponseMessage response = await HttpClient.GetAsync($"api/BrandAlias/Brand/{brandName}");
#if DEBUG
			Log(await response.Content.ReadAsStringAsync());
#endif
			try
			{
				return JsonConvert.DeserializeObject<IEnumerable<BrandAlias>?>(await response.Content.ReadAsStringAsync());
			}
			catch
			{
				return null;
			}
		}
		[Obsolete]
		public async Task<BrandAlias?> GetAliasBasedAliasNameAsync(string alias)
		{
			using HttpResponseMessage response = await HttpClient.GetAsync($"api/BrandAlias/Alias/{alias}");
#if DEBUG
			Log(await response.Content.ReadAsStringAsync());
#endif
			try
			{
				return JsonConvert.DeserializeObject<BrandAlias?>(await response.Content.ReadAsStringAsync());
			}
			catch
			{
				return null;
			}
		}
		#endregion
		#region Post
		[Obsolete]
		public async Task<BrandAlias?> PostAliasAsync(BrandAlias alias)
		{
			using HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"api/BrandAlias", alias);
#if DEBUG
			Log(await response.Content.ReadAsStringAsync() + " && StatusCode: " + response.IsSuccessStatusCode);
#endif
			try
			{
				return JsonConvert.DeserializeObject<BrandAlias>(await response.Content.ReadAsStringAsync());
			}
			catch
			{
				return null;
			}
		}
		#endregion
		#region Put
		[Obsolete]
		public async Task<bool> PutAliasAsync(BrandAlias alias)
		{
			using HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"api/BrandAlias/{alias.AliId}", alias);
#if DEBUG
			Log(await response.Content.ReadAsStringAsync() + " && StatusCode: " + response.IsSuccessStatusCode);
#endif
			return response.IsSuccessStatusCode;
		}
		#endregion
		#region Delete
		[Obsolete]
		public async Task<bool> DeleteAliasAsync(int id)
		{
			using HttpResponseMessage response = await HttpClient.DeleteAsync($"api/BrandAlias/{id}");
#if DEBUG
			Log(await response.Content.ReadAsStringAsync() + " && StatusCode: " + response.IsSuccessStatusCode);
#endif
			return response.IsSuccessStatusCode;
		}
		#endregion
		#endregion
		#region Counter
		[Obsolete]
		public async Task<int?> GetCountBasedBrandNameAsync(string brandName)
		{
			using HttpResponseMessage response = await HttpClient.GetAsync($"api/Count/Brand/{brandName}");
#if DEBUG
			Log(await response.Content.ReadAsStringAsync());
#endif
			try
			{
				return int.Parse(await response.Content.ReadAsStringAsync());
			}
			catch
			{
				return null;
			}
		}
		[Obsolete]
		public async Task<int?> GetCountBasedItemGroupNameAsync(string itemGroupName)
		{
			using HttpResponseMessage response = await HttpClient.GetAsync($"api/Count/ItemGroup/{itemGroupName}");
#if DEBUG
			Log(await response.Content.ReadAsStringAsync());
#endif
			try
			{
				return int.Parse(await response.Content.ReadAsStringAsync());
			}
			catch
			{
				return null;
			}
		}
		#endregion
		#endregion

		#region Private
		/// <summary>
		/// Encodes the string to avoid char causing issues with HTML
		/// </summary>
		/// <param name="str">The string to encode</param>
		/// <returns>The encoded string</returns>
		private string HTMLSafeStringEncode(string str)
        {
            str = str.Replace("+", "&#43;");
            str = str.Replace("/", "%2F;");
            str = str.Replace("-", "%2D;");

            return str;
        }
		/// <summary>
		/// Decodes the string to avoid char causing issues with HTML
		/// </summary>
		/// <param name="str">The string to decode</param>
		/// <returns>The decoded string</returns>
		private string HTMLSafeStringDecode(string str)
        {
            str = str.Replace("&#43;", "+");
            str = str.Replace("%2F;", "/");
            str = str.Replace("%2D;", "-");

            return str;
        }
        /// <summary>
        /// Formats a json, making it more readable for the human eye
        /// </summary>
        /// <param name="json">The json to make readable</param>
        /// <returns>The readable string</returns>
        private string MakeJSNOReadable(string json)
        {
            json = json.Replace("{", "{\n");
            json = json.Replace("}", "\n}");
            json = json.Replace(",", ",\n");

            return json;
        }
        #endregion
    }
}
