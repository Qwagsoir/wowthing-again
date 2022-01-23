﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wowthing.Lib.Models.Player
{
    public class PlayerAccountTransmogSources
    {
        [Key, ForeignKey("Account")]
        public int AccountId { get; set; }
        public PlayerAccount Account { get; set; }

        [Column(TypeName = "jsonb")]
        public Dictionary<int, List<string>> Appearances { get; set; }
    }
}
