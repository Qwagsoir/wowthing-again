﻿namespace Wowthing.Backend.Models.Data.Progress
{
    public class DataProgress : ICloneable, IDataCategory
    {
        public int MinimumLevel { get; set; }
        public string Name { get; set; }
        public string RequiredQuestId { get; set; }
        public List<DataProgressGroup> Groups { get; set; }
        
        public object Clone()
        {
            return new DataProgress
            {
                MinimumLevel = MinimumLevel,
                Name = Name,
                RequiredQuestId = RequiredQuestId,
                Groups = Groups,
            };
        }
    }
}
