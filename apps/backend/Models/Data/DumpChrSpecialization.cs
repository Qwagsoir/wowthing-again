﻿using CsvHelper.Configuration.Attributes;
using Wowthing.Lib.Enums;

namespace Wowthing.Backend.Models.Data;

// ReSharper disable InconsistentNaming
public class DumpChrSpecialization
{
    public short ClassID { get; set; }
    public short ID { get; set; }
    public short PrimaryStatPriority { get; set; }
    public short OrderIndex { get; set; }
    public WowRole Role { get; set; }

    [Name("FemaleName_lang")]
    public string FemaleName { get; set; }

    [Name("Name_lang")]
    public string Name { get; set; }
}
