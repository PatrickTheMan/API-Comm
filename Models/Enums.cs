namespace B2S_API_Comm.Models
{
    public enum ProductGetOptions
    {
        Brand,
        ItemGroup,
        EAN,
        ProductNumber
    }

	public enum CounterGetOptions
	{
		Brand,
		ItemGroup
	}

	public enum AliasGetOptions
	{
		Brand,
		Alias
	}

	public enum BrandGetOptions
	{
		Alias
	}

	public enum ItemGroupGetOptions
	{
		ItemGroup
	}

	public enum DeleteOptions
	{
		Products,
		Brands,
		BrandAlias,
		ItemGroups
	}

	public enum PutOptions
	{
		Products,
		Brands,
		BrandAlias,
		ItemGroups
	}

    public enum PostOptions
    {
        Products,
        Brands,
        BrandAlias,
        ItemGroups
    }
}
