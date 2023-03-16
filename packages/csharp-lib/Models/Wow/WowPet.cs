﻿using System.ComponentModel.DataAnnotations;

namespace Wowthing.Lib.Models.Wow;

public class WowPet
{
    [Key]
    public int Id { get; set; }

    public int CreatureId { get; set; }
    public int SpellId { get; set; }
    public int Flags { get; set; }
    public short PetType { get; set; }
    public short SourceType { get; set; }
    public int ItemId { get; set; }

    public WowPet(int id)
    {
        Id = id;
    }
}
