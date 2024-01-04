namespace B2S_API_Comm.Domain
{
    public class ProductRequest
    {
        #region Constructors
        /// <summary>
        /// Base Constructor
        /// </summary>
        public ProductRequest() { }
        /// <summary>
        /// Constructor using another base req
        /// </summary>
        /// <param name="baseRequest">BaseRequest to copy from</param>
		public ProductRequest(ProductRequest baseRequest) 
        {
            this.Brand = baseRequest.Brand;
            this.ItemGroup = baseRequest.ItemGroup;
            this.EAN = baseRequest.EAN;
            this.UPC = baseRequest.UPC;
            this.ProductNumber = baseRequest.ProductNumber;
            this.TypeNumber = baseRequest.TypeNumber;
            this.Index = baseRequest.Index;
            this.Amount = baseRequest.Amount;
        }
		#endregion
		/// <summary>
		/// The Brand name for the product
		/// </summary>
		public string? Brand { get; set; }
        /// <summary>
        /// The ItemGroup name for the product
        /// </summary>
        public string? ItemGroup { get; set; }
        /// <summary>
        /// The EAN number for the product
        /// </summary>
        public string? EAN { get; set; }
        /// <summary>
        /// The UPC number for the product
        /// </summary>
        public string? UPC { get; set; }
        /// <summary>
        /// Potentially the productnumber
        /// </summary>
        public string? ProductNumber { get; set; }
		/// <summary>
		/// The typenumber for the product
		/// </summary>
		public string? TypeNumber { get; set; }
		/// <summary>
		/// Offset from zero
		/// </summary>
		public int Index { get; set; } = 0;
        /// <summary>
        /// Amount to get
        /// </summary>
        public int Amount { get; set; } = 1;
    }
}
