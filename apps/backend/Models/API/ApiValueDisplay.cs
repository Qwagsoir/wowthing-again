﻿namespace Wowthing.Backend.Models.API
{
    public class ApiValueDisplay
    {
        public int Value { get; set; }
        [JsonProperty("display_string")]
        public string DisplayString { get; set; }
    }
}
