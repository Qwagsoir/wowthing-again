﻿using Newtonsoft.Json;
using Wowthing.Lib.Enums;

namespace Wowthing.Lib.Models.Wow
{
    [Index(nameof(ItemId))]
    [Index(nameof(PetSpeciesId))]
    public class WowAuction
    {
        public long BidPrice { get; set; }
        public long BuyoutPrice { get; set; }
        
        public int ConnectedRealmId { get; set; }
        public int AuctionId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }

        public WowAuctionTimeLeft TimeLeft { get; set; }
        public short Context { get; set; }
        public short PetBreedId { get; set; }
        public short PetLevel { get; set; }
        public short PetQuality { get; set; }
        public short PetSpeciesId { get; set; }
        
        public List<int> BonusIds { get; set; }
        public List<int> ModifierValues { get; set; }
        public List<short> ModifierTypes { get; set; }

        [JsonIgnore]
        public long UsefulPrice => BidPrice > 0 ? BidPrice : BuyoutPrice;
    }
}
