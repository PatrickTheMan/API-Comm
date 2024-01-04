using System;
using System.Collections.Generic;

namespace B2S_API_Comm.Domain;

public partial class Product
{
    public Product() { }
    public Product(Product product) 
    {
        this.PrdId = product.PrdId;
        this.PrdName = product.PrdName;
        this.PrdProductNumber = product.PrdProductNumber;
        this.PrdTypeNumber = product.PrdTypeNumber;
        this.PrdEanGlr = product.PrdEanGlr;
        this.PrdUpc = product.PrdUpc;
        this.BrdId = product.BrdId;
        this.GrpId = product.GrpId;
        this.PrdId = product.PrdId;
        this.SpecJson = product.SpecJson;
    }

    public int PrdId { get; set; }

    public string? PrdName { get; set; }

    public string PrdProductNumber { get; set; } = null!;

    public string? PrdTypeNumber { get; set; }

    public string? PrdEanGlr { get; set; }

    public string? PrdUpc { get; set; }

    public string? PrdProductText { get; set; }

    public int? GrpId { get; set; }

    public int? BrdId { get; set; }

    public string? SpecJson { get; set; }

}
