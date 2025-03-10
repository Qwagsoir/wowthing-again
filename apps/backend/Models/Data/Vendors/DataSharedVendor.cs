﻿namespace Wowthing.Backend.Models.Data.Vendors;

public class DataSharedVendor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Note { get; set; }
    public string[] Tags { get; set; }
    public Dictionary<string, string[]> Locations { get; set; }
    public DataVendorItem[] Sells { get; set; }
}
