using System;
using System.Collections.Generic;

namespace B2S_API_Comm.Domain;

public partial class BrandAlias
{
    public int AliId { get; set; }

    public string AliAlias { get; set; } = null!;

    public int? BrdId { get; set; }

}
